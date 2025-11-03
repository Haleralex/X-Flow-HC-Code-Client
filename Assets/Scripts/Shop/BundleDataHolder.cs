using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private HashSet<Bundle> _purchasingBundles = new HashSet<Bundle>();
        
        public event Action OnPurchaseStateChanged;

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

        public bool IsPurchasing(Bundle bundle)
        {
            return _purchasingBundles.Contains(bundle);
        }

        public void StartPurchaseCoroutine(Bundle bundle, float delay, Action<bool> onComplete)
        {
            if (_purchasingBundles.Contains(bundle))
            {
                onComplete?.Invoke(false);
                Debug.Log("This bundle is already being purchased.");
                return;
            }

            _purchasingBundles.Add(bundle);
            OnPurchaseStateChanged?.Invoke();
            
            StartCoroutine(PurchaseCoroutine(bundle, delay, onComplete));
        }

        private IEnumerator PurchaseCoroutine(Bundle bundle, float delay, Action<bool> onComplete)
        {
            yield return new WaitForSecondsRealtime(delay);

            if (!bundle.CanPurchase())
            {
                _purchasingBundles.Remove(bundle);
                OnPurchaseStateChanged?.Invoke();
                
                onComplete?.Invoke(false);
                Debug.Log("Bundle purchase failed: insufficient resources after delay.");
                yield break;
            }

            bundle.ApplyCosts();
            bundle.ApplyRewards();

            _purchasingBundles.Remove(bundle);
            OnPurchaseStateChanged?.Invoke();
            
            onComplete?.Invoke(true);
            Debug.Log("Bundle purchased successfully.");
        }
    }
}