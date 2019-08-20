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
        public string userProfileHometown = "Milwaukee";


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
            GetAllDailyEvents();
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
        public void GetAllDailyEvents()
        {
            GetDailyEvents("music");
            GetDailyEvents("conference");
            GetDailyEvents("comedy");
            GetDailyEvents("festivals");
            GetDailyEvents("family_fun_kids");
            GetDailyEvents("community");
            GetDailyEvents("outdoors_recreation");
            GetDailyEvents("performing_arts");
        }
        public void GetDailyEvents(string eventType)
        {
            string html = string.Empty;
            string url = @"http://api.eventful.com/json/events/search?app_key=" + Token + "&category=" + eventType + "&location=" + userProfileHometown + "&sort_order=popularity&page_number=1";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            JObject o = JObject.Parse(html);

            foreach (JObject j in o["events"]["event"])
            {
                Events eventt = new Events();
                eventt.Name = (j["title"]).ToString();
                eventt.Latitude = Convert.ToDouble((j["latitude"]));
                eventt.Longitude = Convert.ToDouble((j["longitude"]));
                eventt.Type = eventType;
                eventt.StreetAddress = ((j["venue_address"]).ToString());
                switch (eventType)
                {
                    case "music":
                        eventt.CategoryId = "1";
                        break;
                    case "festivals":
                        eventt.CategoryId = "2";
                        break;
                    case "comedy":
                        eventt.CategoryId = "3";
                        break;
                    case "outdoor_recreation":
                        eventt.CategoryId = "4";
                        break;
                    case "performing_arts":
                        eventt.CategoryId = "8";
                        break;
                    case "family_fun_kids":
                        eventt.CategoryId = "5";
                        break;
                    case "community":
                        eventt.CategoryId = "6";
                        break;
                    case "conference":
                        eventt.CategoryId = "7";
                        break;
                }
                eventt.CityId = 1;
                if ((j["url"]) == null)    
                {
                    eventt.Website = ((j["venue_url"]).ToString());
                }
                else
                {
                    eventt.Website = ((j["url"]).ToString());
                }
                _context.Events.Add(eventt);
            }
            _context.SaveChanges();
        }

    }
}

