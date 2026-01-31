using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MercyAICourt.Systems;

namespace MercyAICourt.UI
{
    /// <summary>
    /// Manages all UI elements for the Mercy AI Court game.
    /// Handles display updates for timer, guilt, and game state screens.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Main UI Elements")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI guiltPercentageText;
        [SerializeField] private Image guiltMeterFill;

        [Header("Screen Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject interrogationPanel;
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Victory/Game Over")]
        [SerializeField] private TextMeshProUGUI victoryText;
        [SerializeField] private TextMeshProUGUI gameOverText;

        private void Start()
        {
            // Subscribe to events
            if (Managers.GameManager.Instance != null)
            {
                Managers.GameManager.Instance.TimerSystem.OnTimerUpdated += UpdateTimer;
                Managers.GameManager.Instance.GuiltSystem.OnGuiltChanged += UpdateGuilt;
                Managers.GameManager.Instance.StateManager.OnStateChanged += OnStateChanged;
            }

            // Initialize UI
            UpdateTimer(Managers.GameManager.Instance.TimerSystem.GameSecondsRemaining);
            UpdateGuilt(Managers.GameManager.Instance.GuiltSystem.GuiltPercentage);
            OnStateChanged(Managers.GameManager.Instance.StateManager.CurrentState);
        }

        private void UpdateTimer(float gameSeconds)
        {
            if (timerText != null)
            {
                timerText.text = Managers.GameManager.Instance.TimerSystem.GetFormattedTime();
            }
        }

        private void UpdateGuilt(float guiltPercentage)
        {
            if (guiltPercentageText != null)
            {
                guiltPercentageText.text = string.Format("{0:0}%", guiltPercentage);
                guiltPercentageText.color = Managers.GameManager.Instance.GuiltSystem.GetGuiltColor();
            }

            if (guiltMeterFill != null)
            {
                guiltMeterFill.fillAmount = Managers.GameManager.Instance.GuiltSystem.GetNormalizedGuilt();
                guiltMeterFill.color = Managers.GameManager.Instance.GuiltSystem.GetGuiltColor();
            }
        }

        private void OnStateChanged(GameState newState)
        {
            // Hide all panels
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (interrogationPanel != null) interrogationPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);

            // Show appropriate panel
            switch (newState)
            {
                case GameState.MainMenu:
                    if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
                    break;

                case GameState.Interrogation:
                    if (interrogationPanel != null) interrogationPanel.SetActive(true);
                    break;

                case GameState.Victory:
                    if (victoryPanel != null)
                    {
                        victoryPanel.SetActive(true);
                        if (victoryText != null)
                        {
                            float finalGuilt = Managers.GameManager.Instance.GuiltSystem.GuiltPercentage;
                            victoryText.text = string.Format("VERDICT: NOT GUILTY\nGuilt reduced to {0:0}%", finalGuilt);
                        }
                    }
                    break;

                case GameState.GameOver:
                    if (gameOverPanel != null)
                    {
                        gameOverPanel.SetActive(true);
                        if (gameOverText != null)
                        {
                            gameOverText.text = "VERDICT: GUILTY\nExecution initiated";
                        }
                    }
                    break;
            }
        }

        // UI Button Callbacks
        public void OnStartGameButton()
        {
            Managers.GameManager.Instance.StartGame();
        }

        public void OnRestartButton()
        {
            Managers.GameManager.Instance.RestartGame();
        }

        public void OnMainMenuButton()
        {
            Managers.GameManager.Instance.ReturnToMainMenu();
        }

        public void OnQuitButton()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (Managers.GameManager.Instance != null)
            {
                Managers.GameManager.Instance.TimerSystem.OnTimerUpdated -= UpdateTimer;
                Managers.GameManager.Instance.GuiltSystem.OnGuiltChanged -= UpdateGuilt;
                Managers.GameManager.Instance.StateManager.OnStateChanged -= OnStateChanged;
            }
        }
    }
}
