using UnityEngine;
using TMPro;
using XFlow.Core;

namespace XFlow.VIP
{
    public class VIPView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI vipText;
        [SerializeField] private string vipFormat = "VIP: {0}s";

        private void OnEnable()
        {
            PlayerData.Instance.Subscribe<VIPResource>(UpdateView);
            UpdateView();
        }

        private void OnDisable()
        {
            PlayerData.Instance.Unsubscribe<VIPResource>(UpdateView);
        }

        private void UpdateView()
        {
            var seconds = (int)VIPController.Instance.GetVIPDuration().TotalSeconds;
            if (vipText != null) vipText.text = string.Format(vipFormat, seconds);
        }

        public void OnCheatButtonClick()
        {
            VIPController.Instance.CheatAddVIPTime();
        }
    }
}