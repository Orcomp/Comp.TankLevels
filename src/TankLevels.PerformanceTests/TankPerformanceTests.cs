#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TankPerformanceTests.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.PerformanceTests
{
	#region using...
	using Infrastructure;
	using NUnit.Framework;
	using NUnitBenchmarker;

	#endregion

	[TestFixture]
	public class TankPerformanceTests
	{
		[TestFixtureSetUp]
		public void TestFixture()
		{
			Benchmarker.Init();
		}

		[Test, TestCaseSource(typeof (TankPerformanceTestFactory), "CheckOperationSpecialTestCases")]
		public void CheckOperationSpecial(TestConfiguration config)
		{
			config.Benchmark(config.TestName, config.Size, 10);
		}

		[Test, TestCaseSource(typeof(TankPerformanceTestFactory), "CheckOperationRandomTestCases")]
		public void CheckOperationRandom(TestConfiguration config)
		{
			config.Benchmark(config.TestName, config.Size, 10);
		}
	}
}