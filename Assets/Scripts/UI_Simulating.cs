using UnityEngine;
using System.Collections;

public class UI_Simulating : MonoBehaviour
{
    private static UI_Simulating obj;
    public static UI_Simulating This() { return obj; }

    public Texture2D bgImg, foodtruckSlideImg;
    private Rect bgRect, slideEndRect;
    private float offsetX;

    void Start()
    {
        obj = this;

        bgRect = UIUtil.NewRect(0, 0, 1024, 768);
        slideEndRect = UIUtil.NewRect(517, 354, 1500, 279);
    }

    public void StartSlide()
    {
        offsetX = UIUtil.ScaleFloat(1026);
    }

    public void Draw()
    {
        //GUI.Box(UIUtil.NewRect(362, 319, 300, 130), "Simulating");
        GUI.DrawTexture(bgRect, bgImg);

        GUI.DrawTexture(slideRectCalculate(), foodtruckSlideImg);
    }

    private Rect slideRectCalculate()
    {
        offsetX = Mathf.Lerp(offsetX, 0, Time.deltaTime);
        return new Rect(slideEndRect.x - offsetX, slideEndRect.y, slideEndRect.width, slideEndRect.height);
    }
}