using UnityEngine;
using TMPro;
using XFlow.Core;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace XFlow.Location
{
    public class LocationView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI locationText;
        [SerializeField] private LocalizedString locationFormat; 

        private void Start()
        {
            PlayerData.Instance.Subscribe<LocationResource>(UpdateView);
            UpdateView();
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        private void OnDestroy()
        {
            PlayerData.Instance.Unsubscribe<LocationResource>(UpdateView);
            if (LocalizationSettings.Instance != null)
                LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged(Locale _)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var location = LocationController.Instance.GetCurrentLocation();
            locationFormat.GetLocalizedStringAsync(location).Completed += h =>
            {
                if (locationText != null) locationText.text = h.Result;
            };
        }

        public void OnCheatButtonClick()
        {
            LocationController.Instance.CheatResetLocation();
        }
    }
}