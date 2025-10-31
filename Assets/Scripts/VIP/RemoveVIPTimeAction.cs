using System;
using UnityEngine;
using XFlow.Core;

namespace XFlow.VIP
{
    [CreateAssetMenu(fileName = "RemoveVIPTime", menuName = "XFlow/VIP/Remove VIP Time")]
    public class RemoveVIPTimeAction : GameActionScriptableObject
    {
        [SerializeField] private int seconds;

        public override bool CanApply()
        {
            return VIPController.Instance.HasVIPTime(TimeSpan.FromSeconds(seconds));
        }

        public override void Apply()
        {
            VIPController.Instance.RemoveVIPTime(TimeSpan.FromSeconds(seconds));
        }
    }
}
