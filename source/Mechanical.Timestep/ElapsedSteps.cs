using System;

namespace Mechanical.Timestep
{
    /// <summary>
    /// The elapsed timesteps.
    /// </summary>
    public readonly ref struct ElapsedSteps
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ElapsedSteps"/> struct.
        /// </summary>
        /// <param name="fullSteps">The number of elapsed timesteps (i.e. frames).</param>
        /// <param name="alpha">The partially elapsed timestep.
        /// This is a number between <c>0</c> (inclusive) and <c>1</c> (exclusive).
        /// The closer it is to <c>1</c>, the closer the next timestep is.</param>
        public ElapsedSteps( int fullSteps, double alpha )
        {
            if( fullSteps < 0 )
                throw new ArgumentOutOfRangeException(nameof(fullSteps));

            if( double.IsNaN(alpha)
             || double.IsInfinity(alpha)
             || alpha < 0
             || alpha >= 1 )
                throw new ArgumentOutOfRangeException(nameof(alpha));

            this.FullSteps = fullSteps;
            this.Alpha = alpha;
        }

        #endregion

        /// <summary>
        /// Gets the number of elapsed timesteps (i.e. frames).
        /// </summary>
        public int FullSteps { get; }

        /// <summary>
        /// Gets the partially elapsed timestep.
        /// This is a number between <c>0</c> (inclusive) and <c>1</c> (exclusive).
        /// The closer it is to <c>1</c>, the closer the next timestep is.
        /// </summary>
        public double Alpha { get; }

        /// <summary>
        /// Gets a <see cref="ElapsedSteps"/> instance representing no timesteps having elapsed.
        /// </summary>
        public static ElapsedSteps Zero => new ElapsedSteps(0, 0d);
    }
}
