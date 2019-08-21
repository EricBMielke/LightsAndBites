# LightsAndBites
Group capstone 


Note: for this to work, you will need to place a new class named "ApiKey.cs" in "LightsAndBites\LightsAndBites\APIKeys" (You must create this new folder). This file should look similar to the below:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightsAndBites.ApiKeys
{
    public static class ApiKey
    {
        public static string eventKey = "[APIKeyValue]";
		public static string mapsKey = "[APIKeyValue]";
    }
}
