using UnityEngine.SceneManagement;

namespace Source
{
    public class SceneChangerService
    {
        private const string Workspace = "Workspace";
        private const string StartScreen = "StartScreen";

        public void SwitchToWorkspace()
        {
            LoadScene(Workspace);
        }
        
        public void SwitchToStartScreen()
        {
            LoadScene(StartScreen);
        }

        private void LoadScene(string sceneName)
        {
            if (SceneManager.GetActiveScene().name != sceneName)
                SceneManager.LoadSceneAsync(sceneName);
        }
    }
}