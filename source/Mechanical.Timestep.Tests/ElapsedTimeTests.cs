using System;
using NUnit.Framework;

namespace Mechanical.Timestep.Tests
{
    [TestFixture]
    public static class ElapsedTimeTests
    {
        [Test]
        public static void PropertiesMirrorConstructor()
        {
            var dt = new ElapsedTime(TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 2));
            Assert.AreEqual(TimeSpan.TicksPerSecond / 2, dt.Time.Ticks);
            Assert.AreEqual(0.5d, dt.Seconds);

            dt = new ElapsedTime(TimeSpan.TicksPerSecond / 2);
            Assert.AreEqual(TimeSpan.TicksPerSecond / 2, dt.Time.Ticks);
            Assert.AreEqual(0.5d, dt.Seconds);

            dt = new ElapsedTime(0.5);
            Assert.AreEqual(TimeSpan.TicksPerSecond / 2, dt.Time.Ticks);
            Assert.AreEqual(0.5d, dt.Seconds);
        }

        [Test]
        public static void DefaultConstructorOrZero()
        {
            var dt = new ElapsedTime();
            Assert.AreEqual(0L, dt.Time.Ticks);
            Assert.AreEqual(0d, dt.Seconds);

            // zero is valid
            dt = new ElapsedTime(TimeSpan.Zero);
            Assert.AreEqual(0L, dt.Time.Ticks);
            Assert.AreEqual(0d, dt.Seconds);

            dt = new ElapsedTime(0L);
            Assert.AreEqual(0L, dt.Time.Ticks);
            Assert.AreEqual(0d, dt.Seconds);

            dt = new ElapsedTime(0d);
            Assert.AreEqual(0L, dt.Time.Ticks);
            Assert.AreEqual(0d, dt.Seconds);
        }

        [Test]
        public static void InvalidConstructorParameters()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedTime(TimeSpan.FromTicks(-TimeSpan.TicksPerSecond)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedTime(-TimeSpan.TicksPerSecond));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedTime(-1d));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedTime(double.NaN));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedTime(double.PositiveInfinity));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedTime(double.NegativeInfinity));
        }
    }
}
