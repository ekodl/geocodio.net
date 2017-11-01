using geocodio.net;
using System;
using System.Collections.Generic;
using Xunit;


namespace XUnitTest
{
    public class UnitTest1
    {
        private Client _client;
        private const string _apiKey = "d97f0308d7991f7f7a0acbc9dc3899548153315";

        [Fact]
        public async void Post_InvalidArguments_ThrowArgumentExceptionAsync()
        {
            _client = new Client("api123");

            await Assert.ThrowsAsync<ArgumentException>(() => _client.Post("", "data"));
            await Assert.ThrowsAsync<ArgumentException>(() => _client.Post("reverse", ""));

        }

        [Fact]
        public async void Post_InvalidActionArg_ThrowArgumentExceptionAsync()
        {
            _client = new Client("ap123");

            await Assert.ThrowsAsync<ArgumentException>(() => _client.Post("cows", "an address"));

        }

        [Fact]
        public async void Post_InvalidApiKey_Catches403()
        {
            _client = new Client("alap");
            
            await Assert.ThrowsAsync<GeocodioException>(() => _client.Post("reverse", "5200 W Mercury Blvd, Newport News, VA"));
        }

        [Fact]
        public async void Post_BadAddressData_Catches422()
        {
            _client = new Client(_apiKey);

            await Assert.ThrowsAsync<GeocodioException>(() => _client.Post("reverse", "123 Under Da Sea"));
        }


        [Fact]
        public async void Post_ValidArg_DoesNotThrowExceptionAsync()
        {
            _client = new Client(_apiKey);

            var returnStr = await _client.Post("geocode", @"[""5200 W Mercury Blvd, Newport News, VA""]");

            Assert.NotNull(returnStr);
        }

        [Fact]
        public async void Geocode_InvalidArg_ThrowsArgumentExceptionAsync()
        {
            _client = new Client(_apiKey);
            List<string> list = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _client.Geocode(list));
        }

        //TODO: turn the following into theories
        [Fact]
        public async void Geocode_OneAddress_Success()
        {
            _client = new Client(_apiKey);
            List<string> list = new List<string>() {"5200 W Mercury Blvd, Newport News, VA"};

            var coord = await _client.Geocode(list);

            Assert.NotNull(coord);
            Assert.NotEmpty(coord);
        }

        [Fact]
        public async void Geocode_MultipleAddresses_Success()
        {
            _client = new Client(_apiKey);
            List<string> list = new List<string>() { "5200 W Mercury Blvd, Newport News, VA", "4101 Washington Ave, Newport News, VA 23607" };

            var coord = await _client.Geocode(list);

            Assert.NotNull(coord);
            Assert.NotEmpty(coord);
        }


        [Fact]
        public async void ReverseGeocode_InvalidArg_ThrowsArgumentExceptionAsync()
        {
            _client = new Client(_apiKey);
            List<Tuple<double, double>> list = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _client.ReverseGeocode(list));
        }

        //TODO: turn the following into theories
        [Fact]
        public async void ReverseGeocode_OneAddress_Success()
        {
            _client = new Client(_apiKey);
            List<Tuple<double, double>> list = new List<Tuple<double, double>>() { new Tuple<double, double>(36.9882362,-76.4371783)};

            var addr = await _client.ReverseGeocode(list);

            Assert.NotNull(addr);
            Assert.NotEmpty(addr);
        }

        [Fact]
        public async void ReverseGeocode_MultipleAddresses_Success()
        {
            _client = new Client(_apiKey);
            List<Tuple<double, double>> list = new List<Tuple<double, double>>() { new Tuple<double, double>(36.9882362, -76.4371783), new Tuple<double, double>(37.0236644, -76.4403612) };

            var addr = await _client.ReverseGeocode(list);

            Assert.NotNull(addr);
            Assert.NotEmpty(addr);
        }
    }
}
