using System;

namespace Mechanical.Timestep
{
    /// <summary>
    /// The time elapsed between to timesteps (i.e. frames).
    /// </summary>
    public readonly ref struct ElapsedTime
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElapsedTime"/> struct.
        /// </summary>
        /// <param name="time">The time elapsed.</param>
        public ElapsedTime( TimeSpan time )
        {
            if( time < TimeSpan.Zero )
                throw new ArgumentOutOfRangeException(nameof(time));

            this.Time = time;
            this.Seconds = time.TotalSeconds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElapsedTime"/> struct.
        /// </summary>
        /// <param name="ticks">The time elapsed (in ticks).</param>
        public ElapsedTime( long ticks )
            : this(new TimeSpan(ticks))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElapsedTime"/> struct.
        /// </summary>
        /// <param name="seconds">The time elapsed (in seconds).</param>
        public ElapsedTime( double seconds )
        {
            if( double.IsNaN(seconds)
             || double.IsInfinity(seconds)
             || seconds < 0d )
                throw new ArgumentOutOfRangeException(nameof(seconds));

            this.Time = TimeSpan.FromSeconds(seconds);
            this.Seconds = seconds;
        }

        #endregion

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> value representing the time elapsed.
        /// </summary>
        public TimeSpan Time { get; }

        /// <summary>
        /// Gets a <see cref="double"/> value representing the time elapsed (in seconds).
        /// </summary>
        public double Seconds { get; }

        /// <summary>
        /// Gets a <see cref="ElapsedTime"/> instance representing no time having elapsed.
        /// </summary>
        public static ElapsedTime Zero => new ElapsedTime(TimeSpan.Zero);
    }
}
