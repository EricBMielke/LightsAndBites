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

        private ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ////seeds
            //ItalianFood();
            //AmericanFood();
            //ChineseFood();
            //MexicanFood();
            //FineDining();
            //SportsBar();
            //CollegeBar();
            //SpecialtyBar();
            //IrishPub();
            //DiveBar();
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



        public void ItalianFood()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=food&type=bar&keyword=italian&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Restaurant restaurant = new Restaurant();
                restaurant.Name = (j["name"]).ToString();
                restaurant.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                restaurant.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                restaurant.Category = "7";
                restaurant.CityId = 1;
                //restaurant.CardPhoto = (j["photos"]["photo_reference"].ToString());
                _context.Restaurants.Add(restaurant);
            }
            _context.SaveChanges();
        }

        public void AmericanFood()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=food&keyword=american&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Restaurant restaurant = new Restaurant();
                restaurant.Name = (j["name"]).ToString();
                restaurant.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                restaurant.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                restaurant.Category = "8";
                restaurant.CityId = 1;
                //restaurant.CardPhoto = (j["photos"]["photo_reference"].ToString());
                _context.Restaurants.Add(restaurant);
            }
            _context.SaveChanges();

        }

        public void ChineseFood()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=food&keyword=chinese&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Restaurant restaurant = new Restaurant();
                restaurant.Name = (j["name"]).ToString();
                restaurant.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                restaurant.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                restaurant.Category = "6";
                restaurant.CityId = 1;
                //restaurant.CardPhoto = (j["photos"]["photo_reference"].ToString());
                _context.Restaurants.Add(restaurant);
            }
            _context.SaveChanges();

        }

        public void MexicanFood()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=food&keyword=mexican&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Restaurant restaurant = new Restaurant();
                restaurant.Name = (j["name"]).ToString();
                restaurant.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                restaurant.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                restaurant.Category = "10";
                restaurant.CityId = 1;
                //restaurant.CardPhoto = (j["photos"]["photo_reference"].ToString());
                _context.Restaurants.Add(restaurant);
            }
            _context.SaveChanges();

        }

        public void FineDining()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=food&keyword=fine+dining&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Restaurant restaurant = new Restaurant();
                restaurant.Name = (j["name"]).ToString();
                restaurant.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                restaurant.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                restaurant.Category = "9";
                restaurant.CityId = 1;
                //restaurant.CardPhoto = (j["photos"]["photo_reference"].ToString());
                _context.Restaurants.Add(restaurant);
            }
            _context.SaveChanges();

        }

        public void SportsBar()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=bar&keyword=sports&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Bar bar = new Bar();
                bar.Name = (j["name"]).ToString();
                bar.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                bar.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                bar.Category = "1";
                bar.CityId = 1;
                _context.Bars.Add(bar);
            }
            _context.SaveChanges();

        }

        public void CollegeBar()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=bar&keyword=college&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Bar bar = new Bar();
                bar.Name = (j["name"]).ToString();
                bar.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                bar.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                bar.Category = "2";
                bar.CityId = 1;
                _context.Bars.Add(bar);
            }
            _context.SaveChanges();
        }

        public void SpecialtyBar()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=bar&keyword=bar+games+specialty&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Bar bar = new Bar();
                bar.Name = (j["name"]).ToString();
                bar.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                bar.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                bar.Category = "3";
                bar.CityId = 1;
                _context.Bars.Add(bar);
            }
            _context.SaveChanges();
        }

        public void IrishPub()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=bar&keyword=irish&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Bar bar = new Bar();
                bar.Name = (j["name"]).ToString();
                bar.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                bar.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                bar.Category = "4";
                bar.CityId = 1;
                _context.Bars.Add(bar);
            }
            _context.SaveChanges();
        }

        public void DiveBar()
        {
            string html = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=43.038902,-87.906471&radius=5000&types=bar&keyword=dive&key=AIzaSyAWH5g0jm4nMECjEk0rvax9gCm0jCHP9hQ";

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
                Bar bar = new Bar();
                bar.Name = (j["name"]).ToString();
                bar.Latitude = Convert.ToDouble((j["geometry"]["location"]["lat"]));
                bar.Longitude = Convert.ToDouble((j["geometry"]["location"]["lng"]));
                bar.Category = "5";
                bar.CityId = 1;
                _context.Bars.Add(bar);
            }
            _context.SaveChanges();
        }


    }
}

