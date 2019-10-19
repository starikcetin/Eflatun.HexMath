using UnityEngine;

namespace Eflatun.HexMath.Integer
{
    public class HexCoordinateSystemInt
    {
        private readonly float _size;

        public HexCoordinateSystemInt(float size)
        {
            _size = size;
        }

        public Vector2 CubeToUnity(CubeCoordinatesInt cube)
        {
            return cube.ToUnity(_size);
        }

        public CubeCoordinatesInt CubeFromUnity(Vector2 point, RoundingMethod roundingMethod)
        {
            return CubeCoordinatesInt.FromUnity(point, _size, roundingMethod);
        }

        public Vector2 OffsetToUnity(OffsetCoordinatesInt offset)
        {
            return offset.ToUnity(_size);
        }

        public OffsetCoordinatesInt OffsetFromUnity(Vector2 point, RoundingMethod roundingMethod)
        {
            return OffsetCoordinatesInt.FromUnity(point, _size, roundingMethod);
        }
    }
}
