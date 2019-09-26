using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TownPatroller.Command;

public class CommandObj : MonoBehaviour
{
    public static CommandObj Instance { get; set; }

    public GameObject inputFieldObj;
    private InputField CommandInputField;

    public delegate void CommandDelegate(string command, string[] args);
    public Dictionary<string, CommandDelegate> CommandDelegates;
    public CommandManager commandManager;

    void Start()
    {
        Instance = this;

        inputFieldObj = GameObject.Find("CommandInputField");
        CommandInputField = inputFieldObj.GetComponent<InputField>();

        CommandInputField.Select();
        CommandInputField.ActivateInputField();

        CommandDelegates = new Dictionary<string, CommandDelegate>();
        commandManager = new CommandManager();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string cmd = CommandInputField.text;

            IGConsole.Instance.printWarnln(">" + cmd);
            var ParsedCmd = CommandArgsParser.ParseArgs(cmd);

            CommandDelegate Command;
            if(CommandDelegates.TryGetValue(ParsedCmd.Item1.ToUpper(), out Command))
            {
                Command(cmd, ParsedCmd.Item2);
            }
            else
            {
                IGConsole.Instance.println("\"" + ParsedCmd.Item1 + "\" is not exist command");
            }

            CommandInputField.text = "";
            CommandInputField.Select();
            CommandInputField.ActivateInputField();
        }
    }
}
