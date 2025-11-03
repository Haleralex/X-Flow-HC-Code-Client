using XFlow.Core;

namespace XFlow.Location
{
    public class LocationController
    {
        private const string DEFAULT_ZONE = "StartZone";
        private static LocationController _instance;
        public static LocationController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LocationController();
                }
                return _instance;
            }
        }

        private LocationController() { }

        public LocationResource GetLocationResource()
        {
            return PlayerData.Instance.Get<LocationResource>();
        }

        public string GetCurrentLocation()
        {
            return GetLocationResource().CurrentLocation;
        }

        public void SetLocation(string newLocation)
        {
            var resource = GetLocationResource();
            resource.CurrentLocation = newLocation;
            PlayerData.Instance.Set(resource);
        }

        public void CheatResetLocation()
        {
            SetLocation(DEFAULT_ZONE);
        }
    }
}
