using UnityEngine;
using XFlow.Core;

namespace XFlow.Health
{
    [CreateAssetMenu(fileName = "AddPercentageHealth", menuName = "XFlow/Health/Add Percentage Health")]
    public class AddPercentageHealthAction : GameActionScriptableObject
    {
        [SerializeField] private float percentage;

        public override bool CanApply()
        {
            return true;
        }

        public override void Apply()
        {
            HealthController.Instance.AddHealthPercentage(percentage);
        }
    }
}
