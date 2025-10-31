using UnityEngine;
using XFlow.Core;

namespace XFlow.Gold
{
    public class GoldController
    {
        private const int CHEAT_ADD_GOLD_AMOUNT = 100;
        private const int MIN_GOLD = 0;
        private static GoldController _instance;
        public static GoldController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GoldController();
                }
                return _instance;
            }
        }

        private GoldController() { }

        public GoldResource GetGoldResource()
        {
            return PlayerData.Instance.Get<GoldResource>();
        }

        public int GetCurrentGold()
        {
            return GetGoldResource().CurrentGold;
        }

        public void AddGold(int amount)
        {
            var resource = GetGoldResource();
            resource.CurrentGold = Mathf.Max(MIN_GOLD, resource.CurrentGold + amount);
            PlayerData.Instance.NotifyChange<GoldResource>();
        }

        public void RemoveGold(int amount)
        {
            var resource = GetGoldResource();
            resource.CurrentGold = Mathf.Max(MIN_GOLD, resource.CurrentGold - amount);
            PlayerData.Instance.NotifyChange<GoldResource>();
        }

        public bool HasGold(int amount)
        {
            return GetCurrentGold() >= amount;
        }

        public void CheatAddGold()
        {
            AddGold(CHEAT_ADD_GOLD_AMOUNT);
        }
    }
}
