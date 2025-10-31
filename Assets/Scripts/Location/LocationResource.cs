namespace XFlow.Location
{
    public class LocationResource
    {
        private const string START_LOCATION = "StartZone";
        public string CurrentLocation { get; set; }

        public LocationResource()
        {
            CurrentLocation = START_LOCATION;
        }
    }
}
