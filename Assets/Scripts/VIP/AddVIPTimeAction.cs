using System;
using UnityEngine;
using XFlow.Core;

namespace XFlow.VIP
{
    [CreateAssetMenu(fileName = "AddVIPTime", menuName = "XFlow/VIP/Add VIP Time")]
    public class AddVIPTimeAction : GameActionScriptableObject
    {
        [SerializeField] private int seconds;

        public override bool CanApply()
        {
            return true;
        }

        public override void Apply()
        {
            VIPController.Instance.AddVIPTime(TimeSpan.FromSeconds(seconds));
        }
    }
}
