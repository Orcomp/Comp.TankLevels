// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ITank.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TankLevels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Entities;

    /// <summary>
    /// Interface ITank.
    /// Defines operations on a buffer tank that has liquid put into and taken away from it at different points in time.
    /// The buffer tank has a min and max limit.
    /// In current naive version the tank itself does not store the history of its levels (put and take away operations) just the Min/Max values
    /// </summary>
    [ContractClass(typeof(TankContract))]
    public interface ITank
    {
        #region Properties

        /// <summary>
        /// Returns predefined minimum allowed quantity for the tank.
        /// </summary>
        /// <value>The minimum allowed quantity for the tank.</value>
        double MinLimit { get; }

        /// <summary>
        /// Returns predefined maximum allowed quantity for the tank.
        /// </summary>
        /// <value>The maximum allowed quantity for the tank.</value>
        double MaxLimit { get; }

        #endregion

        #region Methods

        // TODO: Document return value's meaning - not type!
        /// <summary>
        /// Checks a hypothetical put or take away operation against remaining between the min and max constraints.
        /// </summary>
        /// <param name="startTime">The possible earlier start time of the operation.</param>
        /// <param name="duration">The duration of the operation.</param>
        /// <param name="quantity">The quantity to put or take away. Positive values represent a put operation while negative values are take aways.</param>
        /// <param name="tankLevels">Tank levels are a serie of time/level pairs, and represent the Tank's level over the time.</param>
        /// <returns></returns>
        CheckOperationResult CheckOperation(DateTime startTime, TimeSpan duration, double quantity, IEnumerable<TankLevel> tankLevels);

        #endregion
    }

    [ContractClassFor(typeof(ITank))]
    internal abstract class TankContract : ITank
    {
        public double MinLimit
        {
            get
            {
                // Less than MaxValue
                Contract.Ensures(Contract.Result<double>().CompareTo(MaxLimit) <= 0);

                throw new NotImplementedException();
            }
        }

        public double MaxLimit
        {
            get
            {
                // Greater than MinValue
                Contract.Ensures(MinLimit.CompareTo(Contract.Result<double>()) <= 0);

                throw new NotImplementedException();
            }
        }

        public CheckOperationResult CheckOperation(DateTime startTime, TimeSpan duration, double quantity, IEnumerable<TankLevel> tankLevels)
        {

            // Can't have negative duration
            Contract.Requires(duration.Ticks >= 0);

            // All tank levels must be within the tank's limits
            Contract.Requires(Contract.ForAll(tankLevels, tankLevel => MinLimit <= tankLevel.Level && tankLevel.Level <= MaxLimit));

            // Tank levels have to be sorted in time
            Contract.Requires(!tankLevels.Any() || Contract.ForAll(0, tankLevels.Count() - 1, i => tankLevels.ToArray()[i].DateTime <= tankLevels.ToArray()[i + 1].DateTime));


            // Start time is never before first tank level
            Contract.Ensures(!Contract.Result<CheckOperationResult>().IsSuccess
                || tankLevels.First().DateTime.CompareTo(Contract.Result<CheckOperationResult>().StartTime) <= 0);

            // End time is never after last tank level
            Contract.Ensures(!Contract.Result<CheckOperationResult>().IsSuccess
                || (Contract.Result<CheckOperationResult>().StartTime + duration).CompareTo(tankLevels.Last().DateTime) <= 0);

            // After operation, the tank never overflows/underflows
            Contract.Ensures(!Contract.Result<CheckOperationResult>().IsSuccess
                || tankLevels.All(tankLevel => tankLevel.DateTime <= Contract.Result<CheckOperationResult>().StartTime + duration
                    || MinLimit <= tankLevel.Level + quantity && tankLevel.Level + quantity <= MaxLimit
                ));

            throw new NotImplementedException();
        }
    }
}