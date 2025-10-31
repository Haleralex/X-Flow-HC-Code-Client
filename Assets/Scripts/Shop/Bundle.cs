using UnityEngine;
using XFlow.Core;

namespace XFlow.Shop
{
    [CreateAssetMenu(fileName = "Bundle", menuName = "XFlow/Shop/Bundle")]
    public class Bundle : ScriptableObject
    {
        [SerializeField] private string bundleName;
        [SerializeField] private GameActionScriptableObject[] costs;
        [SerializeField] private GameActionScriptableObject[] rewards;

        public string BundleName => bundleName;
        public GameActionScriptableObject[] Costs => costs;
        public GameActionScriptableObject[] Rewards => rewards;

        public bool CanPurchase()
        {
            if (costs == null || costs.Length == 0)
                return true;

            foreach (var cost in costs)
            {
                if (cost != null && !cost.CanApply())
                    return false;
            }
            return true;
        }

        public void ApplyCosts()
        {
            if (costs == null) return;

            foreach (var cost in costs)
            {
                if (cost != null)
                    cost.Apply();
            }
        }

        public void ApplyRewards()
        {
            if (rewards == null) return;

            foreach (var reward in rewards)
            {
                if (reward != null)
                    reward.Apply();
            }
        }
    }
}
