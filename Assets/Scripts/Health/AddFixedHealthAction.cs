using UnityEngine;
using XFlow.Core;

namespace XFlow.Health
{
    [CreateAssetMenu(fileName = "AddFixedHealth", menuName = "XFlow/Health/Add Fixed Health")]
    public class AddFixedHealthAction : GameActionScriptableObject
    {
        [SerializeField] private int amount;

        public override bool CanApply()
        {
            return true; // Can always add health
        }

        public override void Apply()
        {
            HealthController.Instance.AddHealth(amount);
        }
    }
}
