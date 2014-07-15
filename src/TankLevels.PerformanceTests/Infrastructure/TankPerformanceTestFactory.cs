#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TankPerformanceTestFactory.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.PerformanceTests.Infrastructure
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Entities;
	using NUnitBenchmarker;
	using NUnitBenchmarker.Configuration;
	using Sample;
	using Tests.Infrastructure;

	#endregion

	public class TankPerformanceTestFactory : TankTestBase
	{
		#region Constants
		private const int IterationCount = 100;
		private const int TankStartHour = 0;
		private const int TankEndHour = 168;
		private const int ParameterStartHour = -72;
		private const int ParameterEndHour = 168 + 72;
		private const int ParameterDurationMin = 0;
		private const int ParameterDurationMax = 72;
		#endregion

		#region Properties
		private IEnumerable<Type> ImplementationTypes
		{
			get { return new[] {typeof (DummyTank), typeof (OtherDummyTank)}; }
		}

		private static string[] TestCaseNames
		{
			get
			{
				return new[]
				{
					"Empty Tank with no max/min",
					"Random Tank with no max/min",
					"Random Tank with max/min"
				};
			}
		}

		private static IEnumerable<TestCase> TestCases
		{
			get
			{
				return new[]
				{
					TestCase.EmptyTankWithNoMaxMin,
					TestCase.RandomTankWithNoMaxMin,
					TestCase.RandomTankWithMaxMin
				};
			}
		}

		private static IEnumerable<int> Sizes
		{
			get { return new[] {100, 250, 500, 1000, 2000}; }
		}
		#endregion

		#region Methods
		public IEnumerable<TestConfiguration> CheckOperationTestCases()
		{
			return from implementationType in ImplementationTypes
			       from testCase in TestCases
			       from size in Sizes
			       let prepare = new Action<IPerformanceTestCaseConfiguration>(i =>
			       {
				       var config = (TestConfiguration) i;
				       config.Tank = CreateTank(testCase, implementationType);
				       config.TankLevels = CreateTankLevels(testCase, size);
				       config.Parameters = CreateParameters(testCase);
			       })
			       let run = new Action<IPerformanceTestCaseConfiguration>(i =>
			       {
				       var config = (TestConfiguration) i;
				       foreach (var p in config.Parameters)
				       {
					       // ReSharper disable once UnusedVariable
					       var result = config.Tank.CheckOperation(p.StartTime, p.Duration, p.Quantity, config.TankLevels);
					       // TODO: Check for expected ratio of true/false results depending on testCase
				       }
			       })
			       select new TestConfiguration
			       {
				       TestName = "Tank CheckOperation",
				       TargetImplementationType = implementationType, // Mandatory to set
				       Identifier = string.Format("{0} {1}", implementationType.GetFriendlyName(), TestCaseNames[(int) testCase]),
				       Size = size,
				       Prepare = prepare,
				       Run = run,
				       IsReusable = true
			       };
		}

		private IEnumerable<TankLevel> CreateTankLevels(TestCase testCase, int size)
		{
			double minValue;
			double maxValue;

			if (testCase == TestCase.EmptyTankWithNoMaxMin)
			{
				return new List<TankLevel>();
			}

			GetMinMax(testCase, out minValue, out maxValue);

			var result = new TankLevel[size];
			for (var index = 0; index < result.Length; index++)
			{
				var startDate = GetRandomDateTime(Time(TankStartHour), Time(TankEndHour));
				var level = GetRandomDouble(minValue, maxValue);
				result[index] = new TankLevel(startDate, level);
			}
			return result;
		}

		private CheckOperationParameter[] CreateParameters(TestCase testCase)
		{
			double minValue;
			double maxValue;

			GetMinMax(testCase, out minValue, out maxValue);
			// TODO: Tune for true/false ratio
			var minQuantity = minValue/2.0;
			var maxQuantity = maxValue/2.0;

			var result = new CheckOperationParameter[IterationCount];
			for (var index = 0; index < result.Length; index++)
			{
				var startDate = GetRandomDateTime(Time(ParameterStartHour), Time(ParameterEndHour));
				var duration = GetRandomTimeSpan(Time(GetRandomDouble(ParameterDurationMin, ParameterDurationMax)) - Time(0));
				var quantity = GetRandomDouble(minQuantity, maxQuantity);
				result[index] = new CheckOperationParameter(startDate, duration, quantity);
			}
			return result;
		}

		private ITank CreateTank(TestCase testCase, Type type)
		{
			double minValue;
			double maxValue;
			GetMinMax(testCase, out minValue, out maxValue);
			ImplementationType = type;
			return CreateTank(minValue, maxValue);
		}

		private void GetMinMax(TestCase testCase, out double minValue, out double maxValue)
		{
			switch (testCase)
			{
				case TestCase.EmptyTankWithNoMaxMin:
				case TestCase.RandomTankWithNoMaxMin:
					minValue = double.MinValue;
					maxValue = double.MaxValue;
					break;
				case TestCase.RandomTankWithMaxMin:
					minValue = -100.0;
					maxValue = 100.0;
					break;
				default:
					throw new ArgumentOutOfRangeException("testCase");
			}
		}
		#endregion
	}
}