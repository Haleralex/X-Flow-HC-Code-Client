using UnityEngine;
using TMPro;
using XFlow.Core;

namespace XFlow.Location
{
    public class LocationView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI locationText;
        [SerializeField] private string locationFormat = "Location: {0}"; 

        private void OnEnable()
        {
            PlayerData.Instance.Subscribe<LocationResource>(UpdateView);
            UpdateView();
        }

        private void OnDisable()
        {
            PlayerData.Instance.Unsubscribe<LocationResource>(UpdateView);
        }

        private void UpdateView()
        {
            var location = LocationController.Instance.GetCurrentLocation();
            if (locationText != null) locationText.text = string.Format(locationFormat, location);
        }

        public void OnCheatButtonClick()
        {
            LocationController.Instance.CheatResetLocation();
        }
    }
}