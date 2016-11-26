using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace WhereIsTheCableCar
{
    public class ApiHelper
    {
        public static async Task<Livemap> GetLivemap()
        {
            var httpCliet = new HttpClient();

            var authToken = await GetJsonToken();
            var authHeader = AuthenticationHeaderValue.Parse($"Bearer {authToken.access_token}");

            httpCliet.DefaultRequestHeaders.Authorization = authHeader;

            var url =
                "https://api.vasttrafik.se/bin/rest.exe/v2/livemap?minx=11892060&maxx=12004475&miny=57678144&maxy=57705963&onlyRealtime=yes";

            var res = await httpCliet.GetAsync(url);
            var responseJson = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LiveMapApiJsonResponse>(responseJson).Livemap;
        }

        private static async Task<RootObject> GetJsonToken()
        {
            var key = "1HQIKfYjHW6f2RKT2bAfgNkKVKYa";
            var secret = "LzK145tEhRHOQPgnRJmYykh8euga";

            var bytes = Encoding.UTF8.GetBytes($"{key}:{secret}");
            var authToken = Convert.ToBase64String(bytes);


            var httpCliet = new HttpClient();
            httpCliet.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Basic {authToken}");


            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var content = new FormUrlEncodedContent(pairs);

            var res = await httpCliet.PostAsync("https://api.vasttrafik.se:443/token", content);

            Console.WriteLine($"status code {res.StatusCode}");

            var resData = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RootObject>(resData);
        }
    }

    public class RootObject
    {
        public string scope { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
    }

    public class Vehicle
    {
        public string X { get; set; }
        public string Y { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public string Prodclass { get; set; }
        public string Delay { get; set; }

        public new Vehicles GetType()
        {
            switch (Prodclass.ToLower())
            {
                   case "vas":
                    return Vehicles.WestTrain;
                   case "ldt":
                    return Vehicles.LongDistandeTrain;
                   case "reg":
                    return Vehicles.RegionalTrains;
                   case "bus":
                    return Vehicles.Bus;
                   case "boat":
                    return Vehicles.Boat;
                   case "tram":
                    return Vehicles.Tram;
                   case "taxi":
                    return Vehicles.Taxi;
                default:
                    throw new Exception("Invalid prodClass");
            }
        }
    }

    public class Livemap
    {
        public List<Vehicle> Vehicles { get; set; }
        public string Time { get; set; }
        public string Minx { get; set; }
        public string Maxx { get; set; }
        public string Miny { get; set; }
        public string Maxy { get; set; }
    }

    public class LiveMapApiJsonResponse
    {
        public Livemap Livemap { get; set; }
    }

    public enum Vehicles
    {
        WestTrain,
        LongDistandeTrain,
        RegionalTrains,
        Bus,
        Boat,
        Tram,
        Taxi
    }
}