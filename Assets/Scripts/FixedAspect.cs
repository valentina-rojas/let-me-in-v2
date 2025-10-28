using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspect : MonoBehaviour
{
    public float targetAspectWidth = 16f;
    public float targetAspectHeight = 9f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        SetAspect();
    }

    void OnPreCull()
    {
        SetAspect();
        GL.Clear(true, true, Color.black); // 🔹 Limpia toda la pantalla
    }

    void SetAspect()
    {
        float targetAspect = targetAspectWidth / targetAspectHeight;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
            cam.rect = rect;
        }
    }
}
