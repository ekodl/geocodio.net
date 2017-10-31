using geocodio.net;
using System;
using Xunit;


namespace XUnitTest
{
    public class UnitTest1
    {
        private Client _client;

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
            _client = new Client("YOUR API KEY HERE");

            await Assert.ThrowsAsync<GeocodioException>(() => _client.Post("reverse", "123 Under Da Sea"));
        }


        [Fact]
        public async void Post_ValidArg_DoesNotThrowExceptionAsync()
        {
            _client = new Client("YOUR API KEY HERE", false);

            var returnStr = await _client.Post("geocode", @"[""5200 W Mercury Blvd, Newport News, VA""]");

            Assert.NotNull(returnStr);
        }
    }
}
