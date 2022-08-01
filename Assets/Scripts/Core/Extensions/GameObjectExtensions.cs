using UnityEngine;

namespace SquareDinoTestTask.Core.Extensions {
    public static class GameObjectExtensions {
        public static bool IsInLayer(this GameObject go, LayerMask layer)
            => layer == (layer | 1 << go.layer);
    }
}
