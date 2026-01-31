using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MercyAICourt.UI
{
    /// <summary>
    /// Testing panel for debugging. Toggle with 'T' key.
    /// Allows manual adjustment of guilt, timer, and game state.
    /// </summary>
    public class TestingPanel : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject panelObject;

        [Header("Display")]
        [SerializeField] private TextMeshProUGUI statusText;

        private bool isPanelVisible = false;

        private void Start()
        {
            if (panelObject != null)
            {
                panelObject.SetActive(false);
            }
        }

        private void Update()
        {
            // Toggle panel with 'T' key
            if (Input.GetKeyDown(KeyCode.T))
            {
                TogglePanel();
            }

            UpdateStatusText();
        }

        private void TogglePanel()
        {
            isPanelVisible = !isPanelVisible;
            if (panelObject != null)
            {
                panelObject.SetActive(isPanelVisible);
            }
        }

        private void UpdateStatusText()
        {
            if (statusText != null && Managers.GameManager.Instance != null)
            {
                statusText.text = string.Format(
                    "State: {0}\nGuilt: {1:0}%\nTime: {2}\nPaused: {3}",
                    Managers.GameManager.Instance.StateManager.CurrentState,
                    Managers.GameManager.Instance.GuiltSystem.GuiltPercentage,
                    Managers.GameManager.Instance.TimerSystem.GetFormattedTime(),
                    Managers.GameManager.Instance.TimerSystem.IsPaused
                );
            }
        }

        // Button Callbacks
        public void OnIncreaseGuilt()
        {
            Managers.GameManager.Instance.GuiltSystem.IncreaseGuilt(5f);
        }

        public void OnDecreaseGuilt()
        {
            Managers.GameManager.Instance.GuiltSystem.DecreaseGuilt(5f);
        }

        public void OnAddTime()
        {
            // Add 60 game seconds (1 game minute)
            Managers.GameManager.Instance.TimerSystem.AddTime(60f);
        }

        public void OnSubtractTime()
        {
            // Subtract 60 game seconds (1 game minute)
            Managers.GameManager.Instance.TimerSystem.SubtractTime(60f);
        }

        public void OnSkipToWin()
        {
            // Set guilt below 92% to trigger victory
            Managers.GameManager.Instance.GuiltSystem.SetGuilt(90f);
        }

        public void OnSkipToLose()
        {
            // Set timer to 0 with guilt >= 92%
            Managers.GameManager.Instance.GuiltSystem.SetGuilt(95f);
            Managers.GameManager.Instance.TimerSystem.SubtractTime(Managers.GameManager.Instance.TimerSystem.GameSecondsRemaining);
        }

        public void OnResetGame()
        {
            Managers.GameManager.Instance.RestartGame();
        }

        public void OnTogglePause()
        {
            Managers.GameManager.Instance.TogglePause();
        }
    }
}
