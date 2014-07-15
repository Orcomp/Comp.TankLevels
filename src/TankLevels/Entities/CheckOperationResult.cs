#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckOperationResult.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace TankLevels.Entities
{
	#region using...
	using System;

	#endregion

	/// <summary>
	/// Struct CheckOperationResult. Represents a result for <see cref="ITank.CheckOperation" />
	/// </summary>
	public struct CheckOperationResult : IEquatable<CheckOperationResult>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CheckOperationResult" /> struct.
		/// </summary>
		/// <param name="isSuccess">if set to <c>true</c> means this instance represents a successful check operation result.</param>
		/// <param name="startTime">The start time of the operation if it is possible. Otherwise it is N/A and its recommended value is default(<see cref="DateTime" />) </param>
		public CheckOperationResult(bool isSuccess, DateTime startTime) : this()
		{
			IsSuccess = isSuccess;
			StartTime = startTime;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance represent success result.
		/// </summary>
		/// <value><c>true</c> if this instance represents a successful check operation result; otherwise, <c>false</c>.</value>
		public bool IsSuccess { get; private set; }

		/// <summary>
		/// Gets the start time.
		/// </summary>
		/// <value>The start time of the operation if it is possible (IsSuccess is <c>true</c>). Otherwise it is N/A and its recommended value is default(<see cref="DateTime" />).</value>
		public DateTime StartTime { get; private set; }
		#endregion

		#region IEquatable<CheckOperationResult> Members
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		public bool Equals(CheckOperationResult other)
		{
			return IsSuccess.Equals(other.IsSuccess) && StartTime.Equals(other.StartTime);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is CheckOperationResult && Equals((CheckOperationResult) obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (IsSuccess.GetHashCode()*397) ^ StartTime.GetHashCode();
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("IsSuccess: {0}, StartTime: {1}", IsSuccess, StartTime);
		}
		#endregion

		/// <summary>
		/// Implements the ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(CheckOperationResult left, CheckOperationResult right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Implements the !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(CheckOperationResult left, CheckOperationResult right)
		{
			return !left.Equals(right);
		}
	}
}