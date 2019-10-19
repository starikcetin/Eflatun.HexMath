using System;
using Eflatun.HexMath.Float;
using UnityEngine;

namespace Eflatun.HexMath.Integer
{
    /// <summary>
    /// Representation of hexagons on the hexagonal odd-q offset coordinate system.
    /// Odd-Q: Odd columns are offset.
    /// Flat-Top.
    /// </summary>
    public struct OffsetCoordinatesInt : IEquatable<OffsetCoordinatesInt>, IEquatable<OffsetCoordinatesFloat>
    {
        public int Row { get; }
        public int Col { get; }

        public OffsetCoordinatesInt(int col, int row)
        {
            Row = row;
            Col = col;
        }

        public OffsetCoordinatesInt(OffsetCoordinatesInt other)
        {
            this = other;
        }

        public OffsetCoordinatesInt(OffsetCoordinatesFloat offsetCoordinatesFloat, RoundingMethod roundingMethod)
        {
            this = offsetCoordinatesFloat.ToCube().ToInt(roundingMethod).ToOffset();
        }

        public OffsetCoordinatesInt WithRow(int newRow)
        {
            return new OffsetCoordinatesInt(Col, newRow);
        }

        public OffsetCoordinatesInt WithCol(int newCol)
        {
            return new OffsetCoordinatesInt(newCol, Row);
        }

        public CubeCoordinatesInt ToCube()
        {
            var q = Col;
            var r = Row - (Col + (Col & 1)) / 2;
            var s = -q - r;
            return new CubeCoordinatesInt(q, s, r);
        }

        public Vector2 ToUnity(float size)
        {
            return ToCube().ToUnity(size);
        }

        public static OffsetCoordinatesInt FromUnity(Vector2 point, float size, RoundingMethod roundingMethod)
        {
            return CubeCoordinatesInt.FromUnity(point, size, roundingMethod).ToOffset();
        }

        public bool Equals(OffsetCoordinatesInt other)
        {
            return Row == other.Row && Col == other.Col;
        }

        public override bool Equals(object obj)
        {
            return obj is OffsetCoordinatesInt otherInt && Equals(otherInt)
                || obj is OffsetCoordinatesFloat otherFloat && Equals(otherFloat);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row * 397) ^ Col;
            }
        }

        public static bool operator ==(OffsetCoordinatesInt left, OffsetCoordinatesInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OffsetCoordinatesInt left, OffsetCoordinatesInt right)
        {
            return !left.Equals(right);
        }

        public bool Equals(OffsetCoordinatesFloat other)
        {
            return Mathf.Approximately(Row, other.Row)
                   && Mathf.Approximately(Col, other.Col);
        }

        public static bool operator ==(OffsetCoordinatesInt left, OffsetCoordinatesFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OffsetCoordinatesInt left, OffsetCoordinatesFloat right)
        {
            return !left.Equals(right);
        }

        public OffsetCoordinatesFloat ToFloat()
        {
            return new OffsetCoordinatesFloat(this);
        }

        public static implicit operator OffsetCoordinatesFloat(OffsetCoordinatesInt offsetCoordinatesInt)
        {
            return new OffsetCoordinatesFloat(offsetCoordinatesInt);
        }

        public static implicit operator CubeCoordinatesInt(OffsetCoordinatesInt offsetCoordinatesInt)
        {
            return offsetCoordinatesInt.ToCube();
        }
    }
}
