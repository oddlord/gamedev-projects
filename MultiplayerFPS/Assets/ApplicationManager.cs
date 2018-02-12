using UnityEngine;

public class ApplicationManager : MonoBehaviour {

    public static ApplicationManager instance;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void ExitGame() {
        Debug.Log("Quitting game.");
        Application.Quit();
    }
}
