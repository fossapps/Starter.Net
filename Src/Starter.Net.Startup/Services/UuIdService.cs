namespace Starter.Net.Startup.Services
{
    public class UuIdService : IUuidService
    {
        public string GenerateUuId()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
