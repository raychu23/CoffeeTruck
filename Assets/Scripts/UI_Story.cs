using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI_Story : MonoBehaviour
{
    private List<string> storyText;
    private int storyStep;

    void Start()
    {
        storyStep = -1;

        storyText = new List<string>() {
            "Jo just inherited a coffee truck from his uncle. He loves coffee and has decided to start his own coffee company.  He was able to get a loan from his bank, and after painting and repairing the truck he has $20,000 left for supplies and daily life expenses.",
            "He has a fantastic coffee blend that people really seem to like. He can make it for about $.50 a cup. After creating a careful business plan, Jo estimates his daily business and life expenses to be $300 per day. Jo knows you took a great statistics course and wants to use your knowledge to help him make key business decisions.",
            "Help Jo design a study to determine which of the following factors or combination of factors influence sales: location; time of day; price; type of music."
        };

        NextStory();
    }

    public void NextStory()
    {
        storyStep++;

        // if we are past our story text, load our next scene
        if (storyStep >= storyText.Count)
            StartMainScene();

        // otherwise, change text to next step
        else
            DrawStoryText(storyStep);        
    }

    public Text storyTxt;

    private void DrawStoryText(int step)
    {
        storyTxt.text = storyText[step];
    }

    private void StartMainScene()
    {
        SceneManager.LoadScene("SimScene");
        TopBarSaveController.instance.nameInput.text=PlayerPrefs.GetString("NameID","");
        TopBarSaveController.instance.groupInput.text=PlayerPrefs.GetString("GroupID","");
        SimSetup.This().playerName = TopBarSaveController.instance.nameInput.text;
        SimSetup.This().groupName = TopBarSaveController.instance.groupInput.text;
    }
}