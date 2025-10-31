using System;
using XFlow.Core;

namespace XFlow.VIP
{
    public class VIPController
    {
        private readonly TimeSpan CHEAT_ADD_VIP_TIME_DURATION = TimeSpan.FromHours(1);
        private static VIPController _instance;
        public static VIPController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VIPController();
                }
                return _instance;
            }
        }

        private VIPController() { }

        public VIPResource GetVIPResource()
        {
            return PlayerData.Instance.Get<VIPResource>();
        }

        public TimeSpan GetVIPDuration()
        {
            return GetVIPResource().VIPDuration;
        }

        public void AddVIPTime(TimeSpan duration)
        {
            var resource = GetVIPResource();
            resource.VIPDuration = resource.VIPDuration.Add(duration);
            if (resource.VIPDuration < TimeSpan.Zero)
            {
                resource.VIPDuration = TimeSpan.Zero;
            }
            PlayerData.Instance.NotifyChange<VIPResource>();
        }

        public void RemoveVIPTime(TimeSpan duration)
        {
            AddVIPTime(-duration);
        }

        public bool HasVIPTime(TimeSpan duration)
        {
            return GetVIPDuration() >= duration;
        }

        public void CheatAddVIPTime()
        {
            AddVIPTime(CHEAT_ADD_VIP_TIME_DURATION);
        }
    }
}
