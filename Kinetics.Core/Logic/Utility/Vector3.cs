using System;

namespace Kinetics.Core.Logic.Utility
{
    internal class Vector3
    {
        public Vector3(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
            RecalculateMagnitude();
        }

        private double _x;
        private double _y;
        private double _z;

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                RecalculateMagnitude();
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                RecalculateMagnitude();
            }
        }

        public double Z
        {
            get { return _z; }
            set
            {
                _z = value;
                RecalculateMagnitude();
            }
        }

        public double Magnitude { get; private set; }

        #region Math operators

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Z * b.Y - a.Y * b.Z, a.X * b.Z - a.Z * b.X, a.Y * b.X - a.X * b.Y);
        }

        public static double Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public double Dot(Vector3 other)
        {
            return Dot(this, other);
        }

        public void Normalize()
        {
            _x = _x / Magnitude;
            _y = _y / Magnitude;
            _z = _z / Magnitude;
            RecalculateMagnitude();
        }

        public Vector3 Clone()
        {
            return new Vector3(X, Y, Z);
        }

        #endregion

        #region Specific instances

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);

        public static readonly Vector3 Up = new Vector3(0, 0, 1);

        public static readonly Vector3 Right = new Vector3(1, 0, 0);

        public static readonly Vector3 Forward = new Vector3(0, 1, 0);

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Vector3)obj);
        }

        protected bool Equals(Vector3 other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("V3({0}, {1}, {2})", X.ToString("N"), Y.ToString("N"), Z.ToString("N"));
        }

        private void RecalculateMagnitude()
        {
            Magnitude = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }
    }
}
