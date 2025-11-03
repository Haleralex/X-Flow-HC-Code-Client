using System;
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

        public void PurchaseBundle(Bundle bundle, Action<bool> onComplete)
        {
            if (bundle == null || !bundle.CanPurchase())
            {
                onComplete?.Invoke(false);
                Debug.Log("Bundle purchase failed: insufficient resources.");
                return;
            }

            BundleDataHolder.Instance.StartPurchaseCoroutine(
                bundle,
                NETWORK_DELAY_SECONDS,
                onComplete
            );
        }
    }
}