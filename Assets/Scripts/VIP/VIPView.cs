using UnityEngine;
using TMPro;
using XFlow.Core;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace XFlow.VIP
{
    public class VIPView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI vipText;
        [SerializeField] private LocalizedString vipFormat;

        private void Start()
        {
            PlayerData.Instance.Subscribe<VIPResource>(UpdateView);
            UpdateView();
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        private void OnDestroy()
        {
            PlayerData.Instance.Unsubscribe<VIPResource>(UpdateView);
            if (LocalizationSettings.Instance != null)
                LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }
        private void OnLocaleChanged(Locale _)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var seconds = (int)VIPController.Instance.GetVIPDuration().TotalSeconds;
            vipFormat.GetLocalizedStringAsync(seconds).Completed += h =>
            {
                if (vipText != null) vipText.text = h.Result;
            };
        }

        public void OnCheatButtonClick()
        {
            VIPController.Instance.CheatAddVIPTime();
        }
    }
}