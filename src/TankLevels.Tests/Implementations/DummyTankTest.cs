#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyTankTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.Tests.Implementations
{
	#region using...
	using Demo;
	using NUnit.Framework;
	using TankInterface;

	#endregion

	[TestFixture]
	public class DummyTankTest : TankTest
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			ImplementationType = typeof (DummyTank);
		}
	}
}