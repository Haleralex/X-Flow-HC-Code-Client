using UnityEngine;
using TMPro;
using UnityEngine.UI;
using XFlow.Core;

namespace XFlow.Shop
{
    public class BundleCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        [SerializeField] private string buyText = "Buy";
        [SerializeField] private string processingText = "Processing...";

        private Bundle _bundle;
        private bool _isPurchasing;

        private void Start()
        {
            if (buyButton != null)
                buyButton.onClick.AddListener(OnBuyButtonClick);
            if (infoButton != null)
                infoButton.onClick.AddListener(OnInfoButtonClick);
        }

        private void OnDestroy()
        {
            if (buyButton != null)
                buyButton.onClick.RemoveListener(OnBuyButtonClick);
            if (infoButton != null)
                infoButton.onClick.RemoveListener(OnInfoButtonClick);
        }

        private void OnEnable()
        {
            ShopController.Instance.OnBundleStateChanged += UpdateButtonState;
            UpdateButtonState();
        }

        private void OnDisable()
        {
            ShopController.Instance.OnBundleStateChanged -= UpdateButtonState;
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

        public void UpdateButtonState()
        {
            if (buyButton == null || _bundle == null) return;

            bool canPurchase = ShopController.Instance.CanPurchaseBundle(_bundle);
            buyButton.interactable = canPurchase && !_isPurchasing;

            if (buyButtonText != null)
            {
                buyButtonText.text = _isPurchasing ? processingText : buyText;
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