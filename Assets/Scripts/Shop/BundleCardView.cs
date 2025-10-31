using UnityEngine;
using TMPro;
using UnityEngine.UI;
using XFlow.Core;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace XFlow.Shop
{
    public class BundleCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        [SerializeField] private LocalizedString buyText;
        [SerializeField] private LocalizedString processingText;

        private Bundle _bundle;
        private bool _isPurchasing;

        private void Start()
        {
            if (buyButton != null)
                buyButton.onClick.AddListener(OnBuyButtonClick);
            if (infoButton != null)
                infoButton.onClick.AddListener(OnInfoButtonClick);

            ShopController.Instance.OnBundleStateChanged += UpdateButtonState;
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

            UpdateButtonState();
        }

        private void OnDestroy()
        {
            if (buyButton != null)
                buyButton.onClick.RemoveListener(OnBuyButtonClick);
            if (infoButton != null)
                infoButton.onClick.RemoveListener(OnInfoButtonClick);

            ShopController.Instance.OnBundleStateChanged -= UpdateButtonState;
            if (LocalizationSettings.Instance != null)
                LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged(Locale _)
        {
            UpdateButtonState();
        }

        public void Initialize(Bundle bundle, bool showInfoButton = true)
        {
            _bundle = bundle;
            if (nameText != null)
                nameText.text = bundle.BundleName;
            if (infoButton != null)
                infoButton.gameObject.SetActive(showInfoButton);

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            if (buyButton == null || _bundle == null) return;

            bool canPurchase = ShopController.Instance.CanPurchaseBundle(_bundle);
            buyButton.interactable = canPurchase && !_isPurchasing;

            if (buyButtonText != null)
            {
                var ls = _isPurchasing ? processingText : buyText;
                ls.GetLocalizedStringAsync().Completed += handle =>
                {
                    if (buyButtonText != null)
                        buyButtonText.text = handle.Result;
                };
            }
        }

        private void OnBuyButtonClick()
        {
            if (_isPurchasing || _bundle == null) return;

            _isPurchasing = true;
            UpdateButtonState();
            StartCoroutine(ShopController.Instance.PurchaseBundle(_bundle, OnPurchaseComplete));
        }

        private void OnPurchaseComplete(bool success)
        {
            _isPurchasing = false;
            UpdateButtonState();
        }

        private void OnInfoButtonClick()
        {
            if (_bundle != null)
            {
                BundleDataHolder.Instance.OpenBundleDetailScene(_bundle);
            }
        }
    }
}