using UnityEngine;
using UnityEngine.UI;

public class WebcamDisplay : MonoBehaviour
{
    public static WebCamTexture camTex;
    public RawImage rawImage;

    void Start()
    {
        if (camTex == null)
        {
            camTex = new WebCamTexture();
            camTex.Play();
        }

        rawImage.texture = camTex;
    }
}
