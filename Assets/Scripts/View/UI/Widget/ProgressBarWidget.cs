using UnityEngine;
using UnityEngine.UI;

namespace SquareDinoTestTask.View.UI.Widget {
    public class ProgressBarWidget : MonoBehaviour {
        [SerializeField] private Image bar;

        public void SetProgress(float progress)
            => bar.fillAmount = progress;
    }
}
