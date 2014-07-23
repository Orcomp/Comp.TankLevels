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
	using System.Diagnostics;
	using System.Linq;
	using Demo;
	using Entities;
	using NUnitBenchmarker;
	using NUnitBenchmarker.Configuration;
	using Tests.Infrastructure;

	#endregion

	public class TankPerformanceTestFactory : TankTestBase
	{
		

		#region Constants
		private const int IterationCount = 100;

		private const double TankMinValue = -100;
		private const double TankMaxValue = 100;
		private const double TankStartHour = 0;
		private const double TankEndHour = 168;

		private const double ParameterMinQuantity = -120;
		private const double ParameterMaxQuantity = 120;
		private const double ParameterStartHour = -10;
		private const double ParameterEndHour = 168 + 10;
		private const double ParameterDurationMin = 0.0;
		private const double ParameterDurationMax = 36.0;
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
					"Empty Tank",
					"Tank with no max/min",
					"Tank with max/min"
				};
			}
		}

		private static IEnumerable<TestCase> TestCases
		{
			get
			{
				return new[]
				{
					//TestCase.EmptyTank,
					//TestCase.TankWithNoMaxMin,
					TestCase.TankWithMaxMin
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
				       var trueCount = 0;
					   var falseCount = 0;
				       foreach (var p in config.Parameters)
				       {
					       // ReSharper disable once UnusedVariable
					       var result = config.Tank.CheckOperation(p.StartTime, p.Duration, p.Quantity, config.TankLevels);
					       var dummy = result.IsSuccess ? trueCount++ : falseCount++;
				       }
					   Debug.WriteLine("True: {0}, False: {1}", trueCount, falseCount);
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
			 
			if (testCase == TestCase.EmptyTank)
			{
				return new List<TankLevel>();
			}

			GetMinMax(testCase, out minValue, out maxValue);

			var result = new TankLevel[size];
			var tickStep = (Time(TankEndHour) - Time(TankStartHour)).Ticks/size + 1;
			var levelStep = maxValue/(2*size);

			var dateTime = Time(TankStartHour);
			var level = maxValue/2;
			for (var index = 0; index < result.Length; index++)
			{
				result[index] = new TankLevel(dateTime, level);
				dateTime = dateTime.AddTicks(tickStep);
				level -= levelStep;
				level = -level;
				levelStep = -levelStep;
			}
			return result;
		}

		private CheckOperationParameter[] CreateParameters(TestCase testCase)
		{

			var result = new CheckOperationParameter[IterationCount];
			for (var index = 0; index < result.Length; index++)
			{
				var startDate = GetRandomDateTime(Time(ParameterStartHour), Time(ParameterEndHour));
				var duration = Duration(GetRandomDouble(ParameterDurationMin, ParameterDurationMax));
				var quantity = GetRandomDouble(ParameterMinQuantity, ParameterMaxQuantity);
				result[index] = new CheckOperationParameter(startDate, duration, quantity);
			}
			return result;
		}

		private ITank CreateTank(TestCase testCase, Type type)
		{
			double minValue;
			double maxValue;
			GetMinMax(testCase, out minValue, out maxValue);

			// This is for TankTestBase.CreateTank:
			ImplementationType = type;
			return CreateTank(minValue, maxValue);
		}

		private void GetMinMax(TestCase testCase, out double minValue, out double maxValue)
		{
			switch (testCase)
			{
				case TestCase.EmptyTank:
				case TestCase.TankWithNoMaxMin:
					minValue = double.MinValue;
					maxValue = double.MaxValue;
					break;
				case TestCase.TankWithMaxMin:
					minValue = TankMinValue;
					maxValue = TankMaxValue;
					break;
				default:
					throw new ArgumentOutOfRangeException("testCase");
			}
		}
		#endregion
	}
}