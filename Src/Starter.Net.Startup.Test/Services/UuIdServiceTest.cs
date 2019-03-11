using System.Collections.Generic;
using Starter.Net.Startup.Services;
using Xunit;

namespace Starter.Net.Startup.Test.Services
{
    public class UuIdServiceTest
    {
        [Fact]
        public void TestGeneratesUuid()
        {
            var uuid = new UuIdService();
            var token = uuid.GenerateUuId();
            Assert.NotNull(token);
            Assert.True(token.Length > 0);
        }

        [Fact]
        public void TestUuIdsAreUnique()
        {
            var tokens = new HashSet<string>();
            var uuid = new UuIdService();
            for (var i = 0; i < 100; i++)
            {
                tokens.Add(uuid.GenerateUuId());
            }

            Assert.Equal(100, tokens.Count);
        }
    }
}
