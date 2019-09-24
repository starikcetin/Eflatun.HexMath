using System;
using UnityEngine;

namespace Eflatun.HexMath
{
    /// <summary>
    /// Representation of hexagons on the hexagonal odd-q offset coordinate system.
    /// Odd-Q: Odd columns are offset.
    /// Flat-Top.
    /// </summary>
    public struct OffsetCoordinates : IEquatable<OffsetCoordinates>
    {
        public int Row { get; }
        public int Col { get; }

        public OffsetCoordinates(int col, int row)
        {
            Row = row;
            Col = col;
        }

        public OffsetCoordinates WithRow(int newRow)
        {
            return new OffsetCoordinates(Col, newRow);
        }

        public OffsetCoordinates WithCol(int newCol)
        {
            return new OffsetCoordinates(newCol, Row);
        }

        public CubeCoordinates ToCube()
        {
            var q = Col;
            var r = Row - (Col + (Col & 1)) / 2;
            var s = -q - r;
            return new CubeCoordinates(q, s, r);
        }

        public Vector2 ToUnity(float size)
        {
            return ToCube().ToUnity(size);
        }

        public bool Equals(OffsetCoordinates other)
        {
            return Row == other.Row && Col == other.Col;
        }

        public override bool Equals(object obj)
        {
            return obj is OffsetCoordinates other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row * 397) ^ Col;
            }
        }

        public static bool operator ==(OffsetCoordinates left, OffsetCoordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OffsetCoordinates left, OffsetCoordinates right)
        {
            return !left.Equals(right);
        }
    }
}
