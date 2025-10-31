using UnityEngine;
using TMPro;
using XFlow.Core;
using System;

namespace XFlow.Health
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private string healthFormat = "Health: {0}/{1}";

        private void OnEnable()
        {
            PlayerData.Instance.Subscribe<HealthResource>(UpdateView);
            UpdateView();
        }

        private void OnDisable()
        {
            PlayerData.Instance.Unsubscribe<HealthResource>(UpdateView);
        }

        private void UpdateView()
        {
            var health = HealthController.Instance.GetCurrentHealth();
            var maxHealth = HealthController.Instance.GetMaxHealth();
            if (healthText != null) healthText.text = string.Format(healthFormat, health, maxHealth);
        }

        public void OnCheatButtonClick()
        {
            HealthController.Instance.CheatAddHealth();
        }
    }
}