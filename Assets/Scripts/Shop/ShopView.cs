using UnityEngine;

namespace XFlow.Shop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Bundle[] availableBundles;
        [SerializeField] private BundleCardView bundleCardPrefab;
        [SerializeField] private Transform bundleContainer;

        private void Start()
        {
            InitializeBundles();
        }

        private void InitializeBundles()
        {
            if (availableBundles == null || bundleCardPrefab == null || bundleContainer == null)
                return;

            foreach (var bundle in availableBundles)
            {
                if (bundle == null) continue;

                BundleCardView card = Instantiate(bundleCardPrefab, bundleContainer);
                card.Initialize(bundle, true);
            }
        }
    }
}
