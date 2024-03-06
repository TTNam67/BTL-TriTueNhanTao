using Dasis.Extensions;
using UnityEngine;

namespace Dasis.Common
{
    public class Grider
    {
        public enum InitType
        {
            FromWorldSpaceSize,
            FromCellSize,
        }

        public Vector2 TopLeftCellPosition => topLeftCellPosition;
        public Vector2 TopLeftCornerPosition => topLeftCornerPosition;
        public Vector2 CenterPosition => centerPosition;
        public Vector2 GapSize => gapSize;
        public Vector2Int GridSize => gridSize;

        public Vector2 CellSize
        {
            get { return cellSize; }
            set { cellSize = value; }
        }

        public Vector2 WorldSpaceSize
        {
            get { return worldSpaceSize; }
            set { worldSpaceSize = value; }
        }

        public Vector2 GapPercent
        {
            get { return gapPercent; }
            set { gapPercent = value; }
        }

        private Vector2 worldSpaceSize;
        private Vector2Int gridSize; // ATTENTION: board [column, row]
        private Vector2 gapPercent;

        private Vector2 topLeftCellPosition = new Vector2();
        private Vector2 topLeftCornerPosition = new Vector2();
        private Vector2 centerPosition;
        private Vector2 rectSize = new Vector2();
        private Vector2 cellSize = new Vector2();
        private Vector2 gapSize = new Vector2();

        public void Initialize(InitType initType, Vector2 centerPos, Vector2Int boardGridSize)
        {
            centerPosition = centerPos;
            gridSize = boardGridSize;
            switch (initType)
            {
                case InitType.FromWorldSpaceSize:
                    CalculateSizesFromWorldSpaceSize();
                    break;
                case InitType.FromCellSize:
                    CalculateSizesFromCellSize();
                    break;
            }
            CalculatePositions();
        }

        public void CalculateSizesFromCellSize()
        {
            gapSize = cellSize * gapPercent;
            rectSize = cellSize * gridSize + gapPercent * (gridSize - Vector2.one);
        }

        public void CalculateSizesFromWorldSpaceSize()
        {
            // l = c*n + g*(n-1) = c*n + (c*k)*(n-1) 
            // => l = c * (n + k*(n-1))
            // => c = l / (n + k*(n-1))

            rectSize.x = worldSpaceSize.x;
            rectSize.y = worldSpaceSize.y;

            if (gridSize.x > gridSize.y)
            {
                rectSize.y = rectSize.x / gridSize.x * gridSize.y;
            }

            if (gridSize.x < gridSize.y)
            {
                rectSize.x = rectSize.y / gridSize.y * gridSize.x;
            }

            cellSize = rectSize / (gridSize + gapPercent * (gridSize - Vector2.one));
            gapSize = cellSize * gapPercent;
        }

        public void CalculatePositions()
        {
            topLeftCornerPosition.x = centerPosition.x - rectSize.x / 2;
            topLeftCornerPosition.y = centerPosition.y + rectSize.y / 2;

            topLeftCellPosition.x = topLeftCornerPosition.x + cellSize.x / 2;
            topLeftCellPosition.y = topLeftCornerPosition.y - cellSize.y / 2;
        }

        public bool IsInBoardGrid(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.x >= gridSize.x) return false;
            if (gridPos.y < 0 || gridPos.y >= gridSize.y) return false;
            return true;
        }

        public static bool IsNeighbor(Vector2Int pos1, Vector2Int pos2)
        {
            if (pos1.x == pos2.x && FastMath.Abs(pos1.y - pos2.y) == 1)
            {
                return true;
            }
            if (pos1.y == pos2.y && FastMath.Abs(pos1.x - pos2.x) == 1)
            {
                return true;
            }
            return false;
        }

        public bool IsInBoardGrid(Vector2 worldSpacePos)
        {
            Vector2Int gridPos = WorldSpaceToGridPosition(worldSpacePos);
            return IsInBoardGrid(gridPos);
        }

        public Vector2 GridToWorldSpacePosition(Vector2Int gridPos)
        {
            float x = topLeftCellPosition.x + gridPos.x * (cellSize.x + gapSize.x);
            float y = topLeftCellPosition.y - gridPos.y * (cellSize.y + gapSize.y);
            return new Vector2(x, y);
        }

        public Vector2Int WorldSpaceToGridPosition(Vector2 worldSpacePos)
        {
            Vector2 distanceToTopLeftCorner = new Vector2
            {
                x = worldSpacePos.x - topLeftCornerPosition.x,
                y = topLeftCornerPosition.y - worldSpacePos.y
            };

            float x = distanceToTopLeftCorner.x % (cellSize.x + gapSize.x);
            float y = distanceToTopLeftCorner.y % (cellSize.y + gapSize.y);

            if (x <= cellSize.x && y <= cellSize.y)
            {
                int i = Mathf.CeilToInt(distanceToTopLeftCorner.x / (cellSize.x + gapSize.x)) - 1;
                int j = Mathf.CeilToInt(distanceToTopLeftCorner.y / (cellSize.y + gapSize.y)) - 1;
                return new Vector2Int(i, j);
            }

            return -Vector2Int.one;
        }
    }
}
