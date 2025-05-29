using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class StartScene : MonoBehaviour {

    public static StartScene instance;

    [SerializeField] public InputField nameInput;
    [SerializeField] public InputField groupInput;
    public Selectable firstInput;

    // Element Flow Controls
    EventSystem system;

    // Bad Word Filter 
    [SerializeField] public TextAsset badWordsFile;
    [SerializeField] public GameObject badWordWarningScreen;
    [SerializeField] public GameObject alphaNumWarningScreen;
    private string[] badWords;

    [SerializeField] public Button continueBtn;

     public void Start()
    {
        badWords = badWordsFile.text.Split(',');
        continueBtn.interactable = false;
        firstInput.Select();
        system = EventSystem.current;

    
       // playerName = "";
       // groupName = "";
        //totalTests = 0;
    }

    void Update()
    {
        CheckIDInput();
        NavigateTab();
    }

    private void CheckIDInput()
    {
        if (nameInput.text != "" && groupInput.text != "" && !IsBadWord(nameInput.text, groupInput.text) && IsAlphaNumeric(nameInput.text, groupInput.text))
        {
            continueBtn.interactable = true;
            badWordWarningScreen.SetActive(false);
            alphaNumWarningScreen.SetActive(false);
        }
        else if (IsBadWord(nameInput.text, groupInput.text))
        {
            continueBtn.interactable = false; 
            badWordWarningScreen.SetActive(true);
            alphaNumWarningScreen.SetActive(false); 
        }
        else if (!IsAlphaNumeric(nameInput.text, groupInput.text))
        {
            alphaNumWarningScreen.SetActive(true);
            continueBtn.interactable = false;
            badWordWarningScreen.SetActive(false);
        }
        else
        {
            badWordWarningScreen.SetActive(false);
            alphaNumWarningScreen.SetActive(false);
            continueBtn.interactable = false;
        }
    }

    private bool IsBadWord(string player, string group)
    {
        player = player.ToLower();
        group = group.ToLower();

        foreach (string badword in badWords)
        {
            string newbadword = badword.Substring(1);

            if (player.Contains(newbadword))
            {
                return true;
            }
            if (group.Contains(newbadword))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsAlphaNumeric(string player, string group)
    {
        return player.All(char.IsLetterOrDigit) && group.All(char.IsLetterOrDigit);
    }

    private void NavigateTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
    }
}