using UnityEngine;
using UnityEngine.UI;
using TownPatroller.Console;

public class ConsoleTextManager : MonoBehaviour
{
    public static ConsoleTextManager instance { get; set; }

    public GameObject MainConsoleContent;
    public GameObject MainScrollView;

    public Text MainTextPrefab;
    private ScrollRect MainScrollRect;

    private InGameConsole MainConsole;

    // Start is called before the first frame update
    void Awake()
    {
        MainScrollView = GameObject.Find("Console Scroll View");
        MainScrollRect = MainScrollView.GetComponent<ScrollRect>();
        MainConsoleContent = GameObject.Find("MainConsoleContent");

        DeleteChildObject(MainConsoleContent);

        MainConsoleContent.AddComponent<InGameConsole>();
        MainConsole = MainConsoleContent.GetComponent<InGameConsole>();
        MainConsole._new(MainConsoleContent, MainTextPrefab, MainScrollRect);

        gameObject.AddComponent<IGConsole>();
        IGConsole.Instance.Init();
    }

    void DeleteChildObject(GameObject ParentObject)
    {
        for (int i = 0; i < ParentObject.transform.childCount; i++)
        {
            Destroy(ParentObject.transform.GetChild(i).GetComponent<Text>());
        }
    }
}
