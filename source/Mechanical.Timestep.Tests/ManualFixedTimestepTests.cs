using System;
using NUnit.Framework;

namespace Mechanical.Timestep.Tests
{
    [TestFixture]
    public static class ManualFixedTimestepTests
    {
        [Test]
        public static void InvalidStepTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ManualFixedTimestep(TimeSpan.FromMilliseconds(-1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ManualFixedTimestep(TimeSpan.Zero));
            new ManualFixedTimestep(TimeSpan.FromMilliseconds(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ManualFixedTimestep(TimeSpan.FromMinutes(10)));
        }

        [Test]
        public static void LastUpdateTimeTest()
        {
            var startDate = new DateTime(1989, 3, 14, 0, 0, 0, DateTimeKind.Unspecified);
            var step = TimeSpan.FromSeconds(1);
            var halfStep = new ElapsedTime(TimeSpan.FromMilliseconds(500));

            // NOTE: the default equality operation does not take DateTimeKind into consideration
            bool equals( DateTime dt1, DateTime dt2 )
            {
                return dt1.Ticks == dt2.Ticks
                    && dt1.Kind == dt2.Kind;
            }

            var timestep = new ManualFixedTimestep(step, startDate);
            Assert.True(equals(startDate, timestep.LastUpdateTime));
            timestep.Update(halfStep);
            Assert.True(equals(startDate + halfStep.Time, timestep.LastUpdateTime));

            timestep = new ManualFixedTimestep(step);
            Assert.True(equals(DateTime.UtcNow.Date, timestep.LastUpdateTime.Date));
        }

        [Test]
        public static void TotalTimestepsAndTimeTest()
        {
            var step = TimeSpan.FromSeconds(1);
            var halfStep = new ElapsedTime(TimeSpan.FromMilliseconds(500));

            var timestep = new ManualFixedTimestep(step);
            Assert.AreEqual(TimeSpan.Zero, timestep.TotalTimestepTime);
            Assert.AreEqual(TimeSpan.Zero, timestep.TotalTime);

            timestep.Update(halfStep);
            Assert.AreEqual(TimeSpan.Zero, timestep.TotalTimestepTime);
            Assert.AreEqual(halfStep.Time, timestep.TotalTime);

            timestep.Update(halfStep);
            Assert.AreEqual(step, timestep.TotalTimestepTime);
            Assert.AreEqual(step, timestep.TotalTime);
        }

        [Test]
        public static void UpdateTest()
        {
            var step = TimeSpan.FromSeconds(1);
            var timestep = new ManualFixedTimestep(step);

            var result = timestep.Update(new ElapsedTime(TimeSpan.Zero));
            Assert.AreEqual(0, result.FullSteps);
            Assert.AreEqual(0d, result.Alpha);

            result = timestep.Update(new ElapsedTime(step));
            Assert.AreEqual(1, result.FullSteps);
            Assert.AreEqual(0d, result.Alpha);

            result = timestep.Update(new ElapsedTime(TimeSpan.FromMilliseconds(200)));
            Assert.AreEqual(0, result.FullSteps);
            Assert.AreEqual(0.2d, result.Alpha);

            result = timestep.Update(new ElapsedTime(TimeSpan.FromMilliseconds(3300)));
            Assert.AreEqual(3, result.FullSteps);
            Assert.AreEqual(0.5d, result.Alpha);
        }
    }
}
