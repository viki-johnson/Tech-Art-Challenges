using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomiseTentacles : MonoBehaviour
{

    public MeshRenderer[] tents;
    public Gradient g;
    public Vector2 waveWidth, waveTiling, width, height, alpha, xAxis;

    // Start is called before the first frame update
    void Start()
    {
        foreach(MeshRenderer m in tents)
        {
            m.material.SetColor("_Color3", g.Evaluate(Random.Range(0f,1f)));
            m.material.SetTextureOffset("_NoiseTex", new Vector2(0.5f, Random.Range(0f,1f)));
            m.material.SetFloat("_Width", Random.Range(waveWidth.x, waveWidth.y));
            m.material.SetFloat("_WarpTiling", Random.Range(waveTiling.x, waveTiling.y));
            m.material.SetFloat("_Alpha", Random.Range(alpha.x, alpha.y));

            float x = Random.Range(width.x, width.y);
            float z = Random.Range(height.x, height.y);


            m.gameObject.transform.localScale = new Vector3(x, 1f, z);
            m.gameObject.transform.localPosition = new Vector3(0, -5f*z, Random.Range(xAxis.x, xAxis.y));
        }   
    }
}
