using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoudoutMainMenuButton : MonoBehaviour
{
    public int abilityIndex = 0;
    public string clickedType;

    public Image myImage;

    public void Clicked()
    {
        LoudoutManager.instance.OpenLoadoutSelectableWindow(clickedType, abilityIndex, this);
    }
}
