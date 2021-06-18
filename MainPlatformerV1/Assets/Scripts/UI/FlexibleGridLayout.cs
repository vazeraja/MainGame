using UnityEngine;
using UnityEngine.UI;

namespace MainGame {
    public class FlexibleGridLayout : LayoutGroup {

        public enum FitType {
            Uniform,
            Width,
            Height,
        }
        
        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();

            var sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);

            var rect = rectTransform.rect;
            var parentWidth = rect.width;
            var parentHeight = rect.height;

            var cellWidth = parentWidth / (float) columns - ((spacing.x / (float) columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
            var cellHeight = parentHeight / (float) rows - ((spacing.y / (float) rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

            cellSize.x = cellWidth;
            cellSize.y = cellHeight;

            int rowCount = 0;
            int columnCount = 0;

            for (int i = 0; i < rectChildren.Count; i++) {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = cellSize.y * rowCount + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical() { }

        public override void SetLayoutHorizontal() { }

        public override void SetLayoutVertical() { }
    }
}