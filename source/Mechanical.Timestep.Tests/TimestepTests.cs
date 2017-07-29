using System;
using NUnit.Framework;

namespace Mechanical.Timestep.Tests
{
    [TestFixture]
    public static class TimestepTests
    {
        [Test]
        public static void InvalidStepTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Timestep(TimeSpan.FromMilliseconds(-1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Timestep(TimeSpan.Zero));
            new Timestep(TimeSpan.FromMilliseconds(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Timestep(TimeSpan.FromMinutes(10)));
        }

        [Test]
        public static void LastUpdateTimeTest()
        {
            var startDate = new DateTime(1989, 3, 14, 0, 0, 0, DateTimeKind.Unspecified);
            var step = TimeSpan.FromSeconds(1);
            var halfStep = TimeSpan.FromMilliseconds(500);

            // NOTE: the default equality operation does not take DateTimeKind into consideration
            bool equals( DateTime dt1, DateTime dt2 )
            {
                return dt1.Ticks == dt2.Ticks
                    && dt1.Kind == dt2.Kind;
            }

            var timestep = new Timestep(step, startDate);
            Assert.True(equals(startDate, timestep.LastUpdateTime));
            timestep.GetElapsedSteps(halfStep);
            Assert.True(equals(startDate + halfStep, timestep.LastUpdateTime));

            timestep = new Timestep(step);
            Assert.True(equals(DateTime.UtcNow.Date, timestep.LastUpdateTime.Date));
        }

        [Test]
        public static void TotalTimestepsAndTimeTest()
        {
            var step = TimeSpan.FromSeconds(1);
            var halfStep = TimeSpan.FromMilliseconds(500);

            var timestep = new Timestep(step);
            Assert.AreEqual(TimeSpan.Zero, timestep.TotalTimesteps);
            Assert.AreEqual(TimeSpan.Zero, timestep.TotalTime);

            timestep.GetElapsedSteps(halfStep);
            Assert.AreEqual(TimeSpan.Zero, timestep.TotalTimesteps);
            Assert.AreEqual(halfStep, timestep.TotalTime);

            timestep.GetElapsedSteps(halfStep);
            Assert.AreEqual(step, timestep.TotalTimesteps);
            Assert.AreEqual(step, timestep.TotalTime);
        }

        [Test]
        public static void GetElapsedStepsTest()
        {
            var step = TimeSpan.FromSeconds(1);
            var timestep = new Timestep(step);

            var result = timestep.GetElapsedSteps(TimeSpan.Zero);
            Assert.AreEqual(0, result.fullSteps);
            Assert.AreEqual(0f, result.alpha);

            result = timestep.GetElapsedSteps(step);
            Assert.AreEqual(1, result.fullSteps);
            Assert.AreEqual(0f, result.alpha);

            result = timestep.GetElapsedSteps(TimeSpan.FromMilliseconds(200));
            Assert.AreEqual(0, result.fullSteps);
            Assert.AreEqual(0.2f, result.alpha);

            result = timestep.GetElapsedSteps(TimeSpan.FromMilliseconds(3300));
            Assert.AreEqual(3, result.fullSteps);
            Assert.AreEqual(0.5f, result.alpha);
        }
    }
}
