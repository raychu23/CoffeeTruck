using UnityEngine;
using System.Collections;

public class UI_SetupPulldown : MonoBehaviour
{
    GUIContent[] comboBoxList;
    public ComboBox comboBoxControl;// = new ComboBox();
    public GUIStyle listStyle, btnStyle;
    private Rect dropdownRect;

    private static UI_SetupPulldown obj;
    public static UI_SetupPulldown This() { return obj; }
    

    void Start()
    {
        obj = this;

        dropdownRect = UIUtil.NewRect(473, 322, 204, 28);

        comboBoxList = new GUIContent[3];
        comboBoxList[0] = new GUIContent("Random Sample");
        comboBoxList[1] = new GUIContent("Sequential");
        comboBoxList[2] = new GUIContent("Unbalanced");

        listStyle.normal.textColor = Color.white;
        listStyle.padding.left =
        listStyle.padding.right =
        listStyle.padding.top =
        listStyle.padding.bottom = 4;

        comboBoxControl = new ComboBox(dropdownRect, comboBoxList[0], comboBoxList, btnStyle, "box", listStyle);

        btnStyle = UIUtil.FontFix(btnStyle);
        listStyle = UIUtil.FontFix(listStyle);
    }

    public void Draw()
    {
        comboBoxControl.Show();
    }
}