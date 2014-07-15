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
	using Sample;

	#endregion

	public class TankTestBase
	{
		#region Constants
		private const double DoubleMissing = 987.654;
		private static readonly DateTime Start = new DateTime(2000, 1, 1, 0, 0, 0);
		private static readonly Random Random = new Random(0);
		#endregion

		#region Fields
		protected Type ImplementationType;
		#endregion

		#region Methods
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

		protected ITank CreateTank(double minValue = DoubleMissing, double maxValue = DoubleMissing)
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
		#endregion
	}
}