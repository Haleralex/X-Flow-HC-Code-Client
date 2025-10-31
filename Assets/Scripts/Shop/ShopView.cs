using System.Collections.Generic;
using UnityEngine;
using XFlow.Core;

namespace XFlow.Shop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Bundle[] availableBundles;
        [SerializeField] private BundleCardView bundleCardPrefab;
        [SerializeField] private Transform bundleContainer;
        private readonly List<BundleCardView> _bundles = new();
        private void Start()
        {
            InitializeBundles();
        }
        void OnEnable()
        {
            PlayerData.Instance.OnAnyChanged += UpdateShopUI;
        }
        void OnDestroy()
        {
            PlayerData.Instance.OnAnyChanged -= UpdateShopUI;
        }
        private void UpdateShopUI()
        {
            foreach (var bundle in _bundles)
            {
                bundle.UpdateButtonState();
            }
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
                _bundles.Add(card);
            }
        }
    }
}
