using UnityEngine;
using MercyAICourt.Systems;

namespace MercyAICourt.Managers
{
    /// <summary>
    /// Central singleton manager that coordinates all game systems.
    /// Manages Timer, Guilt, and State systems.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        instance = go.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        private TimerSystem timerSystem;
        private GuiltSystem guiltSystem;
        private GameStateManager stateManager;

        public TimerSystem TimerSystem => timerSystem;
        public GuiltSystem GuiltSystem => guiltSystem;
        public GameStateManager StateManager => stateManager;

        private void Awake()
        {
            // Singleton pattern
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSystems();
        }

        private void InitializeSystems()
        {
            timerSystem = new TimerSystem();
            guiltSystem = new GuiltSystem();
            stateManager = new GameStateManager();

            // Subscribe to events
            timerSystem.OnTimerExpired += OnTimerExpired;
            guiltSystem.OnGuiltChanged += OnGuiltChanged;
        }

        private void Update()
        {
            if (stateManager.IsPlaying())
            {
                timerSystem.Update(Time.deltaTime);
            }
        }

        private void OnTimerExpired()
        {
            // Check if player lost (guilt >= 92%)
            if (!guiltSystem.IsVictoryConditionMet())
            {
                GameOver();
            }
        }

        private void OnGuiltChanged(float newGuilt)
        {
            // Check if player won (guilt < 92%)
            if (stateManager.IsPlaying() && guiltSystem.IsVictoryConditionMet())
            {
                Victory();
            }
        }

        public void StartGame()
        {
            timerSystem.ResetTimer();
            guiltSystem.ResetGuilt();
            stateManager.ChangeState(GameState.Interrogation);
        }

        public void RestartGame()
        {
            StartGame();
        }

        public void PauseGame()
        {
            timerSystem.Pause();
        }

        public void ResumeGame()
        {
            timerSystem.Resume();
        }

        public void TogglePause()
        {
            timerSystem.TogglePause();
        }

        public void Victory()
        {
            stateManager.ChangeState(GameState.Victory);
            timerSystem.Pause();
        }

        public void GameOver()
        {
            stateManager.ChangeState(GameState.GameOver);
            timerSystem.Pause();
        }

        public void ReturnToMainMenu()
        {
            stateManager.ChangeState(GameState.MainMenu);
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                if (timerSystem != null)
                    timerSystem.OnTimerExpired -= OnTimerExpired;
                if (guiltSystem != null)
                    guiltSystem.OnGuiltChanged -= OnGuiltChanged;
            }
        }
    }
}
