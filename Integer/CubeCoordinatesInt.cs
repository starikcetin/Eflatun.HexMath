using System;
using System.Diagnostics.Contracts;
using Eflatun.HexMath.Float;
using UnityEngine;

namespace Eflatun.HexMath.Integer
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
    public struct CubeCoordinatesInt : IEquatable<CubeCoordinatesInt>, IEquatable<CubeCoordinatesFloat>
    {
        public readonly int Q;
        public readonly int S;
        public readonly int R;

        public CubeCoordinatesInt(int q, int s, int r)
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
        public CubeCoordinatesInt(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        public CubeCoordinatesInt(CubeCoordinatesInt other)
        {
            this = other;
        }

        public CubeCoordinatesInt(CubeCoordinatesFloat cubeCoordinatesFloat, RoundingMethod roundingMethod)
        {
            int qInt;
            int rInt;

            switch (roundingMethod)
            {
                case RoundingMethod.Ceil:
                    qInt = Mathf.CeilToInt(cubeCoordinatesFloat.Q);
                    rInt = Mathf.CeilToInt(cubeCoordinatesFloat.R);
                    break;
                case RoundingMethod.Floor:
                    qInt = Mathf.FloorToInt(cubeCoordinatesFloat.Q);
                    rInt = Mathf.FloorToInt(cubeCoordinatesFloat.R);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roundingMethod), roundingMethod, null);
            }

            Q = qInt;
            R = rInt;
            S = -qInt - rInt;
        }

        public CubeCoordinatesInt WithQ(int newQ)
        {
            return new CubeCoordinatesInt(newQ, S, R);
        }

        public CubeCoordinatesInt WithS(int newS)
        {
            return new CubeCoordinatesInt(Q, newS, R);
        }

        public CubeCoordinatesInt WithR(int newR)
        {
            return new CubeCoordinatesInt(Q, S, newR);
        }

        public OffsetCoordinatesInt ToOffset()
        {
            var col = Q;
            var row = R + (Q - (Q & 1)) / 2;
            return new OffsetCoordinatesInt(col, row);
        }

        public Vector2 ToUnity(float size)
        {
            var x = size * (3f / 2 * Q);
            var y = size * (Mathf.Sqrt(3) / 2 * Q + Mathf.Sqrt(3) * R);
            return new Vector2(x, y);
        }

        public static CubeCoordinatesInt FromUnity(Vector2 point, float size, RoundingMethod roundingMethod)
        {
            var q = (2f / 3 * point.x) / size;
            var r = (-1f / 3 * point.x + Mathf.Sqrt(3) / 3 * point.y) / size;

            int qInt;
            int rInt;

            switch (roundingMethod)
            {
                case RoundingMethod.Ceil:
                    qInt = Mathf.CeilToInt(q);
                    rInt = Mathf.CeilToInt(r);
                    break;
                case RoundingMethod.Floor:
                    qInt = Mathf.FloorToInt(q);
                    rInt = Mathf.FloorToInt(r);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(roundingMethod), roundingMethod, null);
            }

            return new CubeCoordinatesInt(qInt, rInt);
        }

        [ContractInvariantMethod]
        private void _ContractInvariant()
        {
            Contract.Assert(Q + S + R == 0);
        }

        public bool Equals(CubeCoordinatesInt other)
        {
            return Q == other.Q && S == other.S && R == other.R;
        }

        public override bool Equals(object obj)
        {
            return obj is CubeCoordinatesInt otherInt && Equals(otherInt)
                || obj is CubeCoordinatesFloat otherFloat && Equals(otherFloat);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Q;
                hashCode = (hashCode * 397) ^ S;
                hashCode = (hashCode * 397) ^ R;
                return hashCode;
            }
        }

        public static bool operator ==(CubeCoordinatesInt left, CubeCoordinatesInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CubeCoordinatesInt left, CubeCoordinatesInt right)
        {
            return !left.Equals(right);
        }

        public bool Equals(CubeCoordinatesFloat other)
        {
            return Mathf.Approximately(Q, other.Q)
                   && Mathf.Approximately(S, other.S)
                   && Mathf.Approximately(R, other.R);
        }

        public static bool operator ==(CubeCoordinatesInt left, CubeCoordinatesFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CubeCoordinatesInt left, CubeCoordinatesFloat right)
        {
            return !left.Equals(right);
        }

        public CubeCoordinatesFloat ToFloat()
        {
            return new CubeCoordinatesFloat(this);
        }

        public static implicit operator CubeCoordinatesFloat(CubeCoordinatesInt cubeCoordinatesInt)
        {
            return new CubeCoordinatesFloat(cubeCoordinatesInt);
        }

        public static implicit operator OffsetCoordinatesInt(CubeCoordinatesInt cubeCoordinatesInt)
        {
            return cubeCoordinatesInt.ToOffset();
        }
    }
}
