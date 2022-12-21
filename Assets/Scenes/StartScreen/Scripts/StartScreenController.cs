using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceMiner
{
    public class StartScreenController : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(Scenes.LEVEL);
        }
    }
}
