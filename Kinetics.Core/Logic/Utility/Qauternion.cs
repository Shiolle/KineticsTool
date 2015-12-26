using System;

namespace Kinetics.Core.Logic.Utility
{
    internal class Qauternion
    {
        private double _w;
        private double _x;
        private double _y;
        private double _z;

        public Qauternion(double w, double x, double y, double z)
        {
            _w = w;
            _x = x;
            _y = y;
            _z = z;
            RecalcualteVectorAngle();
            RecalculateMagnitude();
        }

        public Qauternion(Vector3 vector, double angle)
        {
            _w = Math.Cos(angle / 2d);
            double coef = Math.Sin(angle / 2d);
            _x = vector.X * coef;
            _y = vector.Y * coef;
            _z = vector.Z * coef;
            Axis = vector;
            Angle = angle;
            RecalculateMagnitude();
        }

        public double W
        {
            get { return _w; }
            set
            {
                _w = value;
                RecalcualteVectorAngle();
                RecalculateMagnitude();
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                RecalcualteVectorAngle();
                RecalculateMagnitude();
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                RecalcualteVectorAngle();
                RecalculateMagnitude();
            }
        }

        public double Z
        {
            get { return _z; }
            set
            {
                _z = value;
                RecalcualteVectorAngle();
                RecalculateMagnitude();
            }
        }

        public double Magnitude { get; private set; }

        public Vector3 Axis { get; private set; }

        public double Angle { get; private set; }

        #region Math operations

        public static Qauternion operator +(Qauternion a, Qauternion b)
        {
            return new Qauternion(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Qauternion operator -(Qauternion a, Qauternion b)
        {
            return new Qauternion(a.W - b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Qauternion operator *(Qauternion a, double b)
        {
            return new Qauternion(a.W * b, a.X * b, a.Y * b, a.Z * b);
        }

        public static Qauternion operator *(Qauternion a, Qauternion b)
        {
            //return new Qauternion(
            //    a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            //    a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            //    a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            //    a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W);
            return new Qauternion(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W - a.Y * b.Z + a.Z * b.Y,
                a.W * b.Y + a.X * b.Z + a.Y * b.W - a.Z * b.X,
                a.W * b.Z - a.X * b.Y + a.Y * b.X + a.Z * b.W);
        }

        public void Normalize()
        {
            _w /= Magnitude;
            _x /= Magnitude;
            _y /= Magnitude;
            _z /= Magnitude;
            RecalcualteVectorAngle();
            RecalculateMagnitude();
        }

        #endregion

        #region Special cases

        public static readonly Qauternion Identity = new Qauternion(1, 0, 0, 0);

        public Qauternion GetConjugate()
        {
            var result = new Qauternion(W, -X, -Y, -Z);
            result.Normalize();
            return result;
        }

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
            return Equals((Qauternion)obj);
        }

        protected bool Equals(Qauternion other)
        {
            return _w.Equals(other._w) && _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
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
                hashCode = (hashCode * 397) ^ W.GetHashCode();
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
            return string.Format("Q({0}, {1})", Axis, Angle.ToString("N"));
        }

        private void RecalcualteVectorAngle()
        {
            Angle = 2d * Math.Acos(W);
            double coef = Math.Sin(Angle / 2d);
            Axis = new Vector3(X / coef, Y / coef, Z / coef);
        }

        private void RecalculateMagnitude()
        {
            Magnitude = Math.Sqrt(Math.Pow(W, 2) + Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }
    }
}
