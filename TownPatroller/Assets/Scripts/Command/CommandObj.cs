using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandObj : MonoBehaviour
{
    public GameObject inputFieldObj;
    private InputField CommandInputField;

    // Start is called before the first frame update
    void Start()
    {
        inputFieldObj = GameObject.Find("CommandInputField");
        CommandInputField = inputFieldObj.GetComponent<InputField>();

        CommandInputField.Select();
        CommandInputField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            IGConsole.Instance.Main.println(">" + CommandInputField.text);
            CommandInputField.text = "";
            CommandInputField.Select();
            CommandInputField.ActivateInputField();
        }
    }
}
