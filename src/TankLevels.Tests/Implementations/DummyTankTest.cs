// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyTankTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TankLevels.Tests.Implementations
{
	using Demo;
	using NUnit.Framework;
	using TankInterface;

	[TestFixture]
	public class DummyTankTest : TankTest
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			// TODO: Replace DummyTank with your own implementation
			ImplementationType = typeof (DummyTank);
		}
	}
}