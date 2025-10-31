using UnityEngine;
using XFlow.Core;

namespace XFlow.Location
{
    [CreateAssetMenu(fileName = "ChangeLocation", menuName = "XFlow/Location/Change Location")]
    public class ChangeLocationAction : GameActionScriptableObject
    {
        [SerializeField] private string newLocation;

        public override bool CanApply()
        {
            return !string.IsNullOrEmpty(newLocation);
        }

        public override void Apply()
        {
            LocationController.Instance.SetLocation(newLocation);
        }
    }
}
