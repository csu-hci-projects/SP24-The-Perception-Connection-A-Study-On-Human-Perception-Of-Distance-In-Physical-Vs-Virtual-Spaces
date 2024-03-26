using UnityEngine;
using UnityEngine.XR;

public class ScreenBlackout : MonoBehaviour
{
    private bool isBlackout = false;
    private Camera mainCamera;
    private Texture2D blackTexture;
    private Material blackoutMaterial;

    void Start()
    {
        mainCamera = GetComponent<Camera>();

        // Create a 1x1 pixel black texture
        blackTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.Apply();

        // Create a new material with a solid black color
        blackoutMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        blackoutMaterial.SetColor("_Color", Color.black);
    }

    void Update()
    {
        // Check if the 'B' key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBlackout = !isBlackout;
        }
    }

    void OnPreRender()
    {
        // If in blackout mode, render the black texture over the entire screen
        if (isBlackout)
        {
            blackoutMaterial.SetPass(0);
            mainCamera.targetTexture = null;
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Clear(true, true, Color.black);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord2(0f, 0f); GL.Vertex3(-1f, -1f, 1f);
            GL.TexCoord2(0f, 1f); GL.Vertex3(-1f, 1f, 1f);
            GL.TexCoord2(1f, 1f); GL.Vertex3(1f, 1f, 1f);
            GL.TexCoord2(1f, 0f); GL.Vertex3(1f, -1f, 1f);
            GL.End();
            GL.PopMatrix();
        }
    }

    void OnPostRender()
    {
        // If in blackout mode, bind the black texture and draw a fullscreen quad
        if (isBlackout)
        {
            blackoutMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Vertex3(-1f, -1f, 0f);
            GL.Vertex3(-1f, 1f, 0f);
            GL.Vertex3(1f, 1f, 0f);
            GL.Vertex3(1f, -1f, 0f);
            GL.End();
            GL.PopMatrix();
        }
    }
}