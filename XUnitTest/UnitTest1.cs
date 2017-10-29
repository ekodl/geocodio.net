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
        public async void Post_InvalidAction_ThrowArgumentExceptionAsync()
        {
            _client = new Client("ap123");

            await Assert.ThrowsAsync<ArgumentException>(() => _client.Post("cows", "an address"));

        }
    }
}
