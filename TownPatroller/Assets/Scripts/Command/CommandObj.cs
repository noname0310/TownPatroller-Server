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

    private LinkedList<string> CommandBuffer;
    private LinkedListNode<string> CommandBufferSearchIndex;

    void Start()
    {
        Instance = this;

        inputFieldObj = GameObject.Find("CommandInputField");
        CommandInputField = inputFieldObj.GetComponent<InputField>();

        CommandInputField.Select();
        CommandInputField.ActivateInputField();

        CommandDelegates = new Dictionary<string, CommandDelegate>();
        commandManager = new CommandManager();

        CommandBuffer = new LinkedList<string>();
        CommandBufferSearchIndex = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string cmd = CommandInputField.text;

            if (cmd.Trim() != "")
            {
                if (CommandBuffer.Count == 0 || CommandBuffer.Last.Value != cmd.Trim())
                {
                    CommandBuffer.AddLast(cmd);

                    if (CommandBuffer.Count > 50)
                    {
                        CommandBuffer.RemoveFirst();
                    }
                }
            }

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
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CommandBuffer.Count == 0)
                return;

            if (CommandBufferSearchIndex == null)
            {
                CommandBufferSearchIndex = CommandBuffer.Last;
            }
            else if (CommandInputField.text.Trim() == "")
            {
            }
            else if (CommandBufferSearchIndex.Previous != null)
            {
                CommandBufferSearchIndex = CommandBufferSearchIndex.Previous;
            }

            CommandInputField.text = CommandBufferSearchIndex.Value;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CommandBuffer.Count == 0)
                return;

            if (CommandBufferSearchIndex == null)
            {
                CommandBufferSearchIndex = CommandBuffer.Last;
            }
            else if (CommandInputField.text.Trim() == "")
            {
            }
            else if (CommandBufferSearchIndex.Next != null)
            {
                CommandBufferSearchIndex = CommandBufferSearchIndex.Next;
            }

            CommandInputField.text = CommandBufferSearchIndex.Value;
        }
    }
}
