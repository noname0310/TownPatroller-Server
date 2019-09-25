﻿using UnityEngine;
using TownPatroller.Console;

public class IGConsole : MonoBehaviour
{
    public static IGConsole Instance { get; set; }

    public GameObject MainConsoleContent;
    public InGameConsole Main;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        MainConsoleContent = GameObject.Find("MainConsoleContent");

        Main = MainConsoleContent.GetComponent<InGameConsole>();
    }
}