using System;
using NUnit.Framework;

namespace Mechanical.Timestep.Tests
{
    [TestFixture]
    public static class ElapsedStepsTests
    {
        [Test]
        public static void PropertiesMirrorConstructor()
        {
            var dts = new ElapsedSteps(1, 0.1d);
            Assert.AreEqual(1, dts.FullSteps);
            Assert.AreEqual(0.1d, dts.Alpha);
        }

        [Test]
        public static void DefaultConstructorOrZero()
        {
            var dts = new ElapsedSteps();
            Assert.AreEqual(0L, dts.FullSteps);
            Assert.AreEqual(0d, dts.Alpha);

            // zero is valid
            dts = new ElapsedSteps(0, 0);
            Assert.AreEqual(0L, dts.FullSteps);
            Assert.AreEqual(0d, dts.Alpha);
        }

        [Test]
        public static void InvalidConstructorParameters()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedSteps(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedSteps(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedSteps(0, double.NaN));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedSteps(0, double.PositiveInfinity));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ElapsedSteps(0, double.NegativeInfinity));
        }
    }
}
