using UnityEngine;

namespace Eflatun.HexMath.Float
{
    public class HexCoordinateSystemFloat
    {
        private readonly float _size;

        public HexCoordinateSystemFloat(float size)
        {
            _size = size;
        }

        public Vector2 CubeToUnity(CubeCoordinatesFloat cube)
        {
            return cube.ToUnity(_size);
        }

        public CubeCoordinatesFloat CubeFromUnity(Vector2 point)
        {
            return CubeCoordinatesFloat.FromUnity(point, _size);
        }

        public Vector2 OffsetToUnity(OffsetCoordinatesFloat offset)
        {
            return offset.ToUnity(_size);
        }

        public OffsetCoordinatesFloat OffsetFromUnity(Vector2 point)
        {
            return OffsetCoordinatesFloat.FromUnity(point, _size);
        }
    }
}
