using UnityEngine;
using UnityEditor;
using System.IO;

public class UnityTextureBaker : EditorWindow
{
    private Material sourceMaterial;
    private string savePath = "Assets/BakedTexture.png";
    private int textureSize = 2048;

    [MenuItem("Tools/Texture Baker")]
    public static void ShowWindow()
    {
        GetWindow<UnityTextureBaker>("Texture Baker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Bake Shader Effects to Texture", EditorStyles.boldLabel);
        sourceMaterial = (Material)EditorGUILayout.ObjectField("Source Material", sourceMaterial, typeof(Material), false);
        textureSize = EditorGUILayout.IntField("Texture Size", textureSize);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Bake Texture") && sourceMaterial != null)
            BakeTexture();
    }

    private void BakeTexture()
    {
        Texture2D sourceTexture = sourceMaterial.GetTexture("_MainTex") as Texture2D;
        if (sourceTexture == null)
        {
            sourceTexture = sourceMaterial.GetTexture("_Texture0") as Texture2D;
        }

        if (sourceTexture == null)
        {
            EditorUtility.DisplayDialog("Error", "Could not find main texture on material.", "OK");
            return;
        }

        RenderTexture rt = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        Graphics.Blit(sourceTexture, rt, sourceMaterial);

        RenderTexture.active = rt;
        Texture2D result = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        rt.Release();

        byte[] bytes = result.EncodeToPNG();
        string fullPath = Path.Combine(Application.dataPath, "../", savePath);
        File.WriteAllBytes(fullPath, bytes);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Done!", $"Texture saved to {savePath}", "OK");
        DestroyImmediate(result);
    }
}