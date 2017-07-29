using System;
using System.Diagnostics;

namespace Mechanical.Timestep
{
    /// <summary>
    /// Helps implementing a fixed timestep algorithm, using <see cref="Timestep"/> and <see cref="Stopwatch"/>.
    /// It starts measuring time immediately upon construction.
    /// This class is not thread-safe.
    /// </summary>
    public class StopwatchTimestep
    {
        #region Private Fields

        private readonly Timestep timestep;
        private readonly Stopwatch stopwatch;
        private TimeSpan lastElapsed = TimeSpan.Zero;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StopwatchTimestep"/> class.
        /// </summary>
        /// <param name="stepLength">The length of a single timestep. For example specify 1/60 seconds, to target 60 FPS. The actual FPS may of course be smaller or larger.</param>
        public StopwatchTimestep( TimeSpan stepLength )
            : this(stepLength, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopwatchTimestep"/> class.
        /// </summary>
        /// <param name="stepLength">The length of a single timestep. For example specify 1/60 seconds, to target 60 FPS. The actual FPS may of course be smaller or larger.</param>
        /// <param name="startTime">The start time, only used in the <see cref="LastUpdateTime"/> property.</param>
        public StopwatchTimestep( TimeSpan stepLength, DateTime startTime )
        {
            this.timestep = new Timestep(stepLength);
            this.stopwatch = Stopwatch.StartNew();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the number of full timesteps, and the partial timestep, since the last time this method was called (or this instance was created).
        /// </summary>
        /// <returns>The number of full timesteps, as well as the partial timestep (the latter being greater than or equal to zero, but less than one).</returns>
        public (int fullSteps, float alpha) GetElapsedSteps()
        {
            var currentElapsed = this.stopwatch.Elapsed;
            if( currentElapsed > this.lastElapsed )
            {
                var result = this.timestep.GetElapsedSteps(currentElapsed - this.lastElapsed);
                this.lastElapsed = currentElapsed;
                return result;
            }
            else
            {
                // unfortunately this is possible
                // (precise measurement of small units of time may not always be reliable, on some computers)
                return this.timestep.GetElapsedSteps(TimeSpan.Zero); // the alpha component returned may still be non-zero!
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the sum of elapsed, full timesteps.
        /// This is not updated over time, only through <see cref="GetElapsedSteps"/>.
        /// It may be less than <see cref="TotalTime"/>.
        /// </summary>
        public TimeSpan TotalTimesteps => this.timestep.TotalTimesteps;

        /// <summary>
        /// Gets the total elapsed time, including any partial timestep.
        /// This is not updated over time, only through <see cref="GetElapsedSteps"/>.
        /// It may be greater than <see cref="TotalTimesteps"/>.
        /// </summary>
        public TimeSpan TotalTime => this.timestep.TotalTime;

        /// <summary>
        /// Gets the last time <see cref="GetElapsedSteps"/> was invoked
        /// (assuming the start time was correctly specified in the constructor).
        /// </summary>
        public DateTime LastUpdateTime => this.timestep.LastUpdateTime;

        #endregion
    }
}
