using System;
using UnityEngine;

namespace Eflatun.HexMath
{
    public class HexCoordinateSystemInt
    {
        private readonly float _size;

        public HexCoordinateSystemInt(float size)
        {
            _size = size;
        }

        public Vector2 CubeToUnity(CubeCoordinates cube)
        {
            return cube.ToUnity(_size);
        }

        public CubeCoordinates CubeFromUnity(Vector2 point, RoundingMethod roundingMethod)
        {
            return CubeCoordinates.FromUnity(point, _size, roundingMethod);
        }

        public Vector2 OffsetToUnity(OffsetCoordinates offset)
        {
            return offset.ToUnity(_size);
        }

        public OffsetCoordinates OffsetFromUnity(Vector2 point, RoundingMethod roundingMethod)
        {
            return OffsetCoordinates.FromUnity(point, _size, roundingMethod);
        }
    }
}
