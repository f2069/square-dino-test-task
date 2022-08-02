using UnityEngine.SceneManagement;

namespace SquareDinoTestTask.Core.Utils {
    public class LevelLoader {
        public void ReloadLevel() {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
