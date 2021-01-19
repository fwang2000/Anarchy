using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    private Image img;
    private float loadTime = 1.75f;
    private float alphaChange;
    private RectTransform transform;
    private float scaleTime = 0.05f;
    private Vector3 scaleChange;

    private AnimateText animateText;

    private void Start()
    {
        img = GetComponent<Image>();
        transform = GetComponent<RectTransform>();
        animateText = GameObject.Find("StartButton").GetComponent<AnimateText>();
    }

    private void Update()
    {
        alphaChange = 1.0f / loadTime * Time.deltaTime;

        if (img.fillAmount < 1)
        {
            img.fillAmount += 1.0f / loadTime * Time.deltaTime;

            var tempColor = img.color;
            tempColor.a = tempColor.a + alphaChange;
            img.color = tempColor;
        }

        scaleChange = new Vector3(0.1f, 0.1f, 0);
        var maxSize = new Vector3(1f, 1f, 0);

        if (Vector2.Distance(Input.mousePosition, transform.position) < transform.rect.width - 15)
        {
            animateText.Animate(scaleTime);

            if (transform.localScale.x < maxSize.x)
            {

                transform.localScale += scaleChange / scaleTime * Time.deltaTime;
            }
        } 
        else
        {
            animateText.Reset(scaleTime);

            if (transform.localScale.x > 0.8417202)
            {
                transform.localScale -= scaleChange / scaleTime * Time.deltaTime;
            }
        }
    }
}