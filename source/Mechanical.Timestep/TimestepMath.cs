using System.Runtime.CompilerServices;

namespace Mechanical.Timestep
{
    /// <summary>
    /// Basic math utility functions. 
    /// </summary>
    public static class TimestepMath
    {
        #region UpdatePosition

        /// <summary>
        /// Updates position and velocity (using the semi-implicit euler method).
        /// </summary>
        /// <param name="position">The position to update.</param>
        /// <param name="velocity">The velocity to update.</param>
        /// <param name="acceleration">The current acceleration.</param>
        /// <param name="dt">The time elapsed.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdatePosition(
            ref double position,
            ref double velocity,
            double acceleration,
            ElapsedTime dt )
        {
            velocity += acceleration * dt.Seconds;
            position += velocity * dt.Seconds;
        }

        /// <summary>
        /// Updates position and velocity (using the semi-implicit euler method).
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="currentVelocity">The current velocity.</param>
        /// <param name="acceleration">The current acceleration.</param>
        /// <param name="dt">The time elapsed.</param>
        /// <returns>The updated position and velocity.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double position, double velocity) UpdatePosition(
            double currentPosition,
            double currentVelocity,
            double acceleration,
            ElapsedTime dt )
        {
            var newVelocity = currentVelocity + acceleration * dt.Seconds;
            var newPosition = currentPosition + newVelocity * dt.Seconds;
            return (newPosition, newVelocity);
        }

        #endregion

        #region Lerp

        /// <summary>
        /// Basic linear interpolation.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="alpha">The point between the old and new values to interpolate. MUST be between <c>0</c> and <c>1</c>.</param>
        /// <returns>The interpolated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Lerp( double oldValue, double newValue, double alpha )
        {
            return oldValue * (1d - alpha) + newValue * alpha;
        }

        #endregion
    }
}
