using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code sourced from https://gist.github.com/nothke/e68576aeca5ca6279343f8cd1e0d42ca

public class PixelBoy : MonoBehaviour
{
    public int w = 720;
    int h;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = ((float)Camera.main.pixelHeight / (float)Camera.main.pixelWidth);
        h = Mathf.RoundToInt(w * ratio);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}
