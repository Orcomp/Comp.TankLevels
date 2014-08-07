#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TankLevel.cs" company="Orcomp development team">
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
	/// Struct TankLevel. Holds a tank level value for a given point of time.
	/// </summary>
	public struct TankLevel : IEquatable<TankLevel>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TankLevel" /> struct.
		/// </summary>
		/// <param name="dateTime">The point of time where to define the level.</param>
		/// <param name="level">The level at the given point of time.</param>
		public TankLevel(DateTime dateTime, double level) : this() 
		{
			DateTime = dateTime;
			Level = level;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the point of time value.
		/// </summary>
		/// <value>The date time.</value>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// Gets the level.
		/// </summary>
		/// <value>The level.</value>
		public double Level { get; set; }
		#endregion

		#region IEquatable<TankLevel> Members
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		public bool Equals(TankLevel other)
		{
			return DateTime.Equals(other.DateTime) && Level.Equals(other.Level);
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
			return obj is TankLevel && Equals((TankLevel) obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (DateTime.GetHashCode()*397) ^ Level.GetHashCode();
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("DateTime: {0}, Level: {1}", DateTime, Level);
		}
		#endregion

		/// <summary>
		/// Implements the ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(TankLevel left, TankLevel right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Implements the !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(TankLevel left, TankLevel right)
		{
			return !left.Equals(right);
		}
	}
}