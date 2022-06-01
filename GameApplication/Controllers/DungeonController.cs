using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using GameApplication.Models;
using GameApplication.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace GameApplication.Controllers
{
    public class DungeonController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DungeonController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44348/api/");
        }

        // GET: Dungeon/List
        public ActionResult List()
        {
            //objective => communicate with Dungeon data to retrieve a list of Dungeons
            //curl https://localhost:44348/api/dungeondata/listdungeons

            string url = "dungeondata/listdungeons";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DungeonDto> Dungeons = response.Content.ReadAsAsync<IEnumerable<DungeonDto>>().Result;


            return View(Dungeons);
        }

        // GET: Dungeon/Details/5
        public ActionResult Details(int id)
        {
            DetailsDungeon ViewModel = new DetailsDungeon();

            //objective => communicate with our Dungeon data to retrieve a Dungeon

            string url = "dungeondata/findDungeon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DungeonDto SelectedDungeon = response.Content.ReadAsAsync<DungeonDto>().Result;

            ViewModel.SelectedDungeon = SelectedDungeon;

            //show all creatures lives in this dungeon
            url = "creaturedata/listcreaturesfordungeon/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CreatureDto> AvailableCreatures = response.Content.ReadAsAsync<IEnumerable<CreatureDto>>().Result;

            ViewModel.AvailableCreatures = AvailableCreatures;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Dungeon/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Dungeon/Create
        [HttpPost]
        public ActionResult Create(Dungeon Dungeon)
        {
            Debug.WriteLine("the json payload is :");
            //objective => add a new Dungeon into our system using the API
            string url = "dungeondata/adddungeon";


            string jsonpayload = jss.Serialize(Dungeon);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Dungeon/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "dungeondata/finddungeon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DungeonDto selectedDungeon = response.Content.ReadAsAsync<DungeonDto>().Result;
            return View(selectedDungeon);
        }

        // POST: Dungeon/Update/5
        [HttpPost]
        public ActionResult Update(int id, Dungeon Dungeon)
        {

            string url = "dungeondata/updatedungeon/" + id;
            string jsonpayload = jss.Serialize(Dungeon);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Dungeon/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "dungeondata/finddungeon/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DungeonDto selectedDungeon = response.Content.ReadAsAsync<DungeonDto>().Result;
            return View(selectedDungeon);
        }

        // POST: Dungeon/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "dungeondata/deletedungeon/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
