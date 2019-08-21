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
    public class EventsController : Controller
    {
        private string token = ApiKey.eventKey;
        public string Token => token;

        private ApplicationDbContext _context;
        public double userLongitude = 43.0389;
        public double userLatitude = -87.90647;

        public IActionResult Index()
        {
            return View();
        }
        public void GetAllDailyEvents()
        {
            GetDailyEvents("Concerts");
            GetDailyEvents("Politics");
            GetDailyEvents("Conferences");
            GetDailyEvents("Expos");
            GetDailyEvents("Festivals");
            GetDailyEvents("Performing-arts");
            GetDailyEvents("Sports");
            GetDailyEvents("Community");
        }
        public void GetDailyEvents(string eventType)
        {
            string html = string.Empty;
            string url = @"https://api.predicthq.com/v1/events/?access_token=" + Token + "&location=@"+userLongitude+","+userLatitude+ "&within=10mi@" + userLongitude + "," + userLatitude + "&category="+ eventType.ToLower();

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
                eventt.Name = (j["title"]).ToString();
                eventt.Latitude = Convert.ToDouble((j["location"][0]));
                eventt.Longitude = Convert.ToDouble((j["location"][1]));
                eventt.Type = eventType;
                eventt.StreetAddress = ((j["entities"]["formatted_address"]).ToString());
                eventt.CityId = 1;
                _context.Events.Add(eventt);
            }
            _context.SaveChanges();
        }
    }
}