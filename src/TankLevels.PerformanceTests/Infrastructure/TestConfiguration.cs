#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TestConfiguration.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.PerformanceTests.Infrastructure
{
	#region using...
	using System.Collections.Generic;
	using Entities;
	using global::NUnitBenchmarker.Configuration;

	#endregion

	/// <summary>
	/// Class TestConfiguration.
	/// </summary>
	public class TestConfiguration : PerformanceTestCaseConfigurationBase
	{
		#region Properties
		public int Size { get; set; }
		public string TestName { get; set; }
		public ITank Tank { get; set; }
		public IEnumerable<TankLevel> TankLevels { get; set; }
		public CheckOperationParameter[] Parameters { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return TestName;
		}
		#endregion
	}
}