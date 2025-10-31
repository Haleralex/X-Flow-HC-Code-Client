using UnityEngine;
using XFlow.Core;

namespace XFlow.Health
{
    [CreateAssetMenu(fileName = "RemovePercentageHealth", menuName = "XFlow/Health/Remove Percentage Health")]
    public class RemovePercentageHealthAction : GameActionScriptableObject
    {
        [SerializeField] private float percentage;

        public override bool CanApply()
        {
            return HealthController.Instance.HasHealthPercentage(percentage);
        }

        public override void Apply()
        {
            HealthController.Instance.RemoveHealthPercentage(percentage);
        }
    }
}
