using UnityEngine;
using XFlow.Core;

namespace XFlow.Health
{
    public class HealthController
    {
        private const int CHEAT_ADD_HEALTH_AMOUNT = 50;
        private static HealthController _instance;
        public static HealthController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HealthController();
                }
                return _instance;
            }
        }

        private HealthController() { }

        public HealthResource GetHealthResource()
        {
            return PlayerData.Instance.Get<HealthResource>();
        }

        public int GetCurrentHealth()
        {
            return GetHealthResource().CurrentHealth;
        }

        public int GetMaxHealth()
        {
            return GetHealthResource().MaxHealth;
        }

        public void AddHealth(int amount)
        {
            var data = PlayerData.Instance.Get<HealthResource>();
            data.CurrentHealth = Mathf.Min(data.MaxHealth, data.CurrentHealth + amount);
            PlayerData.Instance.Set(data);
        }

        public void RemoveHealth(int amount)
        {
            var resource = GetHealthResource();
            resource.CurrentHealth = Mathf.Max(resource.CurrentHealth - amount, 0);
            PlayerData.Instance.Set(resource);
        }

        public bool HasHealth(int amount)
        {
            return GetCurrentHealth() >= amount;
        }

        public void AddHealthPercentage(float percentage)
        {
            var resource = GetHealthResource();
            int amount = Mathf.RoundToInt(resource.CurrentHealth * percentage / 100f);
            AddHealth(amount);
        }

        public void RemoveHealthPercentage(float percentage)
        {
            var resource = GetHealthResource();
            int amount = Mathf.RoundToInt(resource.CurrentHealth * percentage / 100f);
            RemoveHealth(amount);
        }

        public bool HasHealthPercentage(float percentage)
        {
            var resource = GetHealthResource();
            int required = Mathf.RoundToInt(resource.CurrentHealth * percentage / 100f);
            return HasHealth(required);
        }

        public void CheatAddHealth()
        {
            AddHealth(CHEAT_ADD_HEALTH_AMOUNT);
        }
    }
}
