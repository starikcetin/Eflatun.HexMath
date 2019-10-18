using System;
using UnityEngine;

namespace Eflatun.HexMath.Float
{
    /// <summary>
    /// Representation of hexagons on the hexagonal odd-q offset coordinate system.
    /// Odd-Q: Odd columns are offset.
    /// Flat-Top.
    /// </summary>
    public struct OffsetCoordinatesFloat : IEquatable<OffsetCoordinatesFloat>
    {
        public float Row { get; }
        public float Col { get; }

        public OffsetCoordinatesFloat(float col, float row)
        {
            Row = row;
            Col = col;
        }

        public OffsetCoordinatesFloat WithRow(float newRow)
        {
            return new OffsetCoordinatesFloat(Col, newRow);
        }

        public OffsetCoordinatesFloat WithCol(float newCol)
        {
            return new OffsetCoordinatesFloat(newCol, Row);
        }

        public CubeCoordinatesFloat ToCube()
        {
            var q = Col;
            var r = Row - (Col + ((Col % 2 + 2) % 2)) / 2;
            var s = -q - r;
            return new CubeCoordinatesFloat(q, s, r);
        }

        public Vector2 ToUnity(float size)
        {
            return ToCube().ToUnity(size);
        }

        public static OffsetCoordinatesFloat FromUnity(Vector2 point, float size)
        {
            return CubeCoordinatesFloat.FromUnity(point, size).ToOffset();
        }

        public bool Equals(OffsetCoordinatesFloat other)
        {
            return Math.Abs(Row - other.Row) <= float.Epsilon && Math.Abs(Col - other.Col) <= float.Epsilon;
        }

        public override bool Equals(object obj)
        {
            return obj is OffsetCoordinatesFloat other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row.GetHashCode() * 397) ^ Col.GetHashCode();
            }
        }

        public static bool operator ==(OffsetCoordinatesFloat left, OffsetCoordinatesFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OffsetCoordinatesFloat left, OffsetCoordinatesFloat right)
        {
            return !left.Equals(right);
        }
    }
}
