﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitClassLibrary
{
    /// <summary>
    /// Class used for storing Angles that may need to be accessed in a different measurement system
    /// Accepts anything as input
    /// 
    /// For an explanation of why this class is immutatble: http://codebetter.com/patricksmacchia/2008/01/13/immutable-types-understand-them-and-use-them/
    /// 
    /// <example>
    /// radians into degrees then returned as string
    /// 
    /// double radians = 10.5;
    /// Angle a = new Angle(AngleType.Radian, radians);
    /// 
    /// a.Degrees.ToString()         //for decimal degrees
    /// a.ToString(AngleType.Degree) //for formated string
    /// </example>
    /// 
    /// </summary>
    public class Angle : IComparable<Angle>
    {
        #region private fields and constants

        //Internally we are currently using radians
        private double _intrinsicValue;
        private static AngleType InternalUnitType = AngleType.Radian;

        #endregion

        #region Properties
        /// <summary>
        /// returns the Angle in Radians
        /// </summary>
        public double Radians
        {
            get { return retrieveAsExternalUnit(AngleType.Radian); }
        }

        /// <summary>
        /// returns the Angle in Degrees
        /// </summary>
        public double Degrees
        {
            get { return retrieveAsExternalUnit(AngleType.Degree); }
        }
        #endregion

        #region helper methods
        /// <summary>
        /// converts an Angle's Radians to Degrees
        /// </summary>
        /// <param name="radians">the radians for the Angle</param>
        /// <returns>the string equivalent of the degrees</returns>
        private static string ConvertDecimalRadiansToDegreesString(double radians)
        {
            //convert out of radians
            double degreesCumulative = radians / (Math.PI / 180);

            //save off the whole degrees
            double degrees = Math.Floor(degreesCumulative);

            //only left with minutes and seconds as a remainder of degrees
            degreesCumulative -= degrees;

            //convert to minutes
            degreesCumulative *= 60;

            //save off the whole minutes
            double minutes = Math.Floor(degreesCumulative);

            //only left with seconds as a remainder of minutes
            degreesCumulative -= degrees;

            //convert to seconds
            degreesCumulative *= 60;

            //round off to whole seconds
            double seconds = Math.Round(degreesCumulative);

            //now convert numbers to strings
            string degreeString = degrees.ToString();
            string minutesString = minutes.ToString();
            string secondsString = seconds.ToString();

            //format symbols
            return string.Format("{0}°{1}'{2}\"", degreeString, minutesString, secondsString);
        }

        /// <summary>
        /// returns the angle in double form as radians or degrees
        /// </summary>
        /// <param name="angleType">the type of angle to convert to</param>
        /// <returns>the double value of the angle</returns>
        private double retrieveAsExternalUnit(AngleType angleType)
        {
            switch (angleType)
            {
                case AngleType.Radian:
                    return _intrinsicValue;
                case AngleType.Degree:
                    return _intrinsicValue / (Math.PI / 180);
                default:
                    //code should never be run
                    return 2;  // there is no enum for 2 so should crash
            }
        }

        /// <summary>
        /// stores a double as the input angle type
        /// </summary>
        /// <param name="fromAngleType">angle type the double represents</param>
        /// <param name="passedValue">angle value</param>
        private void storeAsInternalUnit(AngleType fromAngleType, double passedValue)
        {
            switch (fromAngleType)
            {
                case AngleType.Radian:
                    _intrinsicValue = passedValue;
                    break;
                case AngleType.Degree:
                    _intrinsicValue = passedValue * (Math.PI / 180);
                    break;
            }
        }

        /// <summary>
        /// creates a negative instance of this angle
        /// </summary>
        /// <returns>a negative instance of this angle</returns>
        public Angle Negate()
        {
            return new Angle(AngleType.Radian, this.Radians * -1);
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Angle()
        {
            _intrinsicValue = 0;
        }

        /// <summary>
        /// Create an angle object from an angle value.
        /// </summary>
        /// <param name="angleType">angle unit type</param>
        /// <param name="passedValue">angle value</param>
        public Angle(AngleType angleType, double passedValue)
        {
            switch (angleType)
            {
                case AngleType.Radian:
                    _intrinsicValue = passedValue;
                    break;
                case AngleType.Degree:
                    _intrinsicValue = passedValue * (Math.PI / 180);
                    break;
            }

        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="passedAngle">angle to copy</param>
        public Angle(Angle passedAngle)
        {
            this._intrinsicValue = passedAngle._intrinsicValue;
        }

        /// <summary>
        /// castes an angle string to angle degrees
        /// </summary>
        /// <param name="passedAngleString">angle string to parse</param>
        public Angle( string passedAngleString)
        {
            string[] seperatedStrings = passedAngleString.Split(new char[] { '°', '\'' });
            int degrees;
            int minutes;
            int seconds;

            try
            {
                degrees = int.Parse(seperatedStrings[0]);
                minutes = int.Parse(seperatedStrings[1]);
                seconds = int.Parse(seperatedStrings[2]);
            }
            catch (Exception)
            {
                throw new Exception();
            }

            _intrinsicValue = degrees + (minutes / 60f) + (seconds / 3600f);
        }

        #endregion

        #region Overloaded Operators

        /* You may notice that we do not overload the increment and decrement operators nor do we overload multiplication and division.
         * This is because the user of this library does not know what is being internally stored and those operations will not return useful information. 
         */

        /// <summary>
        /// adds the two angles together
        /// </summary>
        /// <param name="d1">first angle</param>
        /// <param name="d2">second angle</param>
        /// <returns>the sum of the two angles</returns>
        public static Angle operator +(Angle d1, Angle d2)
        {
            //add the two Angles together
            //return a new Angle with the new value
            return new Angle(InternalUnitType, (d1._intrinsicValue + d2._intrinsicValue));
        }

        /// <summary>
        /// subtracts one angle from the other
        /// </summary>
        /// <param name="d1">the angle to be subtracted from</param>
        /// <param name="d2">the angle to subtract</param>
        /// <returns>the result of the first angle minus the second</returns>
        public static Angle operator -(Angle d1, Angle d2)
        {
            //subtract the two Angles
            //return a new Angle with the new value
            return new Angle(InternalUnitType, (d1._intrinsicValue - d2._intrinsicValue));
        }

        /// <summary>
        /// value equals to check if the two angles are equal
        /// </summary>
        /// <param name="d1">first angle to check for equality</param>
        /// <param name="d2">second angle to check for equality</param>
        /// <returns>whether the two angles are equal</returns>
        public static bool operator ==(Angle d1, Angle d2)
        {
            return d1.Equals(d2);
        }

        /// <summary>
        /// value equals to check if the two angles are not equal
        /// </summary>
        /// <param name="d1">first angle to check for inequality</param>
        /// <param name="d2">second angle to check for inequality</param>
        /// <returns>whether the two angles aren't equal</returns>
        public static bool operator !=(Angle d1, Angle d2)
        {
            return !d1.Equals(d2);
        }

        /// <summary>
        /// checks whether one angle is larger than the other
        /// </summary>
        /// <param name="d1">angle that is supposed to be larger</param>
        /// <param name="d2">angle that is supposed to be smaller</param>
        /// <returns>whether the first angle is greater than the second</returns>
        public static bool operator >(Angle d1, Angle d2)
        {
            return d1._intrinsicValue > d2._intrinsicValue;
        }

        /// <summary>
        /// checks whether one angle is smaller than the other
        /// </summary>
        /// <param name="d1">angle that is supposed to be smaller</param>
        /// <param name="d2">angle that is supposed to be larger</param>
        /// <returns>whether the first angle is less than the second</returns>
        public static bool operator <(Angle d1, Angle d2)
        {
            return d1._intrinsicValue < d2._intrinsicValue;
        }

        /// <summary>
        /// checks whether an angle is larger than or equal to the other
        /// </summary>
        /// <param name="d1">supposed to be larger or equal angle</param>
        /// <param name="d2">supposed to be smaller or equal angle</param>
        /// <returns>whether the angle on the left is greater than or equal to the angle on the right</returns>
        public static bool operator >=(Angle d1, Angle d2)
        {
            return d1.Equals(d2) || d1._intrinsicValue > d2._intrinsicValue;
        }

        /// <summary>
        /// checks whether an angle is less than or equal to the other
        /// </summary>
        /// <param name="d1">supposed to be smaller or equal angle</param>
        /// <param name="d2">supposed to be larger or equal angle</param>
        /// <returns>whether the angle on the left is less than or equal to the angle on the right</returns>
        public static bool operator <=(Angle d1, Angle d2)
        {
            return d1.Equals(d2) || d1._intrinsicValue < d2._intrinsicValue;
        }

        /// <summary>
        /// This override determines how this object is inserted into hashtables.
        /// </summary>
        /// <returns>same hashcode as any double would</returns>
        public override int GetHashCode()
        {
            return _intrinsicValue.GetHashCode();
        }

        /// <summary>
        /// Makes sure to throw an error telling the user that this is a bad idea
        /// The Angle class does not know what type of unit it contains, 
        /// (Because it should be thought of containing all unit types) 
        /// Call Angle.Tostring(AngleType) instead
        /// </summary>
        /// <returns>Should never return anything</returns>
        public override string ToString()
        {
            throw new NotImplementedException("The Angle class does not know what type of unit it contains, (Because it should be thought of as containing all unit types) Call Angle.[unit].ToString() instead");
        }

        /// <summary>
        /// value equality override
        /// </summary>
        /// <param name="obj">object to check equality against</param>
        /// <returns>a boolean representing the equality of the two angles</returns>
        public override bool Equals(object obj)
        {
            return Math.Abs(_intrinsicValue - ((Angle)(obj))._intrinsicValue) < Constants.AcceptedEqualityDeviationConstant;
        }

        /// <summary>
        /// Returns the a string converted to a desired unitType
        /// </summary>
        /// <param name="angleType">unit that you want the desired output in</param>
        /// <returns>string representation of angle in unit of choice</returns>
        public string ToString(AngleType angleType)
        {
            switch (angleType)
            {
                case AngleType.Radian:
                    return _intrinsicValue.ToString() + " rad";
                case AngleType.Degree:
                    return ConvertDecimalRadiansToDegreesString(_intrinsicValue) + "°";
                default:
                    //code should never be run
                    return "We were unable to identify your desired Unit Type";
            }
        }

        #endregion

        #region Interface Implementations

        /// <summary>
        /// This implements the IComparable interface and allows Angles to be sorted and such
        /// </summary>
        /// <param name="other">angle to compare against</param>
        /// <returns>1 if this > other; 0 if this == other; -1 if this < other</returns>
        public int CompareTo(Angle other)
        {
            if (this.Equals(other))
                return 0;
            else
                return (this._intrinsicValue.CompareTo(other._intrinsicValue));
        }
        #endregion
    }
}
