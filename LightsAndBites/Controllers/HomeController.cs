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

        private string EventToken = ApiKey.eventKey;
        public string Token => EventToken;
        public string userProfileHometown = "Milwaukee";


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
            List<City> cities = _context.Cities.ToList();
            foreach (City c in cities)
            {
                GetAllDailyEvents(c);
                GetAllBarsAndRestaurantsDaily(c);
            }
            //City chicagoCity = new City();
            //chicagoCity.Id = 4;
            //City milwaukeeCity = new City();
            //milwaukeeCity.Id = 3;
            //GetAllDailyEvents(milwaukeeCity);
            //GetAllDailyEvents(chicagoCity);
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
        public void GetAllDailyEvents(City city)
        { 

            GetDailyEvents("music", city);
            GetDailyEvents("conference", city);
            GetDailyEvents("comedy",city);
            GetDailyEvents("festivals", city);
            GetDailyEvents("family_fun_kids", city);
            GetDailyEvents("community", city);
            GetDailyEvents("observances", city);
            GetDailyEvents("performing_arts", city);
        }

        public void GetAllBarsAndRestaurantsDaily(City city)
        {
            GetGoogleData("Sports", city, "Bar");
            GetGoogleData("College", city, "Bar");
            GetGoogleData("Specialty", city, "Bar");
            GetGoogleData("Irish Pub", city, "Bar");
            GetGoogleData("Dive", city, "Bar");

            GetGoogleData("Chinese", city, "Restaurant");
            GetGoogleData("Italian", city, "Restaurant");
            GetGoogleData("American", city, "Restaurant");
            GetGoogleData("Fine Dining", city, "Restaurant");
            GetGoogleData("Mexican", city, "Restaurant");
        }
        public void GetDailyEvents(string eventType, City city)
        {
            string html = string.Empty;

            string url = @"http://api.eventful.com/json/events/search?app_key=" + Token + "&category=" + eventType + "&location=" + city.CityName + "&sort_order=popularity&page_number=1";

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
                try
                {
                    eventt.PictureUrl = ((j["image"]["medium"]["url"]).ToString());
                }
                catch 
                {
                    eventt.PictureUrl = "Default";
                }

                switch (eventType)
                {
                    case "music":
                        eventt.CategoryId = 11;
                        break;
                    case "festivals":
                        eventt.CategoryId = 12;
                        break;
                    case "comedy":
                        eventt.CategoryId = 13;
                        break;
                    case "observances":
                        eventt.CategoryId = 14;
                        break;
                    case "performing_arts":
                        eventt.CategoryId = 18;
                        break;
                    case "family_fun_kids":
                        eventt.CategoryId = 15;
                        break;
                    case "community":
                        eventt.CategoryId = 16;
                        break;
                    case "conference":
                        eventt.CategoryId = 17;
                        break;
                }

                eventt.CityId = city.Id;
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
                restaurant.CategoryId = 7;
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
                restaurant.CategoryId = 8;
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
                restaurant.CategoryId = 6;
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
                restaurant.CategoryId = 10;
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
                restaurant.CategoryId = 9;
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
                bar.CategoryId = 1;
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
                bar.CategoryId = 2;
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
                bar.CategoryId = 3;
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
                bar.CategoryId = 4;
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
                bar.CategoryId = 5;
                bar.CityId = 1;
                _context.Bars.Add(bar);
            }
            _context.SaveChanges();
        }

        private void GetGoogleData(string keyWord, City city, string type)
        {
            string data = string.Empty;
            string url = @"https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=" + ApiKey.mapsKey + @"&location=" + city.Latitude + @"," + city.Longitude + @"&keyword=" + keyWord + @"&type=" + type.ToLower() + "&radius=5000";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }

            JObject returnData = JObject.Parse(data);
            List<JObject> returnList = new List<JObject>();

            foreach (JObject j in returnData["results"])
            {
                if (type == "Bar")
                {
                    Bar newBar = new Bar();
                    newBar.CategoryId = _context.Categories.Where(c => c.CategoryType == keyWord).Where(c => c.CategoryName == type).Select(c => c.Id).Single();
                    newBar.Category = _context.Categories.Where(c => c.Id == newBar.CategoryId).Single();
                    newBar.Latitude = Convert.ToDouble(j["geometry"]["location"]["lat"]);
                    newBar.Longitude = Convert.ToDouble(j["geometry"]["location"]["lng"]);
                    newBar.Likes = 0;
                    newBar.Dislikes = 0;
                    newBar.Name = j["name"].ToString();
                    newBar.CityId = _context.Cities.Where(c => c.CityName == city.CityName).Select(c => c.Id).Single();
                    newBar.City = _context.Cities.Where(c => c.Id == newBar.CityId).Single();
                    try
                    {
                        newBar.CardPhoto = j["photos"][0]["photo_reference"].ToString();
                    }
                    catch
                    {
                        newBar.CardPhoto = null;
                    }

                    var foundMatchingRestaurant = _context.Restaurants.Where(r => r.Latitude == newBar.Latitude).Where(r => r.Longitude == newBar.Longitude).Where(r => r.Name == newBar.Name).FirstOrDefault();
                    if (foundMatchingRestaurant == null)
                    {
                        _context.Bars.Add(newBar);
                    }
                }
                else if (type == "Restaurant")
                {
                    Restaurant newRestaurant = new Restaurant();
                    newRestaurant.CategoryId = _context.Categories.Where(c => c.CategoryType == keyWord).Where(c => c.CategoryName == type).Select(c => c.Id).Single();
                    newRestaurant.Category = _context.Categories.Where(c => c.Id == newRestaurant.CategoryId).Single();
                    newRestaurant.Latitude = Convert.ToDouble(j["geometry"]["location"]["lat"]);
                    newRestaurant.Longitude = Convert.ToDouble(j["geometry"]["location"]["lng"]);
                    newRestaurant.Likes = 0;
                    newRestaurant.Dislikes = 0;
                    newRestaurant.Name = j["name"].ToString();
                    newRestaurant.CityId = _context.Cities.Where(c => c.CityName == city.CityName).Select(c => c.Id).Single();
                    newRestaurant.City = _context.Cities.Where(c => c.Id == newRestaurant.CityId).Single();
                    try
                    {
                        newRestaurant.CardPhoto = j["photos"][0]["photo_reference"].ToString();
                    }
                    catch
                    {
                        newRestaurant.CardPhoto = null;
                    }

                    var foundMatchingRestaurant = _context.Restaurants.Where(r => r.Latitude == newRestaurant.Latitude).Where(r => r.Longitude == newRestaurant.Longitude).Where(r => r.Name == newRestaurant.Name).FirstOrDefault();
                    if (foundMatchingRestaurant == null)
                    {
                        _context.Restaurants.Add(newRestaurant);
                    }
                }

                else
                {
                    throw new ArgumentException("Must be searching either a bar or restaurant.");
                }
                _context.SaveChanges();
            }
        }


    }
}

