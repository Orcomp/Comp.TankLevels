#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="OtherDummyTank.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.Sample
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using Entities;

	#endregion

	/// <summary>
	/// Class OtheDummyTank a dummy ITank implementation 
	/// </summary>
	public class OtherDummyTank : ITank
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="OtherDummyTank" /> class.
		/// </summary>
		/// <param name="minValue">The minimum allowed quantity for the tank.</param>
		/// <param name="maxValue">The maximum allowed quantity for the tank.</param>
		public OtherDummyTank(double minValue = double.MinValue, double maxValue = double.MaxValue)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}
		#endregion

		#region ITank Members
		/// <summary>
		/// Checks a hypothetical put or take away operation against remaining between the min and max constraints.
		/// </summary>
		/// <param name="startTime">The possible earlier start time of the operation.</param>
		/// <param name="duration">The duration of the operation.</param>
		/// <param name="quantity">The quantity to put or take away. Positive values represent a put operation while negative values are take aways.</param>
		/// <param name="tankLevels">Tank levels are a serie of time/level pairs, and represent the Tank's level over the time.</param>
		/// <returns>CheckOperationResult.</returns>
		public CheckOperationResult CheckOperation(DateTime startTime, TimeSpan duration, double quantity, IEnumerable<TankLevel> tankLevels)
		{
			// This dummy code. You can safely delete it and write something real instead.
			Thread.Sleep(tankLevels.Count()/200);
			return new CheckOperationResult(startTime.Second%2 == 0, startTime);
		}

		/// <summary>
		/// Returns predefined minimum allowed quantity for the tank
		/// </summary>
		/// <value>The minimum allowed quantity for the tank.</value>
		public double MinValue { get; private set; }

		/// <summary>
		/// Returns predefined maximum allowed quantity for the tank
		/// </summary>
		/// <value>The maximum allowed quantity for the tank.</value>
		public double MaxValue { get; private set; }
		#endregion
	}
}