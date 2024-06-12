using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    public Sprite[] sprites;

    [Button]
    public void CreateTexture()
    {
        if (!Directory.Exists("Assets/Source/Textures"))
            Directory.CreateDirectory("Assets/Source/Textures");
        
        foreach (var t in sprites)
        {
            Texture2D texture = new Texture2D(
                (int)t.textureRect.width,
                (int)t.textureRect.height
            );

            Color[] pixels = t.texture.GetPixels(
                (int)t.textureRect.x,
                (int)t.textureRect.y,
                (int)t.textureRect.width,
                (int)t.textureRect.height
            );

            texture.SetPixels(pixels);
            texture.Apply();
        
            File.WriteAllBytes("Assets/Source/Textures/" + t.name + ".png", texture.EncodeToPNG());
        }
    }
}