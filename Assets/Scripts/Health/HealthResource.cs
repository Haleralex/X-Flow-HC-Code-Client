using UnityEngine;

namespace XFlow.Health
{
    public class HealthResource
    {
        private const int MIN_HEALTH = 0;
        private const int MIN_MAX_HEALTH = 1;
        private const int DEFAULT_MAX_HEALTH = 100;
        private const int DEFAULT_CURRENT_HEALTH = 100;
        private int _currentHealth;
        private int _maxHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, MIN_HEALTH, MaxHealth);
        }

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = Mathf.Max(MIN_MAX_HEALTH, value);
                if (_currentHealth > _maxHealth)
                    _currentHealth = _maxHealth;
            }
        }

        public HealthResource()
        {
            _maxHealth = DEFAULT_MAX_HEALTH;
            _currentHealth = DEFAULT_CURRENT_HEALTH;
        }
    }
}
