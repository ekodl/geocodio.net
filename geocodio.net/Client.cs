using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace geocodio.net
{
    /// <summary>
    /// TODO LIST
    /// Add Moq in for http request
    /// assemble url
    /// implement http request
    /// close pull
    /// -------------
    /// implement geocode function
    /// implement reverse function
    /// </summary>





    public class Client
    {
        private const string secureConnectionUrl = "https://";
        private const string plainConnectionUrl = "http://";
        private const string urlBase = "api.geocod.io/v1";
        private const string urlApi = "?api_key=";
        private List<string> acceptedActions = new List<string>() { "geocode", "reverse" };
        private HttpClient client = new HttpClient();

        public string APIKey { get; set; }
        
        public string BaseUrl { get; }
        
        public string UrlEnd { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="apiKey">Personal API Key from geocod.io</param>
        /// <param name="secureConnection">Determines whether to use http or https.  Default is https, 
        /// per the geocod.io spec (and common sense) it is not recommended to change this.</param>
        public Client(string apiKey, bool secureConnection = true)
        {
            BaseUrl = (secureConnection ? secureConnectionUrl : plainConnectionUrl) + urlBase;
            UrlEnd = $"{urlApi}{apiKey}";
        }

        /// <summary>
        /// For addresses -> coordinates.
        /// Given a list of address(es), post them to geocod.io and return
        /// the resulting JArray.
        /// </summary>
        /// <param name="addresses">List of string address(es), for which to obtain latitude/longitude geocoordinates.</param>
        /// <returns>JArray with the result from geocod.io</returns>
        public async Task<JArray> Geocode (List<string> addresses)
        {
            if(addresses == null)
            {
                throw new ArgumentNullException("addresses");
            }

            var jsonAddr = JsonConvert.SerializeObject(addresses);

            var coords = await Post("geocode", jsonAddr);

            var responseObj = JObject.Parse(coords);
            var latlngs = (JArray)responseObj["results"];

            return latlngs;
        }

        /// <summary>
        /// For coordinates -> addresses.
        /// Given a list of coordinates, post them to geocod.io and return the resulting JArray 
        /// of addresses and associated data.
        /// </summary>
        /// <param name="coords">List of tuples that hold the double represented latitude and longitude values.</param>
        /// <returns>JArray with the result from geocod.io</returns>
        public async Task<JArray> ReverseGeocode(List<Tuple<double, double>> coords)
        {
            if(coords == null)
            {
                throw new ArgumentNullException("coords");
            }

            var s = from c in coords
                    select $"{c.Item1},{c.Item2}";

            var jsonCoords = JsonConvert.SerializeObject(s);

            var addr = await Post("reverse", jsonCoords);
            var responseObj = JObject.Parse(addr);
            var addresses = (JArray)responseObj["results"];

            return addresses;
        }

        /// <summary>
        /// Posts the passed in data to geocod.io for either geocoding or reverse geocoding.
        /// </summary>
        /// <param name="action">Geocode or Reverse</param>
        /// <param name="data">JSON of either addresses or geocoordinates for the post body</param>
        /// <returns>the JSON string response</returns>
        public async Task<string> Post (string action, string data)
        {
            //guard clauses
            //TODO turn into argumentNULLexceptions
            if (string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentException("action is a required parameter.");
            }
            if(string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("data is a required parameter.");
            }
            if(!acceptedActions.Contains(action))
            {
                throw new ArgumentException("action is an incorrect value.");
            }

            //setting up the request - headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //setting up the request - body
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            //forming the URI
            var uri = $"{BaseUrl}/{action}{UrlEnd}";

            var result = await client.PostAsync(uri, content);

            if(result.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new GeocodioException("403 Forbidden.  Check your API Key.");
            }

            if(result.StatusCode.ToString() == "422")
            {
                throw new GeocodioException("422 Unprocessable Entity.  Check your data/struture.");
            }

            var resultContent = await result.Content.ReadAsStringAsync();

            return resultContent;
        }
    }
}
