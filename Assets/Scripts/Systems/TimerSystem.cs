using UnityEngine;
using System;

namespace MercyAICourt.Systems
{
    /// <summary>
    /// Manages the game timer. 90 in-game minutes maps to 5 real-world minutes.
    /// 1 real second = 18 game seconds (5 minutes * 60 = 300 real seconds / 90 game minutes * 60 = 5400 game seconds)
    /// </summary>
    public class TimerSystem
    {
        private float gameSecondsRemaining;
        private bool isPaused;
        private const float GAME_SECONDS_PER_REAL_SECOND = 18f;
        private const float INITIAL_GAME_SECONDS = 90f * 60f; // 90 minutes in seconds

        public event Action OnTimerExpired;
        public event Action<float> OnTimerUpdated;

        public bool IsPaused => isPaused;
        public float GameSecondsRemaining => gameSecondsRemaining;

        public TimerSystem()
        {
            ResetTimer();
        }

        public void ResetTimer()
        {
            gameSecondsRemaining = INITIAL_GAME_SECONDS;
            isPaused = false;
            OnTimerUpdated?.Invoke(gameSecondsRemaining);
        }

        public void Update(float deltaTime)
        {
            if (isPaused || gameSecondsRemaining <= 0)
                return;

            gameSecondsRemaining -= deltaTime * GAME_SECONDS_PER_REAL_SECOND;

            if (gameSecondsRemaining <= 0)
            {
                gameSecondsRemaining = 0;
                OnTimerExpired?.Invoke();
            }

            OnTimerUpdated?.Invoke(gameSecondsRemaining);
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
        }

        public void AddTime(float gameSeconds)
        {
            gameSecondsRemaining = Mathf.Clamp(gameSecondsRemaining + gameSeconds, 0, INITIAL_GAME_SECONDS);
            OnTimerUpdated?.Invoke(gameSecondsRemaining);
        }

        public void SubtractTime(float gameSeconds)
        {
            gameSecondsRemaining = Mathf.Max(0, gameSecondsRemaining - gameSeconds);
            OnTimerUpdated?.Invoke(gameSecondsRemaining);
            
            if (gameSecondsRemaining <= 0)
            {
                OnTimerExpired?.Invoke();
            }
        }

        /// <summary>
        /// Returns the timer formatted as MM:SS
        /// </summary>
        public string GetFormattedTime()
        {
            int totalSeconds = Mathf.FloorToInt(gameSecondsRemaining);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
