#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckOperationParameter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.PerformanceTests.Infrastructure
{
	#region using...
	using System;

	#endregion

	/// <summary>
	/// Struct CheckOperationParameter
	/// </summary>
	public struct CheckOperationParameter
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CheckOperationParameter" /> struct.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="quantity">The quantity.</param>
		public CheckOperationParameter(DateTime startTime, TimeSpan duration, double quantity) : this()
		{
			StartTime = startTime;
			Duration = duration;
			Quantity = quantity;
		}
		#endregion

		#region Properties
		public DateTime StartTime { get; set; }
		public TimeSpan Duration { get; set; }
		public double Quantity { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("StartTime: {0}, Duration: {1}, Quantity: {2}", StartTime, Duration, Quantity);
		}
		#endregion
	}
}