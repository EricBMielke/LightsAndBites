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



NOTE: The same thing must be done with connection strings. There should be a "ConnectionStrings" folder created under the solution. This folder should have a "ConnectionString.cs" class file. This file should look like: 

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	namespace LightsAndBites.ConnectionStrings
	{
		public static class ConnectionString
		{
			public static string connectionString = "[connection string]";
		}
	}


In startup, the explicitly stated connection string can then be replaced with ConnectionString.connectionString.
