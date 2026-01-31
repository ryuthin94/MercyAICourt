using System;

namespace MercyAICourt.Systems
{
    /// <summary>
    /// Manages game states: MainMenu, Interrogation, Victory, GameOver
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Interrogation,
        Victory,
        GameOver
    }

    public class GameStateManager
    {
        private GameState currentState;

        public event Action<GameState> OnStateChanged;

        public GameState CurrentState => currentState;

        public GameStateManager()
        {
            currentState = GameState.MainMenu;
        }

        public void ChangeState(GameState newState)
        {
            if (currentState == newState)
                return;

            currentState = newState;
            OnStateChanged?.Invoke(newState);
        }

        public bool IsPlaying()
        {
            return currentState == GameState.Interrogation;
        }

        public bool IsGameOver()
        {
            return currentState == GameState.GameOver || currentState == GameState.Victory;
        }
    }
}
