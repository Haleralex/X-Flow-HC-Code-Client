using UnityEngine;
using TMPro;
using XFlow.Core;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System;

namespace XFlow.Health
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private LocalizedString healthFormat;

        private void Start()
        {
            PlayerData.Instance.Subscribe<HealthResource>(UpdateView);
            UpdateView();

            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        private void OnDestroy()
        {
            PlayerData.Instance.Unsubscribe<HealthResource>(UpdateView);
            if (LocalizationSettings.Instance != null)
                LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private void UpdateView()
        {
            var health = HealthController.Instance.GetCurrentHealth();
            var maxHealth = HealthController.Instance.GetMaxHealth();
            healthFormat.GetLocalizedStringAsync(health, maxHealth).Completed += h =>
            {
                if (healthText != null) healthText.text = h.Result;
            };
        }
        private void OnLocaleChanged(Locale _)
        {
            UpdateView();
        }

        public void OnCheatButtonClick()
        {
            HealthController.Instance.CheatAddHealth();
        }
    }
}