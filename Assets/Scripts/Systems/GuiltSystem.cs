using UnityEngine;
using System;

namespace MercyAICourt.Systems
{
    /// <summary>
    /// Manages the player's guilt percentage (0-100%).
    /// Color coding: Red (90%+), Yellow (50-89%), Green (<50%)
    /// </summary>
    public class GuiltSystem
    {
        private float guiltPercentage;
        private const float INITIAL_GUILT = 98f;
        private const float MIN_GUILT = 0f;
        private const float MAX_GUILT = 100f;

        public event Action<float> OnGuiltChanged;

        public float GuiltPercentage => guiltPercentage;

        public GuiltSystem()
        {
            ResetGuilt();
        }

        public void ResetGuilt()
        {
            guiltPercentage = INITIAL_GUILT;
            OnGuiltChanged?.Invoke(guiltPercentage);
        }

        public void SetGuilt(float percentage)
        {
            guiltPercentage = Mathf.Clamp(percentage, MIN_GUILT, MAX_GUILT);
            OnGuiltChanged?.Invoke(guiltPercentage);
        }

        public void IncreaseGuilt(float amount)
        {
            SetGuilt(guiltPercentage + amount);
        }

        public void DecreaseGuilt(float amount)
        {
            SetGuilt(guiltPercentage - amount);
        }

        /// <summary>
        /// Returns the appropriate color based on guilt percentage
        /// Red: 90%+, Yellow: 50-89%, Green: <50%
        /// </summary>
        public Color GetGuiltColor()
        {
            if (guiltPercentage >= 90f)
                return Color.red;
            else if (guiltPercentage >= 50f)
                return Color.yellow;
            else
                return Color.green;
        }

        /// <summary>
        /// Returns normalized guilt value (0-1) for UI bars
        /// </summary>
        public float GetNormalizedGuilt()
        {
            return guiltPercentage / MAX_GUILT;
        }

        public bool IsVictoryConditionMet()
        {
            return guiltPercentage < 92f;
        }
    }
}
