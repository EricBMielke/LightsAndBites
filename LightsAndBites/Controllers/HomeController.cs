using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LightsAndBites.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using LightsAndBites.ApiKeys;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using LightsAndBites.Data;


namespace LightsAndBites.Controllers
{
    public class HomeController : Controller
    {
        private string token = ApiKey.eventKey;
        public string Token => token;

        private ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
                       
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void GetEvents()
        {
            string html = string.Empty;
            string url = @"https://api.predicthq.com/v1/events/?access_token=" + Token + "&location=@43.0389,-87.90647&within=10mi@43.0389,-87.90647&category=sports";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            JObject o = JObject.Parse(html);

            foreach (JObject j in o["results"])
            {
                Events eventt = new Events();
                //eventt.Name = (j["title"]).ToString();
                eventt.Latitude = Convert.ToDouble((j["location"][0]));
                eventt.Longitude = Convert.ToDouble((j["location"][1]));
                eventt.Type = "concert";
                eventt.CityId = 1;
                //restaurant.CardPhoto = (j["photos"]["photo_reference"].ToString());
                _context.Events.Add(eventt);
            }
            _context.SaveChanges();
        }
    }
}

