using UnityEngine;
using Random = System.Random;

namespace JescoDev.SmoothBrainStates.Tags.Editor {
    public static class TagColor {

        public static Color GetColor(string tag) {
            Random random = new Random(tag.GetHashCode());
            return new Color(
                (float) random.NextDouble(),
                (float) random.NextDouble(),
                (float) random.NextDouble()
            );
        }
        
        public static Color GetTextColor(Color tagColor) {
            return tagColor.maxColorComponent > 0.5f ? Color.black : Color.white;
        }
        
    }
}