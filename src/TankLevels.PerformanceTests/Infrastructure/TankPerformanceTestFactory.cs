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
			// TODO: Remove the dummy implementations and add your own here
			//get { return new[] {typeof (DummyTank), typeof (OtherDummyTank)}; }
			get { return new[] {typeof (DummyTank)}; }
		}

		private static string[] TestCaseNames
		{
			get
			{
				return new[]
				{
					"Empty Tank",
					"100 op. on random tank",
					"ZigZag 1",
					"ZigZag 2",
					"ZigZag 1 start at 50%",
					"ZigZag 2 start at 50%",
					"ZigZag 1 start at 75%",
					"ZigZag 2 start at 75%",
					"ZigZag 1 start at 90%",
					"ZigZag 2 start at 90%",
					"One Interval",
					"Calibration (check: size/10)"
				};
			}
		}

		private static IEnumerable<TestCase> SpecialTestCases
		{
			get
			{
				return new[]
				{
					TestCase.ZiZag1,
					TestCase.ZiZag2,
					TestCase.ZiZag150,
					TestCase.ZiZag250,
					TestCase.ZiZag175,
					TestCase.ZiZag275,
					TestCase.ZiZag190,
					TestCase.ZiZag290
					//TestCase.Calibration,
					//TestCase.OneInterval,
				};
			}
		}

		private static IEnumerable<TestCase> RandomTestCases
		{
			get
			{
				return new[]
				{
					//TestCase.EmptyTank,
					TestCase.RandomTank
				};
			}
		}

		private static IEnumerable<int> Sizes
		{
			//get { return new[] {100, 250, 500, 1000, 2000}; }
			get { return new[] {100, 500, 1000, 5000, 10000, 25000, 50000}; }
			//get { return new[] {10, 1000, 10000, 100000}; }
		}
		#endregion

		#region Methods
		public IEnumerable<TestConfiguration> CheckOperationSpecialTestCases()
		{
			return CheckOperationTestCases(SpecialTestCases);
		}

		public IEnumerable<TestConfiguration> CheckOperationRandomTestCases()
		{
			return CheckOperationTestCases(RandomTestCases);
		}

		public IEnumerable<TestConfiguration> CheckOperationTestCases(IEnumerable<TestCase> testCases)
		{
			return from implementationType in ImplementationTypes
			       from testCase in testCases
			       from size in Sizes
			       let prepare = new Action<IPerformanceTestCaseConfiguration>(i =>
			       {
				       var config = (TestConfiguration) i;
				       config.Tank = CreateTank(size, testCase, implementationType);
				       config.TankLevels = CreateTankLevels(size, testCase);
				       config.Parameters = CreateParameters(size, testCase);
			       })
			       let run = new Action<IPerformanceTestCaseConfiguration>(i =>
			       {
				       var config = (TestConfiguration) i;
				       var trueCount = 0;
				       var falseCount = 0;

				       //if (config.Parameters.Length == 0)
				       //{
				       //	Thread.Sleep(config.Size/10);
				       //	return;
				       //}

				       foreach (var p in config.Parameters)
				       {
					       // ReSharper disable once UnusedVariable
					       var result = config.Tank.CheckOperation(p.StartTime, p.Duration, p.Quantity, config.TankLevels);
					       //Debug.WriteLine((result.StartTime - Time(0)).TotalHours);
					       //var dummy = result.IsSuccess ? trueCount++ : falseCount++;
				       }
				       //Debug.WriteLine("I:{0}, S: {1}, C: {2}, NC: {3}", config.Identifier, config.Size, ((SimpleTank)config.Tank).Cache, ((SimpleTank)config.Tank).NoCache);
				       //Debug.WriteLine("True: {0}, False: {1}", trueCount, falseCount);
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

		private IEnumerable<TankLevel> CreateTankLevels(int size, TestCase testCase)
		{
			double minLimit;
			double maxLimit;
			TankLevel[] result;

			GetMinMax(size, testCase, out minLimit, out maxLimit);
			DateTime dateTime;
			switch (testCase)
			{
				case TestCase.EmptyTank:
					return new List<TankLevel>();
					break;

				case TestCase.RandomTank:
				{
					result = new TankLevel[size];
					var tickStep = (Time(TankEndHour) - Time(TankStartHour)).Ticks/size + 1;
					var levelStep = maxLimit/(2*size);

					dateTime = Time(TankStartHour);
					var level = maxLimit/2;
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
				case TestCase.ZiZag1:
				case TestCase.ZiZag150:
				case TestCase.ZiZag175:
				case TestCase.ZiZag190:
				{
					result = new TankLevel[size + 1];
					dateTime = Time(0);
					for (var index = 0; index < size; index++)
					{
						var level = index%2 == 0 ? index/2 : (index/2) + 2;
						result[index] = new TankLevel(dateTime, level);
						dateTime = dateTime.AddHours(1);
					}
					result[size] = new TankLevel(dateTime.AddHours(1), size/2 - 1); // yes, AddHours(_1_)
					return result;
				}
				case TestCase.ZiZag2:
				case TestCase.ZiZag250:
				case TestCase.ZiZag275:
				case TestCase.ZiZag290:

					result = new TankLevel[size + 2];
					dateTime = Time(0);
					for (var index = 0; index < size; index++)
					{
						var level = index%2 == 0 ? 0 : 10;
						result[index] = new TankLevel(dateTime, level);
						dateTime = dateTime.AddHours(1);
					}
					result[size] = new TankLevel(dateTime, 0);
					result[size + 1] = new TankLevel(dateTime.AddHours(1), 9);
					return result;

				case TestCase.OneInterval:
					result = new TankLevel[2];
					result[0] = new TankLevel(Time(0), 0);
					result[1] = new TankLevel(Time(size), 10);
					return result;

				case TestCase.Calibration:
					return new TankLevel[0];

				default:
					throw new ArgumentOutOfRangeException("testCase");
			}
		}

		private CheckOperationParameter[] CreateParameters(int size, TestCase testCase)
		{
			CheckOperationParameter[] result;
			switch (testCase)
			{
				case TestCase.EmptyTank:
				case TestCase.RandomTank:
					result = new CheckOperationParameter[IterationCount];
					for (var index = 0; index < result.Length; index++)
					{
						var startDate = GetRandomDateTime(Time(ParameterStartHour), Time(ParameterEndHour));
						var duration = Duration(GetRandomDouble(ParameterDurationMin, ParameterDurationMax));
						var quantity = GetRandomDouble(ParameterMinQuantity, ParameterMaxQuantity);
						result[index] = new CheckOperationParameter(startDate, duration, quantity);
					}
					return result;
				case TestCase.ZiZag1:
				case TestCase.ZiZag2:
					result = new CheckOperationParameter[1];
					result[0] = new CheckOperationParameter(Time(0), Duration(1), 1.0);
					return result;
				case TestCase.ZiZag150:
				case TestCase.ZiZag250:
					result = new CheckOperationParameter[1];
					result[0] = new CheckOperationParameter(Time(size*.5), Duration(1), 1.0);
					return result;
				case TestCase.ZiZag175:
				case TestCase.ZiZag275:
					result = new CheckOperationParameter[1];
					result[0] = new CheckOperationParameter(Time(size*.75), Duration(1), 1.0);
					return result;
				case TestCase.ZiZag190:
				case TestCase.ZiZag290:
					result = new CheckOperationParameter[1];
					result[0] = new CheckOperationParameter(Time(size*.9), Duration(1), 1.0);
					return result;
				case TestCase.OneInterval:
					result = new CheckOperationParameter[1];
					result[0] = new CheckOperationParameter(Time(0), Duration(1), 1.0);
					return result;
				case TestCase.Calibration:
					return new CheckOperationParameter[0];
				default:
					throw new ArgumentOutOfRangeException("testCase");
			}
		}

		private ITank CreateTank(int size, TestCase testCase, Type type)
		{
			double minLimit;
			double maxLimit;
			GetMinMax(size, testCase, out minLimit, out maxLimit);

			// This is for TankTestBase.CreateTank:
			ImplementationType = type;
			return CreateTank(minLimit, maxLimit);
		}

		private void GetMinMax(int size, TestCase testCase, out double minLimit, out double maxLimit)
		{
			switch (testCase)
			{
				case TestCase.EmptyTank:
					minLimit = double.MinValue;
					maxLimit = double.MaxValue;
					break;
				case TestCase.RandomTank:
					minLimit = TankMinValue;
					maxLimit = TankMaxValue;
					break;
				case TestCase.ZiZag1:
				case TestCase.ZiZag150:
				case TestCase.ZiZag175:
				case TestCase.ZiZag190:

					minLimit = 0;
					maxLimit = size/2 + 1;
					break;
				case TestCase.ZiZag2:
				case TestCase.ZiZag250:
				case TestCase.ZiZag275:
				case TestCase.ZiZag290:

					minLimit = 0;
					maxLimit = 10;
					break;
				case TestCase.OneInterval:
					minLimit = 0;
					maxLimit = 10;
					break;
				case TestCase.Calibration:
					minLimit = 0; // dummy
					maxLimit = 10; // dummy
					break;

				default:
					throw new ArgumentOutOfRangeException("testCase");
			}
		}
		#endregion
	}
}