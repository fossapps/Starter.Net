using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Starter.Net.Startup.Middlewares;
using Starter.Net.Startup.Services;
using Xunit;

namespace Starter.Net.Startup.Test.Middlewares
{
    public class UuIdTest
    {
        [Fact]
        public async void TestAddsUuIdToContextAndCookie()
        {
            const string mockUuid = "2429bfd5-46ea-44dd-8e91-c78965e64b89";
            var middleware = new UuId(async innerHttpContext =>
            {
                innerHttpContext.Items.TryGetValue("uuid", out var token);
                Assert.NotNull(token);
                Assert.Equal(mockUuid, token.ToString());
                await innerHttpContext.Response.WriteAsync(token.ToString());
            }, new MockUuid(mockUuid));

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            await middleware.Invoke(context);
            context.Items.TryGetValue("uuid", out var uuid);
            Assert.NotNull(uuid);
            Assert.Equal(mockUuid, uuid.ToString());
            context.Response.Headers.TryGetValue("Set-Cookie", out var setCookieHeaders);
            var uuidCookie = setCookieHeaders.ToArray().Where(x => x.StartsWith("uuid=")).ToArray()[0];
            Assert.StartsWith($"uuid={uuid}", uuidCookie);
        }
    }

    internal class MockUuid : IUuidService
    {
        private string Token { get; }
        public MockUuid(string tokenToReturn = "46b6c206-e9f1-424b-9f50-bc1e05050eb2")
        {
            Token = tokenToReturn;
        }

        public string GenerateUuId()
        {
            return Token;
        }
    }
}
