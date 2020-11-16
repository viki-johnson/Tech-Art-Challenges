﻿using UnityEngine;
using System.Collections;

// based on https://github.com/Broxxar/NoMansScanner
public class ScanScript : MonoBehaviour
{
    public Transform character;
    public Material EffectMaterial;
    public float growSpeed, shrinkSpeed, ScanMax;
    private Camera _camera;
    public int nodeCount = 0;
    public bool growing;
    public Vector4[] posArray = new Vector4[100];
    public float[] distArray = new float[100];

    void OnEnable()
	{
		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.Depth;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            growing = true;
            posArray[nodeCount] = character.position;
            nodeCount ++;
            EffectMaterial.SetFloat("_ArrayLength", nodeCount);

            StartCoroutine(Scan());
        }
    }

    IEnumerator Scan()
    {
        while(distArray[nodeCount-1] < ScanMax)
        {
            distArray[nodeCount-1] += Time.deltaTime * growSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        growing = false;
        while(!growing)
        {
        for(int i = 0; i<nodeCount; i++)
        {
            if(distArray[i] > 0)
            {
                distArray[i] -= Time.deltaTime * shrinkSpeed;
                distArray[i] = Mathf.Max(distArray[i], 0);
            }
            yield return null;
        }
        }
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        EffectMaterial.SetVectorArray("_WorldSpaceScannerPos", posArray);
        EffectMaterial.SetFloatArray("_ScanDistanceArr", distArray);
        RaycastCornerBlit(src, dst, EffectMaterial);
    }

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _camera.farClipPlane;
		float camFov = _camera.fieldOfView;
		float camAspect = _camera.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_camera.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}
}