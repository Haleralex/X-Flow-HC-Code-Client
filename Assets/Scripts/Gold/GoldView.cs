using UnityEngine;
using TMPro;
using XFlow.Core;

namespace XFlow.Gold
{
    public class GoldView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private string goldFormat = "Gold: {0}";

        private void Start()
        {
            PlayerData.Instance.Subscribe<GoldResource>(UpdateView);
            UpdateView();
        }

        private void OnDestroy()
        {
            PlayerData.Instance.Unsubscribe<GoldResource>(UpdateView);
        }

        private void UpdateView()
        {
            var gold = GoldController.Instance.GetCurrentGold();
            if (goldText != null) goldText.text = string.Format(goldFormat, gold);
        }

        public void OnCheatButtonClick()
        {
            GoldController.Instance.CheatAddGold();
        }
    }
}