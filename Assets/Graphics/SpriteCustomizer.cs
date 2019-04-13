using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpriteCustomizer
{
    public CustomImage Image;
    public Color Color;
    public int PlayerIndex;
    public Texture2D Texture { get; private set; }
    public Sprite Sprite { get; private set; }

    public SpriteCustomizer(CustomImage image, Color color, int playerIndex)
    {
        Image = image;
        Color = color;
        PlayerIndex = playerIndex;
    }

    public IEnumerator Customize()
    {
        var result = SpriteCustomization.SpriteCustomizerManager.Instance.CustomizeImage(Image.Data, new SpriteCustomization.Color(Color.r, Color.g, Color.b, Color.a), PlayerIndex);
        while (!result.IsCompleted)
            yield return null;
        Texture = new Texture2D(Image.Width, Image.Height, TextureFormat.ARGB32, true, true);
        Texture.SetPixels(result.Result.Select(c => new Color(c.r, c.g, c.b, c.a)).ToArray());
        Sprite = Sprite.Create(Texture, new Rect(0, 0, Image.Width, Image.Height), new Vector2(0.5f, 0.5f), 32);
    }
}