using NUnit.Framework;

namespace Mechanical.Timestep.Tests
{
    [TestFixture]
    public static class TimestepMathTests
    {
        [Test]
        public static void UpdatePosition()
        {
            double position = 0;
            double velocity = 0;
            double acceleration = 10;
            var dt = new ElapsedTime(seconds: 1d);

            TimestepMath.UpdatePosition(ref position, ref velocity, acceleration, dt);
            Assert.AreEqual(10, position);
            Assert.AreEqual(10, velocity);

            TimestepMath.UpdatePosition(ref position, ref velocity, acceleration, dt);
            Assert.AreEqual(30, position);
            Assert.AreEqual(20, velocity);

            TimestepMath.UpdatePosition(ref position, ref velocity, acceleration, dt);
            Assert.AreEqual(60, position);
            Assert.AreEqual(30, velocity);

            // reproduce using overload
            position = velocity = 0;
            for( int i = 0; i < 3; ++i )
                (position, velocity) = TimestepMath.UpdatePosition(position, velocity, acceleration, dt);
            Assert.AreEqual(60, position);
            Assert.AreEqual(30, velocity);
        }

        [Test]
        public static void Lerp()
        {
            var value = TimestepMath.Lerp(3, 5, alpha: 0);
            Assert.AreEqual(3, value);

            value = TimestepMath.Lerp(3, 5, alpha: 1);
            Assert.AreEqual(5, value);

            value = TimestepMath.Lerp(3, 5, alpha: 0.5);
            Assert.AreEqual(4, value);

            value = TimestepMath.Lerp(3, 5, alpha: 0.1);
            Assert.AreEqual(3.2, value);
        }
    }
}
