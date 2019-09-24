using System;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace starikcetin.Eflatun.HexMath
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
    public struct CubeCoordinates : IEquatable<CubeCoordinates>
    {
        public readonly int Q;
        public readonly int S;
        public readonly int R;

        public CubeCoordinates(int q, int s, int r)
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
        public CubeCoordinates(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        public CubeCoordinates WithQ(int newQ)
        {
            return new CubeCoordinates(newQ, S, R);
        }

        public CubeCoordinates WithS(int newS)
        {
            return new CubeCoordinates(Q, newS, R);
        }

        public CubeCoordinates WithR(int newR)
        {
            return new CubeCoordinates(Q, S, newR);
        }

        public OffsetCoordinates ToOffset()
        {
            var col = Q;
            var row = R + (Q - (Q & 1)) / 2;
            return new OffsetCoordinates(col, row);
        }

        public Vector2 ToUnity(float size)
        {
            var x = size * (3f / 2 * Q);
            var y = size * (Mathf.Sqrt(3) / 2 * Q + Mathf.Sqrt(3) * R);
            return new Vector2(x, y);
        }

        public static CubeCoordinates FromUnity(Vector2 point, float size, RoundingMethod roundingMethod)
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

            return new CubeCoordinates(qInt, rInt);
        }

        [ContractInvariantMethod]
        private void _ContractInvariant()
        {
            Contract.Assert(Q + S + R == 0);
        }

        public bool Equals(CubeCoordinates other)
        {
            return Q == other.Q && S == other.S && R == other.R;
        }

        public override bool Equals(object obj)
        {
            return obj is CubeCoordinates other && Equals(other);
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

        public static bool operator ==(CubeCoordinates left, CubeCoordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CubeCoordinates left, CubeCoordinates right)
        {
            return !left.Equals(right);
        }
    }
}
