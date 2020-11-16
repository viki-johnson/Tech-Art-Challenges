using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class paintbrush : MonoBehaviour
{
    public int resolution = 512;
    Texture2D whiteMap;
    public float brushSize;
    public Texture2D[] brushTexture;
    public Texture2D emptyTexture;
    public Color fade;
    public float fadeSpeed;
    Vector2 stored;
    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    public RenderTexture paintText;
    void Start()
    {
        // paintText = new RenderTexture(resolution, resolution, 32);
        CreateClearTexture();// clear black texture to draw on
        StartCoroutine(FadeTrail(paintText));
    }
 
    void Update()
    {
 
        // Debug.DrawRay(transform.position, Vector3.down * 20f, Color.magenta);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Collider coll = hit.collider;
            Debug.Log(coll.name);
            if (coll != null)
            {
                // if (!paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that
                if(!paintText)
                {
                    Renderer rend = hit.transform.GetComponent<Renderer>();


                    // paintTextures.Add(coll, getWhiteRT());
                    // paintText = new RenderTexture(resolution, resolution, 32);
                    Graphics.Blit(whiteMap, paintText);

                    // rend.material.SetTexture("_PaintMap", paintTextures[coll]);
                    rend.material.SetTexture("_PaintMap", paintText);
                }
                if (stored != hit.lightmapCoord) // stop drawing on the same point
                {
                    stored = hit.lightmapCoord;
                    Vector2 pixelUV = hit.lightmapCoord;
                    pixelUV.y *= resolution;
                    pixelUV.x *= resolution;
                    // DrawTexture(paintTextures[coll], pixelUV.x, pixelUV.y);
                    DrawTexture(paintText, pixelUV.x, pixelUV.y);
                }
            }
        }
    }
 
    void DrawTexture(RenderTexture rt, float posX, float posY)
    {
        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size
 
        // draw brushtexture

        Texture2D bt = brushTexture[Random.Range(0, brushTexture.Length)];
        // Graphics.DrawTexture(new Rect(0,0,1000,1000), emptyTexture, new Rect(0, 0, bt.width, bt.height), 0, 0, 0, 0, fade, null);
        Graphics.DrawTexture(new Rect(posX - bt.width / brushSize, (rt.height - posY) - bt.height / brushSize, bt.width / (brushSize * 0.5f), bt.height / (brushSize * 0.5f)), bt);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture
    }
 
    RenderTexture getWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }
 
    void CreateClearTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.black);
        whiteMap.Apply();
    }

    IEnumerator FadeTrail(RenderTexture rt)
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            RenderTexture.active = rt; // activate rendertexture for drawtexture;
            GL.PushMatrix();                       // save matrixes
            GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size
 
            Graphics.DrawTexture(new Rect(0,0,1000,1000), emptyTexture, new Rect(0, 0, 10, 10), 0, 0, 0, 0, fade, null);


            // Texture2D bt = brushTexture[Random.Range(0, brushTexture.Length)];
            // Graphics.DrawTexture(new Rect(5f - bt.width / brushSize, (rt.height - 5f) - bt.height / brushSize, bt.width / (brushSize * 0.5f), bt.height / (brushSize * 0.5f)), bt);

            GL.PopMatrix();
            RenderTexture.active = null;
            Debug.Log("did a fade");
            yield return new WaitForSeconds(fadeSpeed);
        }
    }

}