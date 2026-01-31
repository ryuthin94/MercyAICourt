using System;

namespace MercyAICourt.Tests
{
    /// <summary>
    /// Simple standalone test to verify core game logic without Unity.
    /// This can be run in Unity's Test Runner or adapted for standalone testing.
    /// </summary>
    public class CoreSystemsTest
    {
        public static void RunTests()
        {
            Console.WriteLine("=== Mercy AI Court Core Systems Test ===\n");
            
            TestTimerSystem();
            TestGuiltSystem();
            TestGameStateManager();
            
            Console.WriteLine("\n=== All Tests Complete ===");
        }

        private static void TestTimerSystem()
        {
            Console.WriteLine("--- Timer System Test ---");
            
            var timer = new MercyAICourt.Systems.TimerSystem();
            
            // Test initial state
            Assert(timer.GameSecondsRemaining == 5400f, "Initial time should be 5400 seconds (90 minutes)");
            Assert(timer.GetFormattedTime() == "90:00", "Initial formatted time should be 90:00");
            
            // Test countdown
            timer.Update(1f); // 1 real second = 18 game seconds
            Assert(timer.GameSecondsRemaining == 5382f, "After 1 second, should have 5382 seconds remaining");
            
            // Test pause
            timer.Pause();
            Assert(timer.IsPaused, "Timer should be paused");
            float beforePause = timer.GameSecondsRemaining;
            timer.Update(1f);
            Assert(timer.GameSecondsRemaining == beforePause, "Time should not decrease when paused");
            
            // Test resume
            timer.Resume();
            Assert(!timer.IsPaused, "Timer should not be paused");
            
            // Test add/subtract time
            timer.AddTime(60f);
            Assert(timer.GameSecondsRemaining > beforePause, "Adding time should increase remaining time");
            
            timer.SubtractTime(120f);
            Assert(timer.GameSecondsRemaining < beforePause, "Subtracting time should decrease remaining time");
            
            // Test reset
            timer.ResetTimer();
            Assert(timer.GameSecondsRemaining == 5400f, "Reset should return to 5400 seconds");
            
            Console.WriteLine("✓ Timer System tests passed");
        }

        private static void TestGuiltSystem()
        {
            Console.WriteLine("--- Guilt System Test ---");
            
            var guilt = new MercyAICourt.Systems.GuiltSystem();
            
            // Test initial state
            Assert(guilt.GuiltPercentage == 98f, "Initial guilt should be 98%");
            Assert(!guilt.IsVictoryConditionMet(), "Victory condition should not be met at 98%");
            
            // Test color coding
            guilt.SetGuilt(95f);
            Assert(guilt.GetGuiltColor() == UnityEngine.Color.red, "Guilt >= 90% should be red");
            
            guilt.SetGuilt(70f);
            Assert(guilt.GetGuiltColor() == UnityEngine.Color.yellow, "Guilt 50-89% should be yellow");
            
            guilt.SetGuilt(30f);
            Assert(guilt.GetGuiltColor() == UnityEngine.Color.green, "Guilt < 50% should be green");
            
            // Test victory condition
            guilt.SetGuilt(91f);
            Assert(guilt.IsVictoryConditionMet(), "Victory condition should be met below 92%");
            
            guilt.SetGuilt(92f);
            Assert(!guilt.IsVictoryConditionMet(), "Victory condition should not be met at 92%");
            
            // Test clamping
            guilt.SetGuilt(150f);
            Assert(guilt.GuiltPercentage == 100f, "Guilt should be clamped to 100%");
            
            guilt.SetGuilt(-10f);
            Assert(guilt.GuiltPercentage == 0f, "Guilt should be clamped to 0%");
            
            // Test increase/decrease
            guilt.SetGuilt(50f);
            guilt.IncreaseGuilt(10f);
            Assert(guilt.GuiltPercentage == 60f, "IncreaseGuilt should add to current value");
            
            guilt.DecreaseGuilt(15f);
            Assert(guilt.GuiltPercentage == 45f, "DecreaseGuilt should subtract from current value");
            
            Console.WriteLine("✓ Guilt System tests passed");
        }

        private static void TestGameStateManager()
        {
            Console.WriteLine("--- Game State Manager Test ---");
            
            var stateManager = new MercyAICourt.Systems.GameStateManager();
            
            // Test initial state
            Assert(stateManager.CurrentState == MercyAICourt.Systems.GameState.MainMenu, 
                   "Initial state should be MainMenu");
            Assert(!stateManager.IsPlaying(), "Should not be playing in MainMenu");
            Assert(!stateManager.IsGameOver(), "Should not be game over in MainMenu");
            
            // Test state changes
            stateManager.ChangeState(MercyAICourt.Systems.GameState.Interrogation);
            Assert(stateManager.CurrentState == MercyAICourt.Systems.GameState.Interrogation, 
                   "State should change to Interrogation");
            Assert(stateManager.IsPlaying(), "Should be playing in Interrogation");
            
            stateManager.ChangeState(MercyAICourt.Systems.GameState.Victory);
            Assert(stateManager.IsGameOver(), "Victory should count as game over");
            
            stateManager.ChangeState(MercyAICourt.Systems.GameState.GameOver);
            Assert(stateManager.IsGameOver(), "GameOver should count as game over");
            
            Console.WriteLine("✓ Game State Manager tests passed");
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"Assertion failed: {message}");
            }
        }
    }
}
