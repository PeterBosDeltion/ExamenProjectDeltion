using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveIcon : MonoBehaviour
{
    public Image crossImage;
    public void Complete()
    {
        crossImage.enabled = true;
    }
}
