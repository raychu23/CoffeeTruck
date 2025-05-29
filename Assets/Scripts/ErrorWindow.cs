using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorWindow : MonoBehaviour {

    public Text errorText;

    public void SetMessage(string message)
    {
        errorText.text = message;
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
