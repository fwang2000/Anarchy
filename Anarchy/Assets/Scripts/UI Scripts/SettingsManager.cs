using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class SettingsManager : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();


    public void ClickQuitButton()
    {
        Application.Quit();
    }

    public void ClickMinimizeButton()
    {
        ShowWindow(GetActiveWindow(), 2);
    }
}
