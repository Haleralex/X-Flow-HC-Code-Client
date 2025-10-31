using UnityEngine;
using TMPro;
using XFlow.Core;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace XFlow.Gold
{
    public class GoldView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private LocalizedString goldFormat;

        private void Start()
        {
            PlayerData.Instance.Subscribe<GoldResource>(UpdateView);
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
            UpdateView();
        }

        private void OnDestroy()
        {
            PlayerData.Instance.Unsubscribe<GoldResource>(UpdateView);
            if (LocalizationSettings.Instance != null)
                LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged(Locale _)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var gold = GoldController.Instance.GetCurrentGold();
            goldFormat.GetLocalizedStringAsync(gold).Completed += h =>
            {
                if (goldText != null) goldText.text = h.Result;
            };
        }

        public void OnCheatButtonClick()
        {
            GoldController.Instance.CheatAddGold();
        }
    }
}