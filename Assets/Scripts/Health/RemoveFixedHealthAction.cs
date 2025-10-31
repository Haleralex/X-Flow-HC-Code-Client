using UnityEngine;
using XFlow.Core;

namespace XFlow.Health
{
    [CreateAssetMenu(fileName = "RemoveFixedHealth", menuName = "XFlow/Health/Remove Fixed Health")]
    public class RemoveFixedHealthAction : GameActionScriptableObject
    {
        [SerializeField] private int amount;

        public override bool CanApply()
        {
            return HealthController.Instance.HasHealth(amount);
        }

        public override void Apply()
        {
            HealthController.Instance.RemoveHealth(amount);
        }
    }
}
