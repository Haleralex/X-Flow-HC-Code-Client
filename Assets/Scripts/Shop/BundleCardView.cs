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

        private void Awake()
        {
            if (buyButton != null)
                buyButton.interactable = false;
            if (buyButtonText != null)
                buyButtonText.text = buyText;
        }

        private void Start()
        {
            if (buyButton != null)
                buyButton.onClick.AddListener(OnBuyButtonClick);
            if (infoButton != null)
                infoButton.onClick.AddListener(OnInfoButtonClick);
            
            UpdateButtonState();
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
            PlayerData.Instance.OnAnyChanged += UpdateButtonState;
            BundleDataHolder.Instance.OnPurchaseStateChanged += UpdateButtonState;
        }

        private void OnDisable()
        {
            PlayerData.Instance.OnAnyChanged -= UpdateButtonState;
            BundleDataHolder.Instance.OnPurchaseStateChanged -= UpdateButtonState;
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

            bool isPurchasingThisBundle = BundleDataHolder.Instance.IsPurchasing(_bundle);
            bool canPurchase = !isPurchasingThisBundle && _bundle.CanPurchase();
            
            buyButton.interactable = canPurchase;

            if (buyButtonText != null)
            {
                buyButtonText.text = isPurchasingThisBundle ? processingText : buyText;
            }
        }

        private void OnBuyButtonClick()
        {
            if (BundleDataHolder.Instance.IsPurchasing(_bundle) || _bundle == null) return;

            ShopController.Instance.PurchaseBundle(_bundle, OnPurchaseComplete);
        }

        private void OnPurchaseComplete(bool success)
        {
            Debug.Log($"Purchase complete: {success}");
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