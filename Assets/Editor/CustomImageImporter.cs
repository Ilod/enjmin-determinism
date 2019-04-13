using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "cimg")]
public class CustomImageImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var texture = new Texture2D(1, 1);
        texture.LoadImage(System.IO.File.ReadAllBytes(ctx.assetPath));
        
        var data = texture.GetPixels()
            .Select(c => c == Color.black)
            .ToArray();

        var image = ScriptableObject.CreateInstance<CustomImage>();
        image.Data = data;
        image.Width = texture.width;
        image.Height = texture.height;
        ctx.AddObjectToAsset("data", image, texture);
        ctx.SetMainObject(image);
    }
}
