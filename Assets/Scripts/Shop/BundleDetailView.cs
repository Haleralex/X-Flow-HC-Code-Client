using UnityEngine;
using UnityEngine.UI;

namespace XFlow.Shop
{
    public class BundleDetailView : MonoBehaviour
    {
        [SerializeField] private BundleCardView bundleCard;
        [SerializeField] private Button backButton;

        private void Start()
        {
            if (backButton != null)
                backButton.onClick.AddListener(OnBackButtonClick);

            Bundle selectedBundle = BundleDataHolder.Instance.SelectedBundle;
            if (selectedBundle != null && bundleCard != null)
            {
                bundleCard.Initialize(selectedBundle, false);
            }
        }

        private void OnDestroy()
        {
            if (backButton != null)
                backButton.onClick.RemoveListener(OnBackButtonClick);
        }

        private void OnBackButtonClick()
        {
            BundleDataHolder.Instance.ReturnToShopScene();
        }
    }
}
