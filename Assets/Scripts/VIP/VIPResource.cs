using System;

namespace XFlow.VIP
{
    public class VIPResource
    {
        public TimeSpan VIPDuration { get; set; }

        public VIPResource()
        {
            VIPDuration = TimeSpan.Zero;
        }
    }
}
