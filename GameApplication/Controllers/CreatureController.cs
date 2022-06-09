using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using GameApplication.Models;
using GameApplication.Models.ViewModels;
using System.Web.Script.Serialization;

namespace GameApplication.Controllers
{
    public class CreatureController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CreatureController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44348/api/");
        }


        // GET: Creature/List
        public ActionResult List()
        {
            //Communicate with creature data api to retrieve a list of creatures
            //curl https://localhost:44348/api/creaturedata/listcreatures

            string url = "creaturedata/listcreatures";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CreatureDto> creatures = response.Content.ReadAsAsync<IEnumerable<CreatureDto>>().Result;

            return View(creatures);
        }

        // GET: Creature/Details/5
        public ActionResult Details(int id)
        {
            DetailsCreature ViewModel = new DetailsCreature();

            //objective => communicate with our creature data api to retrieve a creature

            string url = "creaturedata/findcreature/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CreatureDto SelectedCreature = response.Content.ReadAsAsync<CreatureDto>().Result;
            Debug.WriteLine("creature received : ");
            Debug.WriteLine(SelectedCreature.CreatureName);

            ViewModel.SelectedCreature = SelectedCreature;

            //show associated Dungeons with this creature
            url = "Dungeondata/listdungeonsforcreature/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DungeonDto> LiveinDungeons = response.Content.ReadAsAsync<IEnumerable<DungeonDto>>().Result;

            ViewModel.LiveinDungeons = LiveinDungeons;

            url = "Dungeondata/listdungeonsnothavingcreature/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DungeonDto> AvailableDungeons = response.Content.ReadAsAsync<IEnumerable<DungeonDto>>().Result;

            ViewModel.AvailableDungeons = AvailableDungeons;


            return View(ViewModel);
        }

        //POST: Creature/Associate/{creatureid}
        [HttpPost]
        public ActionResult Associate(int id, int DungeonID)
        {
            string url = "creaturedata/associatecreaturewithdungeon/" + id + "/" + DungeonID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            return RedirectToAction("Details/" + id);
        }

        //POST: Creature/UnAssociate/{id}?DungeonID={dungeonID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int DungeonID)
        {
            Debug.WriteLine("Attempting to unassociate creature :" + id + " with dungeon: " + DungeonID);

            //call our api to associate creature with the Dungeon
            string url = "creaturedata/unassociatecreaturewithdungeon/" + id + "/" + DungeonID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        public ActionResult Error()
        {
            return View();
        }

        // GET: Creature/New
        public ActionResult New()
        {
            //Will show all info about all races
            //GET api/racedata/listraces
            
            string url = "racedata/listraces";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<RaceDto> RaceOptions = response.Content.ReadAsAsync<IEnumerable<RaceDto>>().Result;

            return View(RaceOptions);
        }

        // POST: Creature/Create
        [HttpPost]
        public ActionResult Create(Creature creature)
        {
            //objective => add a new creature into our system using the API
            string url = "creaturedata/addcreature";


            string jsonpayload = jss.Serialize(creature);

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

        // GET: Creature/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCreature ViewModel = new UpdateCreature();

            //creature information (existing)
            string url = "creaturedata/findcreature/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CreatureDto SelectedCreature = response.Content.ReadAsAsync<CreatureDto>().Result;
            ViewModel.SelectedCreature = SelectedCreature;

            // all races to choose from when this creature updated
            //creature information (existing)
            url = "racedata/listraces/";
            response = client.GetAsync(url).Result;
            IEnumerable<RaceDto> RaceOptions = response.Content.ReadAsAsync<IEnumerable<RaceDto>>().Result;

            ViewModel.RaceOptions = RaceOptions;

            return View(ViewModel);
        }

        // POST: Creature/Update/5
        [HttpPost]
        public ActionResult Update(int id, Creature creature, HttpPostedFileBase CreaturePic)
        {

            string url = "creaturedata/updatecreature/" + id;
            string jsonpayload = jss.Serialize(creature);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Debug.WriteLine(content);

            //update request logic
            if (response.IsSuccessStatusCode && CreaturePic != null)
            {
                //Sending image data
                url = "CreatureData/UploadCreaturePic/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(CreaturePic.InputStream);
                requestcontent.Add(imagecontent, "CreaturePic", CreaturePic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Creature/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "creaturedata/findcreature/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CreatureDto selectedcreature = response.Content.ReadAsAsync<CreatureDto>().Result;
            return View(selectedcreature);
        }

        // POST: Creature/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "creaturedata/deletecreature/" + id;
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
