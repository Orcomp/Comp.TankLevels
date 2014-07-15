#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ITank.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels
{
	#region using...
	using System;
	using System.Collections.Generic;
	using Entities;

	#endregion

	/// <summary>
	/// Interface ITank.
	/// Defines operations on a buffer tank that has liquid put into and taken away from it at different points in time.
	/// The buffer tank has a min and max limit.
	/// In current naive version the tank itself does not store the history of its levels (put and take away operations) just the Min/Max values
	/// </summary>
	public interface ITank
	{
		#region Properties
		/// <summary>
		/// Returns predefined minimum allowed quantity for the tank
		/// </summary>
		/// <value>The minimum allowed quantity for the tank</value>
		double MinValue { get; }

		/// <summary>
		/// Returns predefined maximum allowed quantity for the tank
		/// </summary>
		/// <value>The maximum allowed quantity for the tank.</value>
		double MaxValue { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Checks a hypothetical put or take away operation against remaining between the min and max constraints.
		/// </summary>
		/// <param name="startTime">The possible earlier start time of the operation.</param>
		/// <param name="duration">The duration of the operation.</param>
		/// <param name="quantity">The quantity to put or take away. Positive values represent a put operation while negative values are take aways.</param>
		/// <param name="tankLevels">Tank levels are a serie of time/level pairs, and represent the Tank's level over the time.</param>
		/// <returns>CheckOperationResult.</returns>
		CheckOperationResult CheckOperation(DateTime startTime, TimeSpan duration, double quantity, IEnumerable<TankLevel> tankLevels);
		#endregion
	}
}