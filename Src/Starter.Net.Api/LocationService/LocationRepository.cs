using System.Linq;
using Starter.Net.Api.Models;
using Starter.Net.Startup.Services;

namespace Starter.Net.Api.LocationService
{
    public class LocationRepository<T> : ILocationRepository
    {
        private readonly IUuidService _uuidService;
        private readonly ApplicationContext _db;

        public LocationRepository(IUuidService uuidService, ApplicationContext db)
        {
            _uuidService = uuidService;
            _db = db;
        }

        public Location Create(Location item)
        {
            item.Id = _uuidService.GenerateUuId();
            _db.Locations.Add(item);
            _db.SaveChangesAsync();
            return item;
        }

        public void Update(string id, Location item)
        {
            var location = new Location
            {
                Id = id
            };
            _db.Locations.Attach(location);
            location.Accuracy = item.Accuracy;
            location.Latitude = item.Latitude;
            location.Longitude = item.Longitude;
            _db.SaveChangesAsync();
        }

        public void Remove(string id)
        {
            var location = new Location
            {
                Id = id
            };
            _db.Locations.Attach(location);
            _db.Locations.Remove(location);
            _db.SaveChangesAsync();
        }

        public Location Find(string id)
        {
            return _db.Locations.FirstOrDefault(x => x.Id == id);
        }
    }
}
