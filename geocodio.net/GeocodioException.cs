using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace geocodio.net
{
    public class GeocodioException : HttpRequestException
    {
        public GeocodioException() { }

        public GeocodioException(string message) 
            :base(message)
        {

        }

        public GeocodioException(string message, Exception inner)
            :base(message, inner)
        {

        }
        
    }
}
