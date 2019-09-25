using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TownPatroller.Console
{
    public class InGameConsole : MonoBehaviour
    {
        private GameObject ConsoleContent;
        private Text TextPrefab;
        private LinkedList<Text> TextOBJs;
        private LinkedList<Text> ActiveTextOBJs;

        private ScrollRect scrollRect;

        private const int CreateTickCount = 10;
        private const int MaxCount = 300;

        private bool firstmsg;
        private int CreatePos;

        public void _new(GameObject consoleContent, Text textPrefab, ScrollRect scrollrect)
        {
            ConsoleContent = consoleContent;
            TextPrefab = textPrefab;
            scrollRect = scrollrect;

            TextOBJs = new LinkedList<Text>();
            ActiveTextOBJs = new LinkedList<Text>();

            firstmsg = true;
            CreatePos = 0;
        }

        private void CreateTextPrefabs(int createcount)
        {
            for (int i = 0; i < createcount; i++)
            {
                TextOBJs.AddFirst(Instantiate(TextPrefab, ConsoleContent.transform));
                TextOBJs.First.Value.gameObject.SetActive(false);
            }
        }

        private void GetOlderText()
        {
            CreatePos -= (int)ActiveTextOBJs.Last.Value.rectTransform.sizeDelta.y;

            for (int i = 0; i < ConsoleContent.transform.childCount; i++)
            {
                RectTransform ChildRectTransform = ConsoleContent.transform.GetChild(i).GetComponent<RectTransform>();

                ChildRectTransform.localPosition = new Vector3(
                    ChildRectTransform.localPosition.x,
                    ChildRectTransform.localPosition.y + (int)ActiveTextOBJs.Last.Value.rectTransform.sizeDelta.y,
                    ChildRectTransform.localPosition.z);
            }

            ActiveTextOBJs.Last.Value.rectTransform.localPosition = new Vector3(
                ActiveTextOBJs.Last.Value.rectTransform.localPosition.x,
                TextPrefab.rectTransform.localPosition.y,
                ActiveTextOBJs.Last.Value.rectTransform.localPosition.z);

            ConsoleContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ConsoleContent.GetComponent<RectTransform>().sizeDelta.x, CreatePos);
            scrollRect.verticalNormalizedPosition = 0;

            Text text = ActiveTextOBJs.Last.Value;
            ActiveTextOBJs.RemoveLast();

            ActiveTextOBJs.AddFirst(text);
        }

        public void println(string msg)
        {
            if(ActiveTextOBJs.Count >= MaxCount)
            {
                GetOlderText();
            }
            else
            {
                if (TextOBJs.Count == 0)
                    CreateTextPrefabs(CreateTickCount);

                Text text = TextOBJs.Last.Value;
                TextOBJs.RemoveLast();

                ActiveTextOBJs.AddFirst(text);
            }

            Text crtext = ActiveTextOBJs.First.Value;

            crtext.gameObject.SetActive(true);

            if (firstmsg)
            {
                crtext.text = ",";
                crtext.GetComponent<ContentSizeFitter>().SetLayoutVertical();
                CreatePos = (int)crtext.rectTransform.sizeDelta.y;
                firstmsg = false;
            }

            crtext.text = msg;

            crtext.GetComponent<ContentSizeFitter>().SetLayoutVertical();

            crtext.rectTransform.localPosition = new Vector3(
                crtext.rectTransform.localPosition.x, 
                crtext.rectTransform.localPosition.y - CreatePos, 
                crtext.rectTransform.localPosition.z);

            CreatePos += (int)crtext.rectTransform.sizeDelta.y;
            ConsoleContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ConsoleContent.GetComponent<RectTransform>().sizeDelta.x, CreatePos);

            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}
