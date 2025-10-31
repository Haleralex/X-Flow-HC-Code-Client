using UnityEngine;
using XFlow.Core;

namespace XFlow.Gold
{
    [CreateAssetMenu(fileName = "RemoveFixedGold", menuName = "XFlow/Gold/Remove Fixed Gold")]
    public class RemoveFixedGoldAction : GameActionScriptableObject
    {
        [SerializeField] private int amount;

        public override bool CanApply()
        {
            return GoldController.Instance.HasGold(amount);
        }

        public override void Apply()
        {
            GoldController.Instance.RemoveGold(amount);
        }
    }
}
