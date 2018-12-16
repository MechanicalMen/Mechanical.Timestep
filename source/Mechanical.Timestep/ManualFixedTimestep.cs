using System;

namespace Mechanical.Timestep
{
    /// <summary>
    /// Helps implementing a fixed timestep algorithm.
    /// The elapsed time must be specified manually.
    /// No operations (like rendering or updating game state) are invoked.
    /// This class is not thread-safe.
    /// </summary>
    public class ManualFixedTimestep
    {
        #region Private Fields

        // NOTE: these are used to guard against some rather unlikely edge cases
        private const long Int32Max = (long)int.MaxValue;
        private const long MaxTimestepTicks = long.MaxValue / Int32Max; // about 7 minutes

        private readonly DateTime startTime;
        private readonly TimeSpan timestep;
        private TimeSpan elapsedTimesteps = TimeSpan.Zero;
        private TimeSpan accumulatedTime = TimeSpan.Zero;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualFixedTimestep"/> class.
        /// </summary>
        /// <param name="stepLength">The length of a single timestep. For example specify 1/60 seconds, to target 60 FPS. The actual FPS may of course be smaller or larger.</param>
        public ManualFixedTimestep( TimeSpan stepLength )
            : this(stepLength, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualFixedTimestep"/> class.
        /// </summary>
        /// <param name="stepLength">The length of a single timestep. For example specify 1/60 seconds, to target 60 FPS. The actual FPS may of course be smaller or larger.</param>
        /// <param name="startTime">The start time, only used in the <see cref="LastUpdateTime"/> property.</param>
        public ManualFixedTimestep( TimeSpan stepLength, DateTime startTime )
        {
            if( stepLength <= TimeSpan.Zero
             || stepLength.Ticks > MaxTimestepTicks )
                throw new ArgumentOutOfRangeException(nameof(stepLength));

            this.startTime = startTime;
            this.timestep = stepLength;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the number of full timesteps, and the partial timestep, since the last time this method was called (or this instance was created).
        /// </summary>
        /// <param name="dt">The amount of time elapsed, since the last time this method was called (or this instance was created).</param>
        /// <returns>The elapsed timesteps calculated.</returns>
        public ElapsedSteps Update( ElapsedTime dt )
        {
            // record actual time elapsed
            this.accumulatedTime += dt.Time;

            // round down to multiples of "timestep"
            int numSteps = (int)Math.Min(Int32Max, this.accumulatedTime.Ticks / this.timestep.Ticks);
            var roundedTime = new TimeSpan(numSteps * this.timestep.Ticks); // this does not overflow because of the check in the constructor

            // update statistics
            this.elapsedTimesteps += roundedTime; // this can theoretically overflow, but that means that the total elapsed time is larger than roughly 29K years, in which case you have bigger problems :)
            this.accumulatedTime -= roundedTime;
            var alpha = (this.accumulatedTime.Ticks / (double)this.timestep.Ticks);
            return new ElapsedSteps(numSteps, alpha);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the sum of elapsed, full timesteps.
        /// This is not updated over time, only through <see cref="Update"/>.
        /// It may be less than <see cref="TotalTime"/>.
        /// </summary>
        public TimeSpan TotalTimestepTime => this.elapsedTimesteps;

        /// <summary>
        /// Gets the total elapsed time, including the partial timestep.
        /// This is not updated over time, only through <see cref="Update"/>.
        /// It may be greater than <see cref="TotalTimestepTime"/>.
        /// </summary>
        public TimeSpan TotalTime => this.elapsedTimesteps + this.accumulatedTime;

        /// <summary>
        /// Gets the last time <see cref="Update"/> was invoked
        /// (assuming the start time was correctly specified in the constructor).
        /// </summary>
        public DateTime LastUpdateTime => this.startTime + this.TotalTime;

        #endregion
    }
}
