using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFlow.Shop
{

    public class BundleDataHolder : MonoBehaviour
    {
        private const string BUNDLE_DETAIL_SCENE_NAME = "BundleDetail";
        private const string SHOP_SCENE_NAME = "ShopScene";
        private static BundleDataHolder _instance;
        public static BundleDataHolder Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject holder = new GameObject("BundleDataHolder");
                    _instance = holder.AddComponent<BundleDataHolder>();
                    DontDestroyOnLoad(holder);
                }
                return _instance;
            }
        }

        public Bundle SelectedBundle { get; private set; }

        public void SetSelectedBundle(Bundle bundle)
        {
            SelectedBundle = bundle;
        }

        public void OpenBundleDetailScene(Bundle bundle)
        {
            SetSelectedBundle(bundle);
            SceneManager.LoadScene(BUNDLE_DETAIL_SCENE_NAME);
        }

        public void ReturnToShopScene()
        {
             SceneManager.LoadScene(SHOP_SCENE_NAME);
        }
    }
}
