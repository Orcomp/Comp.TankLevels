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

		[Test, TestCaseSource(typeof (TankPerformanceTestFactory), "CheckOperationTestCases")]
		public void CheckOperation(TestConfiguration config)
		{
			config.IsReusable = false; // Default
			config.Benchmark(config.TestName, config.Size, 10);
		}
	}
}