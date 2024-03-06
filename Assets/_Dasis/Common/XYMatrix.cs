using System.Collections.Generic;
using UnityEngine;

namespace Dasis.Common
{
    public class XYMatrix
    {
        public enum Degree
        {
            _N45 = -1,
            _N90 = -2,
            _P45 = +1,
            _P90 = +2,
            _180 = 4,
        }

        public static Vector2Int Up = new Vector2Int(0, -1);
        public static Vector2Int UpRight = new Vector2Int(1, -1);
        public static Vector2Int Right = new Vector2Int(1, 0);
        public static Vector2Int DownRight = new Vector2Int(1, 1);
        public static Vector2Int Down = new Vector2Int(0, 1);
        public static Vector2Int DownLeft = new Vector2Int(-1, 1);
        public static Vector2Int Left = new Vector2Int(-1, 0);
        public static Vector2Int UpLeft = new Vector2Int(-1, -1);

        public static List<Vector2Int> Directions = new List<Vector2Int>
        {
            Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft,
        };

        public static Vector2Int Rotate(Vector2Int direction, Degree degree)
        {
            int dirIndex = Directions.FindIndex(dir => dir == direction);
            if (dirIndex == -1)
            {
                Debug.Log("Invalid XYMatrix Direction");
                return Vector2Int.zero;
            }
            int rotatedDirIndex = (Directions.Count + dirIndex + (int)degree) % Directions.Count;
            return Directions[rotatedDirIndex];
        }
    }
}
