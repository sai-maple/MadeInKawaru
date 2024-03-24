using UnityEngine;

namespace MadeInKawaru.Extensions
{
    public static class RectTransformExtensions
    {
        /// <summary>他のRectTransformと重なっているかどうかを判定します。</summary>
        /// <param name="self">自身のRectTransformです。</param>
        /// <param name="other">他のRectTransformです。</param>
        /// <returns>重なっている場合にtrueを返します。</returns>
        public static bool Overlaps(this RectTransform self, RectTransform other) =>
            self.WorldRect().Overlaps(other.WorldRect());

        /// <summary>ワールド座標系でのRectを取得します。</summary>
        /// <param name="self">自身のRectTransformです。</param>
        /// <returns>ワールド座標系でのRectを返します。</returns>
        private static Rect WorldRect(this RectTransform self)
        {
            var corners = new Vector3[4];
            self.GetWorldCorners(corners);

            var sizeDelta = self.sizeDelta;
            var lossyScale = self.lossyScale;

            var width = sizeDelta.x * lossyScale.x;
            var height = sizeDelta.y * lossyScale.y;

            return new Rect(corners[0], new Vector2(width, height));
        }
    }
}