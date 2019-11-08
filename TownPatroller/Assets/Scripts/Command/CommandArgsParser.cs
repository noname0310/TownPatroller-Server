using System.Collections.Generic;
using System.Text;

namespace TownPatroller.Command
{
    class CommandArgsParser
    {
        public static (string, string[]) ParseArgs(string msg)
        {
            msg = RemoveSpace(msg);
            (string, List<string>) splitArgs = SplitArgs(msg);

            splitArgs.Item1 = SwitchQuote(splitArgs.Item1);
            for (int i = 0; i < splitArgs.Item2.Count; i++)
            {
                splitArgs.Item2[i] = SwitchQuote(splitArgs.Item2[i]);
            }

            return (splitArgs.Item1, splitArgs.Item2.ToArray());
        }

        private static string SwitchQuote(string msg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool PrevIsOSlash = false;

            for (int i = 0; i < msg.Length; i++)
            {
                if (PrevIsOSlash)
                {
                    if (msg[i] == '\"')
                    {
                        stringBuilder.Append('\"');
                    }
                    else
                    {
                        stringBuilder.Append('\\');
                        stringBuilder.Append(msg[i]);
                    }
                }
                else
                {
                    if(msg[i] != '\"' && msg[i] != '\\')
                    {
                        stringBuilder.Append(msg[i]);
                    }
                }

                if (msg[i] == '\\')
                {
                    PrevIsOSlash = true;
                }
                else
                {
                    PrevIsOSlash = false;
                }
            }

            return stringBuilder.ToString();
        }

        private static (string, List<string>) SplitArgs(string msg)
        {
            string Head = "";
            List<string> Splitmsg = new List<string>();
            bool IsFirstValue = true;
            bool PrevIsOSlash = false;
            bool FindSpace = true;

            int startindex = 0;
            int endindex = 0;

            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i] == '\"' && PrevIsOSlash == false)
                {
                    FindSpace = !FindSpace;
                }

                if (msg[i] == '\\')
                {
                    PrevIsOSlash = true;
                }
                else
                {
                    PrevIsOSlash = false;
                }

                if (msg[i] == ' ' && FindSpace == true)
                {
                    endindex = i - 1;
                    if (IsFirstValue)
                    {
                        Head = msg.Substring(startindex, endindex - startindex + 1);
                        IsFirstValue = false;
                    }
                    else
                        Splitmsg.Add(msg.Substring(startindex, endindex - startindex + 1));
                    startindex = i + 1;
                }
            }
            if (IsFirstValue)
            {
                Head = msg.Substring(startindex);
                IsFirstValue = false;
            }
            else
                Splitmsg.Add(msg.Substring(startindex));

            return (Head, Splitmsg);
        }

        private static string RemoveSpace(string msg)
        {
            msg = msg.Trim();

            StringBuilder stringBuilder = new StringBuilder();

            bool PrevIsSpace = false;
            bool PrevIsOSlash = false;
            bool FindSpace = true;

            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i] == '\"' && PrevIsOSlash == false)
                {
                    FindSpace = !FindSpace;
                }

                if (msg[i] == '\\')
                {
                    PrevIsOSlash = true;
                }
                else
                {
                    PrevIsOSlash = false;
                }

                if (FindSpace)
                {
                    if (msg[i] == ' ')
                    {
                        if (!PrevIsSpace)
                        {
                            stringBuilder.Append(msg[i]);
                        }
                        PrevIsSpace = true;
                    }
                    else
                    {
                        stringBuilder.Append(msg[i]);
                        PrevIsSpace = false;
                    }
                }
                else
                {
                    stringBuilder.Append(msg[i]);
                    PrevIsSpace = false;
                }
            }

            return stringBuilder.ToString();
        }
    }
}
