using System;
using System.Diagnostics.Contracts;
using Eflatun.HexMath.Integer;
using UnityEngine;

namespace Eflatun.HexMath.Float
{
    /// <summary>
    /// Representation of hexagons on the hexagonal cube coordinate system.
    /// Values are integers.
    /// Always: (Q + S + R = 0).
    /// Flat-Top.
    /// </summary>
    /// <remarks>
    /// Q == X
    /// S == Y
    /// R == Z
    /// These are not regular XYZ, these are the XYZ in hexagonal cube coordinate system.
    /// That is why using QSR is a better idea in order to distinguish them.
    /// </remarks>
    public struct CubeCoordinatesFloat : IEquatable<CubeCoordinatesFloat>, IEquatable<CubeCoordinatesInt>
    {
        public readonly float Q;
        public readonly float S;
        public readonly float R;

        public CubeCoordinatesFloat(float q, float s, float r)
        {
            Q = q;
            S = s;
            R = r;
        }

        /// <summary>
        /// Constructor for hexagonal axial coordinate system support.
        /// </summary>
        /// <remarks>
        /// Since Q + R + S = 0, one of them is redundant. Here, we dropped S and derived it with (-Q-R).
        /// </remarks>
        public CubeCoordinatesFloat(float q, float r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        public CubeCoordinatesFloat(CubeCoordinatesFloat other)
        {
            this = other;
        }

        public CubeCoordinatesFloat(CubeCoordinatesInt cubeCoordinatesInt)
        {
            Q = cubeCoordinatesInt.Q;
            R = cubeCoordinatesInt.R;
            S = cubeCoordinatesInt.S;
        }

        public CubeCoordinatesFloat WithQ(float newQ)
        {
            return new CubeCoordinatesFloat(newQ, S, R);
        }

        public CubeCoordinatesFloat WithS(float newS)
        {
            return new CubeCoordinatesFloat(Q, newS, R);
        }

        public CubeCoordinatesFloat WithR(float newR)
        {
            return new CubeCoordinatesFloat(Q, S, newR);
        }

        public OffsetCoordinatesFloat ToOffset()
        {
            var col = Q;
            var row = R + (Q - ((Q % 2 + 2) % 2)) / 2;
            return new OffsetCoordinatesFloat(col, row);
        }

        public Vector2 ToUnity(float size)
        {
            var x = size * (3f / 2 * Q);
            var y = size * (Mathf.Sqrt(3) / 2 * Q + Mathf.Sqrt(3) * R);
            return new Vector2(x, y);
        }

        public static CubeCoordinatesFloat FromUnity(Vector2 point, float size)
        {
            var q = (2f / 3 * point.x) / size;
            var r = (-1f / 3 * point.x + Mathf.Sqrt(3) / 3 * point.y) / size;

            return new CubeCoordinatesFloat(q, r);
        }

        [ContractInvariantMethod]
        private void _ContractInvariant()
        {
            Contract.Assert(Math.Abs(Q + S + R) <= float.Epsilon);
        }

        public bool Equals(CubeCoordinatesFloat other)
        {
            return Math.Abs(Q - other.Q) <= float.Epsilon && Math.Abs(S - other.S) <= float.Epsilon && Math.Abs(R - other.R) <= float.Epsilon;
        }

        public override bool Equals(object obj)
        {
            return obj is CubeCoordinatesFloat otherFloat && Equals(otherFloat)
                   || obj is CubeCoordinatesInt otherInt && Equals(otherInt);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Q.GetHashCode();
                hashCode = (hashCode * 397) ^ S.GetHashCode();
                hashCode = (hashCode * 397) ^ R.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(CubeCoordinatesFloat left, CubeCoordinatesFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CubeCoordinatesFloat left, CubeCoordinatesFloat right)
        {
            return !left.Equals(right);
        }
        
        public bool Equals(CubeCoordinatesInt other)
        {
            return Math.Abs(Q - other.Q) <= float.Epsilon && Math.Abs(S - other.S) <= float.Epsilon && Math.Abs(R - other.R) <= float.Epsilon;
        }

        public static bool operator ==(CubeCoordinatesFloat left, CubeCoordinatesInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CubeCoordinatesFloat left, CubeCoordinatesInt right)
        {
            return !left.Equals(right);
        }

        public CubeCoordinatesInt ToInt(RoundingMethod roundingMethod)
        {
            return new CubeCoordinatesInt(this, roundingMethod);
        }
    }
}
