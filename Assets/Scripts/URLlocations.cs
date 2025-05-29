using UnityEngine;
using System.Collections;

public class URLlocations : MonoBehaviour
{
    public static URLlocations instance;

    private void Start()
    {
        instance = this;
        Initialize();
    }

    private string serviceURL, webreporterURL;

    //private static string serviceURL = "http://localhost/statisticallygrounded/addrow.php";
    //private static string serviceURL_local = "http://statgamesstaging.tietronix.com/statisticallygrounded/addrow.php";

    //private static string webreporterURL = "http://localhost/statisticallygrounded/webreporter.php";
    //private static string webreporterURL_local = "http://statgamesstaging.tietronix.com/statisticallygrounded/webreporter.php";

    private void Initialize()
    {
        StartCoroutine("LoadUrls");
    }

    public string GetServiceUrl()
    {
        return serviceURL;
    }

    public string GetWebReporterUrl()
    {
        return webreporterURL;
    }
    
    private IEnumerator LoadUrls()
    {
        string path = Application.dataPath + "/StreamingAssets/url.txt";
        if (!path.Contains("://"))
            path = "file://" + path;
        WWW www = new WWW(path);
        yield return www;
        string fileText = System.Text.Encoding.UTF8.GetString(www.bytes);
        if (!string.IsNullOrEmpty(fileText))
        {
            serviceURL = LoadUrl(fileText, 0);
            webreporterURL = LoadUrl(fileText, 1);
        }
    }

    private string LoadUrl(string fileContents, int num)
    {
        var lines = fileContents.Split("\n"[0]);

        // make sure we dont do something dumb
        if (lines.Length <= num)
        {
            Debug.LogWarning("we didnt pull enough urls from our text file");
            return null;
        }

        var splitAgain = lines[num].Split(',');

        if (splitAgain.Length != 2)
        {
            Debug.LogWarning("incorrectly formatted text file for urls");
            return null;
        }

        return splitAgain[1];
    }
}