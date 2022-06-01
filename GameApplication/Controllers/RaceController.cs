using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using GameApplication.Models;
using System.Web.Script.Serialization;
using GameApplication.Models.ViewModels;

namespace GameApplication.Controllers
{
    public class RaceController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RaceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44348/api/");
        }

        // GET: Race/List
        public ActionResult List()
        {
            //objective => communicate with race data to list all races
            //curl https://localhost:44348/api/Racedata/listRaces

            string url = "racedata/listraces";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RaceDto> Race = response.Content.ReadAsAsync<IEnumerable<RaceDto>>().Result;

            return View(Race);
        }

        // GET: Race/Details/5
        public ActionResult Details(int id)
        {
            // Get Race data api to retrieve a race
            //curl https://localhost:44348/api/racedata/findrace/{id}

            DetailsRace ViewModel = new DetailsRace();

            string url = "racedata/findRace/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RaceDto SelectedRace = response.Content.ReadAsAsync<RaceDto>().Result;


            ViewModel.SelectedRace = SelectedRace;

            //All information about Creatures
            //Sending request to gather info about creatures which is related to a selected RaceID
            url = "creaturedata/listcreaturesforrace/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CreatureDto> RelatedCreatures = response.Content.ReadAsAsync<IEnumerable<CreatureDto>>().Result;

            ViewModel.RelatedCreatures = RelatedCreatures;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Race/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Race/Create
        [HttpPost]
        public ActionResult Create(Race Race)
        {
            Debug.WriteLine("the json payload is :");
            //objective => add a new Race into our system using the API
            //curl -H "Content-Type:application/json" -d @Race.json https://localhost:44348/api/racedata/addRace 
            string url = "racedata/addrace";

            string jsonpayload = jss.Serialize(Race);

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

        // GET: Race/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "racedata/findrace/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RaceDto selectedRace = response.Content.ReadAsAsync<RaceDto>().Result;
            return View(selectedRace);
        }

        // POST: Race/Update/5
        [HttpPost]
        public ActionResult Update(int id, Race Race)
        {

            string url = "racedata/updaterace/" + id;
            string jsonpayload = jss.Serialize(Race);
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

        // GET: Race/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "racedata/findrace/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RaceDto selectedRace = response.Content.ReadAsAsync<RaceDto>().Result;
            return View(selectedRace);
        }

        // POST: Race/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "racedata/deleterace/" + id;
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