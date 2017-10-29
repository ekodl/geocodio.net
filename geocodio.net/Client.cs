using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace geocodio.net
{
    public class Client
    {
        private const string secureConnectionUrl = "https://";
        private const string plainConnectionUrl = "http://";
        private const string urlBase = "api.geocod.io/v1/";

        public string APIKey { get; set; }
        
        public string BaseUrl { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="apiKey">Personal API Key from geocod.io</param>
        /// <param name="secureConnection">Determines whether to use http or https.  Default is https, 
        /// per the geocod.io spec (and common sense) it is not recommended to change this.</param>
        public Client(string apiKey, bool secureConnection = true)
        {
            BaseUrl = (secureConnection ? secureConnectionUrl : plainConnectionUrl) + urlBase;


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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Posts the passed in data to geocod.io for either geocoding or reverse geocoding.
        /// </summary>
        /// <param name="action">Geocode or Reverse</param>
        /// <param name="data">JSON of either addresses or geocoordinates for the post body</param>
        /// <returns>the JSON string response</returns>
        public async Task<string> Post (string action, string data)
        {
            throw new NotImplementedException();
        }
    }
}
