#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TankTestBase.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.Tests.Infrastructure
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Demo;
	using Entities;
	using NUnit.Framework;

	#endregion

	/// <summary>
	/// Class TankTestBase. Contains helper methods for ITank testing
	/// </summary>
	public class TankTestBase
	{
		#region Constants
		private const double DoubleMissing = 987.654;
		private static readonly DateTime Start = new DateTime(2000, 1, 1, 0, 0, 0);
		private static readonly Random Random = new Random(0);

		protected static Type ImplementationType;
		#endregion

		#region Methods
		[DebuggerStepThrough]
		protected static TimeSpan Duration(int hours)
		{
			return new TimeSpan(0, hours, 0, 0);
		}

		protected static TimeSpan Duration(double hours)
		{
			return TimeSpan.FromHours(hours);
		}

		[DebuggerStepThrough]
		protected static DateTime Time(int hours)
		{
			return Start.AddHours(hours);
		}

		[DebuggerStepThrough]
		protected static DateTime Time(int hours, int minutes, int second, double milliseconds)
		{
			return Start.AddHours(hours).AddMinutes(minutes).AddSeconds(second).AddMilliseconds(milliseconds);
		}

		[DebuggerStepThrough]
		public static DateTime Time(double hours)
		{
			return Start.AddHours(hours);
		}

		public static TimeSpan GetRandomTimeSpan(TimeSpan from = default(TimeSpan), TimeSpan to = default(TimeSpan))
		{
			if (Equals(from, default(TimeSpan)))
			{
				from = TimeSpan.MinValue;
			}

			if (Equals(to, default(TimeSpan)))
			{
				to = TimeSpan.MaxValue;
			}
			var range = to - @from;
			var randTimeSpan = new TimeSpan((long) (Random.NextDouble()*range.Ticks));
			return from + randTimeSpan;
		}

		public static IEnumerable<TimeSpan> GetRandomTimeSpans(int count, TimeSpan from = default(TimeSpan), TimeSpan to = default(TimeSpan))
		{
			for (var i = 0; i < count; i++)
			{
				yield return GetRandomTimeSpan(from, to);
			}
		}

		public static DateTime GetRandomDateTime(DateTime from = default(DateTime), DateTime to = default(DateTime))
		{
			if (Equals(from, default(DateTime)))
			{
				from = DateTime.MinValue;
			}

			if (Equals(to, default(DateTime)))
			{
				to = DateTime.MaxValue;
			}
			var range = to - @from;
			var randTimeSpan = new TimeSpan((long) (Random.NextDouble()*range.Ticks));
			return from + randTimeSpan;
		}

		public static IEnumerable<DateTime> GetRandomDateTimes(int count, DateTime from = default(DateTime), DateTime to = default(DateTime))
		{
			for (var i = 0; i < count; i++)
			{
				yield return GetRandomDateTime(from, to);
			}
		}

		public static double GetRandomDouble(double from = double.MinValue, double to = double.MaxValue)
		{
			return Random.NextDouble()*(to - from) + from;
		}

		public static IEnumerable<double> GetRandomDoubles(int count, double from = double.MinValue, double to = double.MaxValue)
		{
			for (var i = 0; i < count; i++)
			{
				yield return GetRandomDouble(from, to);
			}
		}

		protected static ITank CreateTank(double minValue = DoubleMissing, double maxValue = DoubleMissing)
		{
			if (ImplementationType == null)
			{
				ImplementationType = typeof (DummyTank);
			}

			var constructorInfos = ImplementationType.GetConstructors().Where(ci => ci.GetParameters().Count() == 2);
			return (ITank) constructorInfos.First()
				.Invoke(new[]
				{
					minValue.Equals(DoubleMissing) ? Type.Missing : minValue,
					maxValue.Equals(DoubleMissing) ? Type.Missing : maxValue
				});
		}

		protected static IEnumerable<TankLevel> MultiplyTankLevels(IEnumerable<TankLevel> tankLevels, int multiplyer)
		{
			return tankLevels.Select(tankLevel => MultiplyTankLevel(tankLevel, multiplyer));
		}

		private static TankLevel MultiplyTankLevel(TankLevel tankLevel, int multiplyer)
		{
			return new TankLevel(tankLevel.DateTime, tankLevel.Level*multiplyer);
		}

		protected static void ActAndAssert(DateTime startTime, TimeSpan duration, double quantity, IEnumerable<TankLevel> tankLevels, bool expectedIsSuccess, double expectedHour, double minValue, double maxValue)
		{
			var tankLevelsArray = tankLevels.ToArray();

			var tank = CreateTank(minValue, maxValue);
			ActAndAssert(startTime, duration, quantity, tankLevelsArray, expectedIsSuccess, expectedHour, tank);

			tank = CreateTank(-maxValue, -minValue);
			tankLevelsArray = MultiplyTankLevels(tankLevelsArray, -1).ToArray();
			ActAndAssert(startTime, duration, -quantity, tankLevelsArray, expectedIsSuccess, expectedHour, tank);
		}

		private static void ActAndAssert(DateTime startTime, TimeSpan duration, double quantity, IEnumerable<TankLevel> tankLevels, bool expectedIsSuccess, double expectedHour, ITank tank)
		{
			var tla = tankLevels.ToArray();
			var result = tank.CheckOperation(startTime, duration, quantity, tla);

			// Test sabotage. Comment the next 2 lines out to check the test logic and data
			// expectedIsSuccess = true; // Should make all expected false fail
			// expectedHour = expectedHour + 0.000001; // Should make all expected true fail by slightly changed starttime

			if (result.IsSuccess)
			{
				Assert.IsTrue(expectedIsSuccess);

				// 5000 ticks equals 50 nanoseconds.
				Assert.Less(Math.Abs(Time(expectedHour).Ticks - result.StartTime.Ticks), 5000);
			}
			else
			{
				Assert.IsFalse(expectedIsSuccess);
			}
		}
		#endregion
	}
}