using UnityEngine;
using System.Collections;

public class UIUtil : MonoBehaviour
{
    private static float scale;

    public static float GetScale()
    {
        return scale;
    }

    public static Rect NewRect(float x, float y, float width, float height)
    {
        return new Rect(ScaleFloat(x), ScaleFloat(y), ScaleFloat(width), ScaleFloat(height));
    }

    public static Rect NewRect(Rect foo)
    {
        return NewRect(foo.x, foo.y, foo.width, foo.height);
    }

    public static Vector2 NewVector2(float x, float y)
    {
        return new Vector2(ScaleFloat(x), ScaleFloat(y));
    }

    public static float ScaleFloat(float x)
    {
        while (scale == 0)
        {
            SetScale();
        }

        return (x / scale);
    }

    public static float ScaleFloat(int x)
    {
        return ScaleFloat((float)x);
    }

    private static void SetScale()
    {
        scale = Mathf.Max(1024f / Screen.width, .01f);
        //Debug.Log("Scale: " + scale);
    }

    public static GUIStyle FontFix(GUIStyle style)
    {
        if (style.font == null)
            return style; // we dont have a font to mess with

        style.fontSize = Mathf.RoundToInt(ScaleFloat(FontCheck(style.font)));
        // JEFF NOTE:
        // basically, webgl builds don't seem to scale fonts correctly
        // since we are building at 800 x 600, i scaled all of the fonts down in the editor
        // We are still pulling their original sizes because hte edit scales fonts correctly
        // but in essence all this scaling doesnt actually affect the webgl build

        return style;
    }


    private static int FontCheck(Font foo)
    {
        string fontName = foo.name;

        if (fontName == "calibri")
            return 16;

        if (fontName == "calibri_button")
            return 25;

        if (fontName == "calibri_graph")
            return 22;

        if (fontName == "calibri_story")
            return 35;

        if (fontName == "calibri_textField")
            return 18;

        Debug.LogError("Font name not found");
        return 0;
    }
}
