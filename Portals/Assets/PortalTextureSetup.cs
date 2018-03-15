using UnityEngine;

public class PortalTextureSetup : MonoBehaviour {

    public Camera cameraB;
    public Camera cameraA;

    public Material cameraMatB;
    public Material cameraMatA;

    void Start () {
		if (cameraB.targetTexture != null) {
            cameraB.targetTexture.Release();
        }
        cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatB.mainTexture = cameraB.targetTexture;

        if (cameraA.targetTexture != null) {
            cameraA.targetTexture.Release();
        }
        cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatA.mainTexture = cameraA.targetTexture;
    }
}
