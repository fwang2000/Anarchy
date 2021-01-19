using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimateText : MonoBehaviour
{
    private int maxFontSize = 20;
    private float fontScale = 1f;
    private float spaceScale = 1f;

    private TextMeshProUGUI text;

    public void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Animate(float scaleTime)
    {
        if (text.fontSize < maxFontSize)
        {
            text.fontSize += fontScale / scaleTime * Time.deltaTime;
            text.characterSpacing += spaceScale / scaleTime * Time.deltaTime;
        }
    }

    public void Reset(float scaleTime)
    {
        if (text.fontSize > 16)
        {
            text.fontSize -= fontScale / scaleTime * Time.deltaTime;
            text.characterSpacing -= spaceScale / scaleTime * Time.deltaTime;
        }
    }
}
