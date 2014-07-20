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
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Demo;
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
	[TestFixture]
	public class TankTest : TankTestBase
	{
		[Test]
		[TestCase(0, true, 5)]
		[TestCase(4.95, true, 5)]
		[TestCase(4.96, true, 5)]
		[TestCase(4.97, true, 5)]
		[TestCase(4.98, true, 5)]
		[TestCase(4.99, true, 5)]
		[TestCase(5, true, 5)]
		[TestCase(5.01, true, 7.51)]
		[TestCase(5.02, true, 7.52)]
		[TestCase(5.03, true, 7.53)]
		[TestCase(5.04, true, 7.54)]
		[TestCase(5.05, true, 7.55)]
		[TestCase(5.1, true, 7.6)]
		[TestCase(9.9, true, 12.4)]
		[TestCase(10, true, 12.5)]
		[TestCase(10.0000001, false, -1)]
		public void CheckOperation_Constant(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			#region Timeline
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			// 0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16
			// Existing operations:
			// |--- Put 5 --------------|(5)
			//                                                   |---Take away 5----------|(-5)
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
			#endregion

			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 5.0),
				new TankLevel(Time(15), 0.0),
				new TankLevel(Time(100), 0.0),
			};

			var startTime = Time(5);
			var duration = Duration(2.5);
			const int minValue = -10;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		[TestCase(0, true, 5)]
		[TestCase(5, true, 5)]
		[TestCase(7.45, true, 5)]
		[TestCase(7.46, true, 5)]
		[TestCase(7.47, true, 5)]
		[TestCase(7.48, true, 5)]
		[TestCase(7.49, true, 5)]
		[TestCase(7.5, true, 5)]
		[TestCase(7.51, true, 5.01)]
		[TestCase(7.52, true, 5.02)]
		[TestCase(7.53, true, 5.03)]
		[TestCase(7.54, true, 5.04)]
		[TestCase(7.55, true, 5.05)]
		[TestCase(7.6, true, 5.1)]
		[TestCase(9.9, true, 7.4)]
		[TestCase(9.95, true, 7.45)]
		[TestCase(9.96, true, 7.46)]
		[TestCase(9.97, true, 7.47)]
		[TestCase(9.98, true, 7.48)]
		[TestCase(9.99, true, 7.49)]
		[TestCase(10, true, 7.5)]
		[TestCase(10.0000001, false, -1)]
		public void CheckOperation_Decreasing(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			#region Timeline
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
			#endregion

			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 0.0),
				new TankLevel(Time(100), 0.0),
			};

			var startTime = Time(5);
			var duration = Duration(2.5);
			const int minValue = -10;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		[TestCase(0, false, -1)]
		[TestCase(5, false, -1)]
		[TestCase(100000, false, -1)]
		public void CheckOperation_EmptyTank(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			var tankLevels = new TankLevel[]
			{
			};

			var startTime = Time(5);
			var duration = Duration(10);
			const int minValue = -10;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		// Serie 0:  _/\_ with top of 10 and ground level 0
		//        Se,St, D   Q, Min, Max 
		[TestCase(0, 0, 2.5, 5, -15, 15, true, 0)]
		[TestCase(0, 0, 2.5, 5.01, -15, 15, true, 2.505)]
		[TestCase(0, 0, 2.5, 15, -15, 15, true, 7.5)]
		[TestCase(0, 0, 2.5, 15.000001, -15, 15, false, -1)]
		[TestCase(0, 5, 2.5, 10, -15, 15, true, 5)]
		[TestCase(0, 5, 2.5, 10.1, -15, 15, true, 5.05)]
		[TestCase(0, 5, 2.5, 15, -15, 15, true, 7.5)]
		[TestCase(0, 5, 2.5, 15.00001, -15, 15, false, -1)]
		[TestCase(0, 0, 6.0, 6, -15, 15, true, 0)]
		[TestCase(0, 0, 6.0, 6.01, -15, 15, true, 0.0083195045)]

		// Serie 1: _/-\_ with top of 10 and ground level 0
		//        Se,St, D   Q, Min, Max 
		[TestCase(1, 0, 2.5, 0, -15, 15, true, 0)]
		[TestCase(1, 0, 2.5, 5, -15, 15, true, 0)]
		[TestCase(1, 0, 2.5, 5.01, -15, 15, true, 7.505)]
		[TestCase(1, 0, 2.5, 5.02, -15, 15, true, 7.51)]
		[TestCase(1, 0, 2.5, 5.03, -15, 15, true, 7.515)]
		[TestCase(1, 0, 2.5, 5.04, -15, 15, true, 7.52)]
		[TestCase(1, 0, 2.5, 5.05, -15, 15, true, 7.525)]
		[TestCase(1, 0, 2.5, 15, -15, 15, true, 12.5)]
		[TestCase(1, 0, 2.5, 15.0001, -15, 15, false, -1)]
		[TestCase(1, 0, 6, 0, -15, 15, true, 0)]
		[TestCase(1, 0, 6, 5, -15, 15, true, 0)]
		[TestCase(1, 0, 6, 5.01, -15, 15, true, 4.01197601102778)] // endless 
		[TestCase(1, 0, 6, 5.02, -15, 15, true, 4.02390439058333)] // endless 
		[TestCase(1, 0, 6, 5.03, -15, 15, true, 4.03578530994444)] // endless 
		[TestCase(1, 0, 6, 5.04, -15, 15, true, 4.04761906705556)] // endless 
		[TestCase(1, 0, 6, 5.05, -15, 15, true, 4.05940596008333)] // endless 
		[TestCase(1, 0, 6, 15, -15, 15, true, 9.0)]
		[TestCase(1, 0, 2.5, 15.0001, -15, 15, false, -1)]


		// Serie 2: -\/-  with top of 10 and bottom of 0
		//        Se,St, D   Q, Min, Max 
		[TestCase(2, 0, 2.5, 0, -15, 15, true, 0)]
		[TestCase(2, 0, 2.5, 5, -15, 15, true, 0)]
		[TestCase(2, 0, 2.5, 5.00001, -15, 15, false, -1)]
		[TestCase(2, 0, 0.00001, 5, -15, 15, true, 0)]
		[TestCase(2, 0, 0.00001, 5.00001, -15, 15, false, -1)]
		[TestCase(2, 0, 0.0, 5, -15, 15, true, 0)]
		[TestCase(2, 0, 0.0, 5.00001, -15, 15, false, -1)]

		// Serie 3: -\_/-  with top of 10 and bottom of 0
		//        Se,St, D   Q, Min, Max 
		[TestCase(3, 0, 2.5, 0, -15, 15, true, 0)]
		[TestCase(3, 0, 2.5, 5, -15, 15, true, 0)]
		[TestCase(3, 0, 2.5, 5.00001, -15, 15, false, -1)]
		[TestCase(3, 0, 0.00001, 5, -15, 15, true, 0)]
		[TestCase(3, 0, 0.00001, 5.00001, -15, 15, false, -1)]
		[TestCase(3, 0, 0.0, 5, -15, 15, true, 0)]
		[TestCase(3, 0, 0.0, 5.00001, -15, 15, false, -1)]

		// Serie 4: Continous rising 
		//        Se,St, D   Q, Min, Max 
		[TestCase(4, 0, 1, 0, -10, 10, true, 0)]
		[TestCase(4, 0, 1, 5, -10, 10, true, 0)]
		[TestCase(4, 0, 1, 5.00001, -10, 10, false, -1)]

		// Serie 5: Continous falling
		//        Se,St, D   Q, Min, Max 
		[TestCase(5, 0, 1, 0, -10, 10, true, 0)]
		[TestCase(5, 0, 1, 6, -10, 10, true, 0)]
		[TestCase(5, 0, 1, 6.5, -10, 10, true, 0.5)]
		[TestCase(5, 0, 1, 7, -10, 10, true, 1)]
		[TestCase(5, 0, 1, 7.5, -10, 10, true, 1.5)]
		[TestCase(5, 0, 1, 8, -10, 10, true, 2)]
		[TestCase(5, 0, 1, 8.5, -10, 10, true, 2.5)]
		[TestCase(5, 0, 1, 9, -10, 10, true, 3)]
		[TestCase(5, 0, 1, 9.5, -10, 10, true, 3.5)]
		[TestCase(5, 0, 1, 10, -10, 10, true, 4)]
		[TestCase(5, 0, 1, 10.00001, -10, 10, false, -1)]

		// Serie 6: Staircase up
		//        Se,St, D   Q, Min, Max 
		[TestCase(6, 0, 0.5, 0, -10, 10, true, 0)]
		[TestCase(6, 0, 0.5, 5, -10, 10, true, 0)]
		[TestCase(6, 0, 0.5, 5.00001, -10, 10, false, -1)]
		[TestCase(6, 0, 1.0, 0, -10, 10, true, 0)]
		[TestCase(6, 0, 1.0, 5, -10, 10, true, 0)]
		[TestCase(6, 0, 1.0, 5.00001, -10, 10, false, -1)]
		[TestCase(6, 0, 2.0, 0, -10, 10, true, 0)]
		[TestCase(6, 0, 2.0, 5, -10, 10, true, 0)]
		[TestCase(6, 0, 2.0, 5.00001, -10, 10, false, -1)]
		[TestCase(6, 0, 0.0, 0, -10, 10, true, 0)]
		[TestCase(6, 0, 0.0, 5, -10, 10, true, 0)]
		[TestCase(6, 0, 0.0, 5.00001, -10, 10, false, -1)]

		// Serie 7: Staircase down
		//        Se,St, D   Q, Min, Max 
		[TestCase(7, 0, 0.5, 0, -10, 10, true, 0)]
		[TestCase(7, 0, 0.5, 4.5, -10, 10, true, 0)]
		[TestCase(7, 0, 0.5, 5, -10, 10, true, 0)]
		[TestCase(7, 0, 0.5, 5.5, -10, 10, true, 0.545454531833333)] // endless 
		[TestCase(7, 0, 0.5, 6, -10, 10, true, 0.583333343194444)] // endless 
		[TestCase(7, 0, 0.5, 6.5, -10, 10, true, 1.53846153611111)] // endless 
		[TestCase(7, 0, 0.5, 7, -10, 10, true, 1.57142856711111)] // endless 
		[TestCase(7, 0, 0.5, 7.5, -10, 10, true, 2.53333333130556)] // endless 
		[TestCase(7, 0, 0.5, 8, -10, 10, true, 2.56249997019444)] // endless 
		[TestCase(7, 0, 0.5, 8.5, -10, 10, true, 3.52941176291667)] // endless 
		[TestCase(7, 0, 0.5, 9, -10, 10, true, 3.55555555219444)] // endless 
		[TestCase(7, 0, 0.5, 9.5, -10, 10, true, 4.52631577844444)] // endless 
		[TestCase(7, 0, 0.5, 10, -10, 10, true, 4.54999998205556)] // endless 
		[TestCase(7, 0, 0.5, 10.00001, -10, 10, false, -1)]
		[TestCase(7, 0, 1, 0, -10, 10, true, 0)]
		[TestCase(7, 0, 1, 5, -10, 10, true, 0)]
		[TestCase(7, 0, 1, 5.5, -10, 10, true, 0.0909090935555556)] // endless 
		[TestCase(7, 0, 1, 6, -10, 10, true, 0.166666656666667)] // endless 
		[TestCase(7, 0, 1, 6.5, -10, 10, true, 1.07692310208333)] // endless 
		[TestCase(7, 0, 1, 7, -10, 10, true, 1.14285716408333)] // endless 
		[TestCase(7, 0, 1, 7.5, -10, 10, true, 2.06666669244444)] // endless 
		[TestCase(7, 0, 1, 8, -10, 10, true, 2.12499997019444)] // endless 
		[TestCase(7, 0, 1, 8.5, -10, 10, true, 3.05882355566667)] // endless 
		[TestCase(7, 0, 1, 9, -10, 10, true, 3.11111113425)] 
		[TestCase(7, 0, 1, 9.5, -10, 10, true, 4.05263158675)]
		[TestCase(7, 0, 1, 10, -10, 10, true, 4.09999999397222)]
		[TestCase(7, 0, 1, 10.00001, -10, 10, false, -1)]
		[TestCase(7, 0, 2, 0, -10, 10, true, 0)]
		[TestCase(7, 0, 2, 6, -10, 10, true, 0)]
		[TestCase(7, 0, 2, 6.5, -10, 10, true, 0.153846174416667)]
		[TestCase(7, 0, 2, 7, -10, 10, true, 0.285714298444444)]
		[TestCase(7, 0, 2, 7.5, -10, 10, true, 1.13333335513889)]
		[TestCase(7, 0, 2, 8, -10, 10, true, 1.24999997019444)]
		[TestCase(7, 0, 2, 8.5, -10, 10, true, 2.11764708158333)]
		[TestCase(7, 0, 2, 9, -10, 10, true, 2.22222223872222)]
		[TestCase(7, 0, 2, 9.5, -10, 10, true, 3.10526314372222)]
		[TestCase(7, 0, 2, 10, -10, 10, true, 3.20000001780556)]
		[TestCase(7, 0, 2, 10.00001, -10, 10, false, -1)]
		[TestCase(7, 0, 0, 0, -10, 10, true, 0)]
		[TestCase(7, 0, 0, 5, -10, 10, true, 0)]
		[TestCase(7, 0, 0, 5.00001, -10, 10, true, 1)]
		[TestCase(7, 0, 0, 5.5, -10, 10, true, 1)]
		[TestCase(7, 0, 0, 6, -10, 10, true, 1)]
		[TestCase(7, 0, 0, 6.00001, -10, 10, true, 2)]
		[TestCase(7, 0, 0, 6.5, -10, 10, true, 2)]
		[TestCase(7, 0, 0, 7, -10, 10, true, 2)]
		[TestCase(7, 0, 0, 7.00001, -10, 10, true, 3)]
		[TestCase(7, 0, 0, 7.5, -10, 10, true, 3)]
		[TestCase(7, 0, 0, 8, -10, 10, true, 3)]
		[TestCase(7, 0, 0, 8.00001, -10, 10, true, 4)]
		[TestCase(7, 0, 0, 8.5, -10, 10, true, 4)]
		[TestCase(7, 0, 0, 9, -10, 10, true, 4)]
		[TestCase(7, 0, 0, 9.00001, -10, 10, true, 5)]
		[TestCase(7, 0, 0, 9.5, -10, 10, true, 5)]
		[TestCase(7, 0, 0, 10, -10, 10, true, 5)]
		[TestCase(7, 0, 0, 10.00001, -10, 10, false, -1)]
		public void CheckOperation_Generic(int serie, int startTime, double duration, double quantity, double minValue, double maxValue, bool expectedIsSuccess, double expectedHour)
		{
			#region Fill the tank
			var allTankLevels = new List<TankLevel>
			{
				// _/\_ with top of 10 and ground level 0
				
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 10.0),
				new TankLevel(Time(10), 0.0),

				// _/-\_ with top of 10 and ground level 0
				
				new TankLevel(Time(100), 0.0),
				new TankLevel(Time(105), 10.0),
				new TankLevel(Time(110), 10.0),
				new TankLevel(Time(115), 0.0),
				new TankLevel(Time(199), 0.0),

				// -\/-  with top of 10 and bottom of 0
				
				new TankLevel(Time(200), 10.0),
				new TankLevel(Time(205), 10.0),
				new TankLevel(Time(210), 0.0),
				new TankLevel(Time(215), 10.0),
				new TankLevel(Time(299), 10.0),

				// -\_/-  with top of 10 and bottom of 0
				
				new TankLevel(Time(300), 10.0),
				new TankLevel(Time(305), 10.0),
				new TankLevel(Time(310), 0.0),
				new TankLevel(Time(315), 0.0),
				new TankLevel(Time(320), 10.0),
				new TankLevel(Time(399), 10.0),

				// Continous rising 
				
				new TankLevel(Time(400), 0.0),
				new TankLevel(Time(401), 1.0),
				new TankLevel(Time(402), 2.0),
				new TankLevel(Time(403), 3.0),
				new TankLevel(Time(404), 4.0),
				new TankLevel(Time(405), 5.0),
				new TankLevel(Time(499), 5.0),

				// Continous fall
				
				new TankLevel(Time(500), 5.0),
				new TankLevel(Time(501), 4.0),
				new TankLevel(Time(502), 3.0),
				new TankLevel(Time(503), 2.0),
				new TankLevel(Time(504), 1.0),
				new TankLevel(Time(505), 0.0),
				new TankLevel(Time(599), 0.0),

				// Staircase up
				
				new TankLevel(Time(600), 0.0),
				new TankLevel(Time(601), 0.0),
				new TankLevel(Time(601), 1.0),
				new TankLevel(Time(602), 1.0),
				new TankLevel(Time(602), 2.0),
				new TankLevel(Time(603), 2.0),
				new TankLevel(Time(603), 3.0),
				new TankLevel(Time(604), 3.0),
				new TankLevel(Time(604), 4.0),
				new TankLevel(Time(605), 4.0),
				new TankLevel(Time(605), 5.0),
				new TankLevel(Time(699), 5.0),

				// Staircase down
				
				new TankLevel(Time(700), 5.0),
				new TankLevel(Time(701), 5.0),
				new TankLevel(Time(701), 4.0),
				new TankLevel(Time(702), 4.0),
				new TankLevel(Time(702), 3.0),
				new TankLevel(Time(703), 3.0),
				new TankLevel(Time(703), 2.0),
				new TankLevel(Time(704), 2.0),
				new TankLevel(Time(704), 1.0),
				new TankLevel(Time(705), 1.0),
				new TankLevel(Time(705), 0.0),
				new TankLevel(Time(799), 0.0),
			};
			#endregion

			// Select the tanklevel serie (by parameter serie) for testing:
			var serieOffset = TimeSpan.FromHours(serie*100);
			var tankLevels = (from tankLevel in allTankLevels
			                  where tankLevel.DateTime >= Time(serie*100) && tankLevel.DateTime < Time((serie + 1)*100)
			                  select new TankLevel(tankLevel.DateTime - serieOffset, tankLevel.Level)).ToList();

			ActAndAssert(Time(startTime), Duration(duration), quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		[TestCase(0, true, 0)]
		[TestCase(2.5, true, 0)]
		[TestCase(5, true, 0)]
		[TestCase(5.0000000001, true, 2.5)]
		[TestCase(5.001, true, 2.501)]
		[TestCase(5.002, true, 2.502)]
		[TestCase(5.003, true, 2.5030000)]
		[TestCase(5.004, true, 2.5040000)]
		[TestCase(5.005, true, 2.505)]
		[TestCase(5.1, true, 2.6000000)]
		[TestCase(9.9, true, 7.4)]
		[TestCase(9.95, true, 7.45)]
		[TestCase(9.96, true, 7.46)]
		[TestCase(9.97, true, 7.47)]
		[TestCase(9.98, true, 7.48)]
		[TestCase(9.99, true, 7.49)]
		[TestCase(10, true, 7.5)]
		[TestCase(10.000001, false, -1)]
		public void CheckOperation_Increasing(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			#region Timeline
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
			// | <- startTime = 0
			// |------------| duration = 2.5, quantity is variable
			//
			// |----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|----|
			// 0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16
			#endregion

			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 0.0),
			};

			var startTime = Time(0);
			var duration = Duration(2.5);
			const int minValue = -10;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		[TestCase(0, true, 5)]
		[TestCase(7.45, true, 5)]
		[TestCase(7.46, true, 5)]
		[TestCase(7.47, true, 5)]
		[TestCase(7.48, true, 5)]
		[TestCase(7.49, true, 5)]
		[TestCase(7.5, true, 5)]
		[TestCase(7.51, true, 5.01)]
		[TestCase(7.52, true, 5.02)]
		[TestCase(7.53, true, 5.03)]
		[TestCase(7.54, true, 5.04)]
		[TestCase(7.55, true, 5.05)]
		[TestCase(7.60, true, 5.1)]
		[TestCase(9.9, true, 7.4)]
		[TestCase(10, true, 7.5)]
		[TestCase(10.1, true, 7.52475247525)]
		[TestCase(109.9, true, 9.77252047316667)]
		[TestCase(109.95, true, 9.77262388908333)]
		[TestCase(109.96, true, 9.77264460169444)]
		[TestCase(109.97, true, 9.77266523980555)]
		[TestCase(109.98, true, 9.77268595241667)]
		[TestCase(109.99, true, 9.7727065905)]
		[TestCase(110, true, 9.77272727272727)]
		[TestCase(110.0000001, false, -1)]
		public void CheckOperation_NoZeroTimeChangeInLevels_Decreasing(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			#region Timeline
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
			#endregion

			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 0.0),
				new TankLevel(Time(10).AddMilliseconds(1), -100.0),
				new TankLevel(Time(100), -100.0),
			};

			var startTime = Time(5);
			var duration = Duration(2.5);
			const int minValue = -100;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}

		[Test]
		[TestCase(0, true, 5)]
		[TestCase(7.45, true, 5)]
		[TestCase(7.46, true, 5)]
		[TestCase(7.47, true, 5)]
		[TestCase(7.48, true, 5)]
		[TestCase(7.49, true, 5)]
		[TestCase(7.5, true, 5)]
		[TestCase(7.51, true, 5.01)]
		[TestCase(7.52, true, 5.02)]
		[TestCase(7.53, true, 5.03)]
		[TestCase(7.54, true, 5.04)]
		[TestCase(7.55, true, 5.05)]
		[TestCase(7.60, true, 5.1)]
		[TestCase(9.9, true, 7.4)]
		[TestCase(10, true, 7.5)]
		[TestCase(10.1, true, 7.52475247522222)]
		[TestCase(109.9, true, 9.77252047313889)]
		[TestCase(109.95, true, 9.77262388908333)]
		[TestCase(109.96, true, 9.77264460169444)]
		[TestCase(109.97, true, 9.77266523980555)]
		[TestCase(109.98, true, 9.77268595241667)]
		[TestCase(109.99, true, 9.7727065905)]
		[TestCase(110, true, 9.7727272727272)]
		[TestCase(110.00000001, false, -1)]
		public void CheckOperation_ZeroTimeChangeInLevels(double quantity, bool expectedIsSuccess, double expectedHour)
		{
			#region Timeline
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
			#endregion

			var tankLevels = new List<TankLevel>
			{
				new TankLevel(Time(0), 0.0),
				new TankLevel(Time(5), 5.0),
				new TankLevel(Time(10), 0.0),
				new TankLevel(Time(10), -100.0), // ZeroTimeChange
				new TankLevel(Time(100), -100.0),
			};

			var startTime = Time(5);
			var duration = Duration(2.5);
			const int minValue = -100;
			const int maxValue = 10;

			ActAndAssert(startTime, duration, quantity, tankLevels, expectedIsSuccess, expectedHour, minValue, maxValue);
		}
	}
}