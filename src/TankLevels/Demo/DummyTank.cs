// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyTank.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TankLevels.Demo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Entities;

    /// <summary>
    /// A dummy <see cref="ITank"/> implementation.
    /// </summary>
    public class DummyTank : ITank
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyTank" /> class.
        /// </summary>
        /// <param name="minValue">The minimum allowed quantity for the tank.</param>
        /// <param name="maxValue">The maximum allowed quantity for the tank.</param>
        public DummyTank(double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        #endregion

        #region ITank Members

        /// <inheritdoc/>
        public double MinValue { get; private set; }

        /// <inheritdoc/>
        public double MaxValue { get; private set; }

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
            Thread.Sleep(tankLevels.Count() / 10000);
            return new CheckOperationResult(startTime.Second % 2 == 0, startTime.AddSeconds(1000));
        }

        #endregion
    }
}