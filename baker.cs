using UnityEngine;
using UnityEditor;
using System.IO;

public class BakePoiyomiHueShift : EditorWindow {
    private Material sourceMaterial;
    private string savePath = "Assets/BakedTexture.png";
    private int textureSize = 2048;

    [MenuItem("Tools/Bake Poiyomi Hue Shift")]
    public static void ShowWindow() {
        GetWindow<BakePoiyomiHueShift>("Bake Hue Shift");
    }

    private void OnGUI() {
        GUILayout.Label("Bake Poiyomi Color Adjust to Texture", EditorStyles.boldLabel);
        sourceMaterial = (Material)EditorGUILayout.ObjectField("Source Material", sourceMaterial, typeof(Material), false);
        textureSize = EditorGUILayout.IntField("Texture Size", textureSize);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Bake Texture") && sourceMaterial != null)
            BakeTexture();
    }

    private void BakeTexture() {
        // Grab the main texture from the material
        Texture2D sourceTexture = sourceMaterial.GetTexture("_MainTex") as Texture2D;
        if (sourceTexture == null) {
            // Poiyomi uses _Texture0 as the main texture slot
            sourceTexture = sourceMaterial.GetTexture("_Texture0") as Texture2D;
        }

        if (sourceTexture == null) {
            EditorUtility.DisplayDialog("Error", "Could not find main texture on material.", "OK");
            return;
        }

        // Create a quad with this material and render it
        RenderTexture rt = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        // Blit the texture through the material (applies all shader effects including hue shift)
        Graphics.Blit(sourceTexture, rt, sourceMaterial);

        // Read back to Texture2D
        RenderTexture.active = rt;
        Texture2D result = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        rt.Release();

        // Save as PNG
        byte[] bytes = result.EncodeToPNG();
        string fullPath = Path.Combine(Application.dataPath, "../", savePath);
        File.WriteAllBytes(fullPath, bytes);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Done!", $"Texture saved to {savePath}", "OK");
        DestroyImmediate(result);
    }
}