using UnityEngine;
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

    public void println(string msg)
    {
        Main.println(msg);
    }

    public void println(string msg, bool DisplayTime)
    {
        Main.println(msg, DisplayTime);
    }

    public void printWarnln(string msg)
    {
        Main.printWarnln(msg);
    }
}
