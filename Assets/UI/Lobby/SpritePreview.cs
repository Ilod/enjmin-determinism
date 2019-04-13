using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritePreview : MonoBehaviour {
    public CustomImage image;
    private int index;

    private Color lastColor;
    private int request = 0;
    private int validated = 0;

    // Use this for initialization
    IEnumerator Start()
    {
        var wheel = transform.parent.gameObject.GetComponentInChildren<ColorWheelControl>();
        if (wheel != null)
            lastColor = wheel.Selection;
        index = GetComponentInParent<PlayerPanelInfo>().playerIndex;
        
        yield return StartCoroutine(UpdateColor());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator UpdateColor()
    {
        var id = ++request;
        var color = lastColor;
        var customizer = new SpriteCustomizer(image, color, index);
        yield return StartCoroutine(customizer.Customize());
        if (validated < id)
        {
            validated = id;
            GetComponent<RawImage>().texture = customizer.Texture;
        }
    }

    public void SetColor(Color color)
    {
        if (lastColor != color)
        {
            lastColor = color;
            StartCoroutine(UpdateColor());
        }
    }
}
