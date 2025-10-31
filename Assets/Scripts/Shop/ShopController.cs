using System;
using System.Collections;
using UnityEngine;

namespace XFlow.Shop
{
    public class ShopController
    {
        private const float NETWORK_DELAY_SECONDS = 3f;
        private static ShopController _instance;
        public static ShopController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ShopController();
                }
                return _instance;
            }
        }

        private ShopController() { }

        public event Action OnBundleStateChanged;

        public IEnumerator PurchaseBundle(Bundle bundle, Action<bool> onComplete)
        {
            if (bundle == null || !bundle.CanPurchase())
            {
                onComplete?.Invoke(false);
                yield break;
            }

            yield return new WaitForSeconds(NETWORK_DELAY_SECONDS);

            bundle.ApplyCosts();
            bundle.ApplyRewards();

            OnBundleStateChanged?.Invoke();

            onComplete?.Invoke(true);
        }

        public bool CanPurchaseBundle(Bundle bundle)
        {
            return bundle != null && bundle.CanPurchase();
        }
    }
}
