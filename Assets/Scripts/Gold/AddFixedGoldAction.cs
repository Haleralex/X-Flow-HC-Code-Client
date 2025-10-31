using UnityEngine;
using XFlow.Core;

namespace XFlow.Gold
{
    [CreateAssetMenu(fileName = "AddFixedGold", menuName = "XFlow/Gold/Add Fixed Gold")]
    public class AddFixedGoldAction : GameActionScriptableObject
    {
        [SerializeField] private int amount;

        public override bool CanApply()
        {
            return true;
        }

        public override void Apply()
        {
            GoldController.Instance.AddGold(amount);
        }
    }
}
