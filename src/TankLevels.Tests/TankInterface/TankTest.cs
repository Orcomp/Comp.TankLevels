#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TankTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.Tests.TankInterface
{
	#region using...
	using System.Collections.Generic;
	using Entities;
	using Implementations;
	using Infrastructure;
	using NUnit.Framework;

	#endregion

	/// <summary>
	/// Tests implementations against of ITank interface. Can be used with different implementations
	/// simply by inheriting from this class. For reference sample see <see cref="DummyTankTest" />
	/// <para>
	/// This TestFixture is not for running directly so marked with [Explicit]. All the tests from this
	/// class will run indirectly by its inherited classes like  <see cref="DummyTankTest" />
	/// </para>
	/// <para>
	/// Usage guidelines:
	/// ----------------
	/// To allow reuse this class with different implementations of ITank always use black box testing
	/// against ITank. A factory method is available for creating test target instances without having
	/// to refer to a concrete ITank implementation: <see cref="TankTestBase.CreateTank" />.
	/// This factory method's signature mimics the <see cref="DummyTank" /> implementation's constructor signature.
	/// Implementors  can choose not to follow this constructor signature (because an interface definition
	/// like ITank can not constrain constructor) however in this case that implementation will not be
	/// testable by simply reusing this test class.
	/// </para>
	/// </summary>
	[Explicit]
	public class TankTest : TankTestBase
	{
		#region Methods
		[Test]
		[TestCase(0, true, 5)]
		[TestCase(9.99, true, 5)]
		[TestCase(10.0, true, 5)]
		[TestCase(10.000000001, false, -1)]
		[TestCase(100000.0, false, -1)]
		public void CheckOperation_EmptyTank(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 0.0),
				new TankLevel(Time(10).AddMilliseconds(1), -100.0),
			};

			var startTime = Time(5);
			var duration = Duration(10);
			const int minValue = -10;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		[TestCase(0, true, 5)]
		[TestCase(7.49, true, 5)]
		[TestCase(7.50, true, 5)]
		[TestCase(7.51, true, 5.01)]
		[TestCase(7.60, true, 5.1)]
		[TestCase(9.9, true, 7.4)]
		[TestCase(10, true, 7.5)]
		[TestCase(10.1, true, 7.52475247525)]
		[TestCase(109.9, true, 9.77252047316667)]
		[TestCase(110, true, 9.77272727272222)]
		[TestCase(110.1, false, -1)]
		public void CheckOperation_NoZeroTimeChangeInLevels_VariableQuantity(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			// 0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16
			// Existing operations:
			// |--- Put 5 --------------|(5)
			//                          |---Take away 5----------|(-5)
			//
			//
			// Levels:
			// 0.0 1.0  2.0  3.0  4.0  5.0  4.0  3.0  2.0  1.0  0.0, -100.0 from 10 + one msec ->
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			//
			// Check for:
			//                          | <- startTime = 5
			//                          |------------| duration = 2.5, quantity is variable
			//
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			// 0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16
			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 0.0),
				new TankLevel(Time(10).AddMilliseconds(1), -100.0),
			};

			var startTime = Time(5);
			var duration = Duration(2.5);
			const int minValue = -100;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		[TestCase(0, true, 5)]
		[TestCase(7.49, true, 5)]
		[TestCase(7.50, true, 5)]
		[TestCase(7.51, true, 5.01)]
		[TestCase(7.60, true, 5.1)]
		[TestCase(9.9, true, 7.4)]
		[TestCase(10, true, 7.49999999997222)]
		[TestCase(10.1, true, 7.52475247522222)]
		[TestCase(109.9, true, 9.77252047313889)]
		[TestCase(110, true, 9.77272727269444)]
		[TestCase(110.1, false, -1)]
		public void CheckOperation_ZeroTimeChangeInLevels_VariableQuantity(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			// 0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16
			// Existing operations:
			// |--- Put 5 --------------|(5)
			//                          |---Take away 5----------|(-5)
			//
			//
			// Levels:
			// 0.0 1.0  2.0  3.0  4.0  5.0  4.0  3.0  2.0  1.0  0.0 and -100.0 ->
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			//
			// Check for:
			//                          | <- startTime = 5
			//                          |------------| duration = 2.5, quantity is variable
			//
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			// 0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16
			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 0.0),
				new TankLevel(Time(10), -100.0), // ZeroTimeChange
				//new TankLevel(Time(20).AddMilliseconds(1), -100.0),
			};

			var startTime = Time(5);
			var duration = Duration(2.5);
			const int minValue = -100;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}
		#endregion
	}
}