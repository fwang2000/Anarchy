using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FontColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject loginText;
    private TextMeshProUGUI loginTextGUI;
    private bool mouse_over = false;

    private void Start()
    {
        loginTextGUI = loginText.GetComponent<TextMeshProUGUI>();
        Debug.Log(loginTextGUI);
    }

    private void Update()
    {
        if (mouse_over)
        {
            loginTextGUI.color = new Color(1, 0.9098f, 0.6745f);
        }
        else
        {
            loginTextGUI.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
    }
}
