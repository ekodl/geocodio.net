using System;
using System.Collections.Generic;
using System.Text;

namespace geocodio.net
{
    public class GeocodioException : Exception
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
