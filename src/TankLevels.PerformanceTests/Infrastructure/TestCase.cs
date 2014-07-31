#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCase.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.PerformanceTests.Infrastructure
{
	public enum TestCase
	{
		EmptyTank = 0,
		RandomTank = 1,
		ZiZag1 = 2,
		ZiZag2 = 3,
		ZiZag150 = 4,
		ZiZag250 = 5,
		ZiZag175 = 6,
		ZiZag275 = 7,
		ZiZag190 = 8,
		ZiZag290 = 9,
		OneInterval = 10,
		Calibration = 11
	}
}