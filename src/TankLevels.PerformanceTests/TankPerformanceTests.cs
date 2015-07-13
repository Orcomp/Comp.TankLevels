// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TankPerformanceTests.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TankLevels.PerformanceTests
{
    using Infrastructure;
    using NUnit.Framework;
    using NUnitBenchmarker;

    [TestFixture]
    public class TankPerformanceTests
    {
        [TestFixtureSetUp]
        public void TestFixture()
        {
            Benchmarker.Init();
        }

        [Test, TestCaseSource(typeof(TankPerformanceTestFactory), "CheckOperationZigZag1TestCases")]
        public void CheckOperationZigZag1(TestConfiguration config)
        {
            config.Benchmark(config.TestName, config.Size, 3);
        }

        [Test, TestCaseSource(typeof(TankPerformanceTestFactory), "CheckOperationZigZag2TestCases")]
        public void CheckOperationZigZag2(TestConfiguration config)
        {
            config.Benchmark(config.TestName, config.Size, 3);
        }

        [Test, TestCaseSource(typeof(TankPerformanceTestFactory), "CheckOperationRandomTestCases")]
        public void CheckOperationRandom(TestConfiguration config)
        {
            config.Benchmark(config.TestName, config.Size, 3);
        }
    }
}