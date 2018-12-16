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
    }
}
