using System;
using System.Diagnostics;

namespace Mechanical.Timestep
{
    /// <summary>
    /// Helps implementing a fixed timestep algorithm, using <see cref="ManualFixedTimestep"/> and <see cref="Stopwatch"/>.
    /// It starts measuring time immediately upon construction.
    /// This class is not thread-safe.
    /// </summary>
    public class StopwatchFixedTimestep
    {
        #region Private Fields

        private readonly ManualFixedTimestep timestep;
        private readonly Stopwatch stopwatch;
        private TimeSpan lastElapsed = TimeSpan.Zero;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StopwatchFixedTimestep"/> class.
        /// </summary>
        /// <param name="stepLength">The length of a single timestep. For example specify 1/60 seconds, to target 60 FPS. The actual FPS may of course be smaller or larger.</param>
        public StopwatchFixedTimestep( TimeSpan stepLength )
            : this(stepLength, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopwatchFixedTimestep"/> class.
        /// </summary>
        /// <param name="stepLength">The length of a single timestep. For example specify 1/60 seconds, to target 60 FPS. The actual FPS may of course be smaller or larger.</param>
        /// <param name="startTime">The start time, only used in the <see cref="LastUpdateTime"/> property.</param>
        public StopwatchFixedTimestep( TimeSpan stepLength, DateTime startTime )
        {
            this.timestep = new ManualFixedTimestep(stepLength, startTime);
            this.stopwatch = Stopwatch.StartNew();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the number of full timesteps, and the partial timestep, since the last time this method was called (or this instance was created).
        /// </summary>
        /// <returns>The number of full timesteps, as well as the partial timestep (the latter being greater than or equal to zero, but less than one).</returns>
        public void Update( out ElapsedTime dt, out ElapsedSteps steps )
        {
            var currentElapsed = this.stopwatch.Elapsed;
            if( currentElapsed > this.lastElapsed )
            {
                dt = new ElapsedTime(currentElapsed - this.lastElapsed);
                this.lastElapsed = currentElapsed;
            }
            else
            {
                // unfortunately this is possible
                // (precise measurement of small units of time may not always be reliable, on some computers)
                dt = ElapsedTime.Zero;
                //// NOTE: in this case, the partial timestep may still be non-zero!
            }
            steps = this.timestep.Update(dt);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the sum of elapsed, full timesteps.
        /// This is not updated over time, only through <see cref="Update"/>.
        /// It may be less than <see cref="TotalTime"/>.
        /// </summary>
        public TimeSpan TotalTimestepTime => this.timestep.TotalTimestepTime;

        /// <summary>
        /// Gets the total elapsed time, including any partial timestep.
        /// This is not updated over time, only through <see cref="Update"/>.
        /// It may be greater than <see cref="TotalTimestepTime"/>.
        /// </summary>
        public TimeSpan TotalTime => this.timestep.TotalTime;

        /// <summary>
        /// Gets the last time <see cref="Update"/> was invoked
        /// (assuming the start time was correctly specified in the constructor).
        /// </summary>
        public DateTime LastUpdateTime => this.timestep.LastUpdateTime;

        #endregion
    }
}
