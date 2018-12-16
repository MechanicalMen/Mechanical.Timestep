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
    }
}
