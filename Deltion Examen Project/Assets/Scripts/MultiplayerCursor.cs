//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using System.Runtime.InteropServices;

//public class MultiplayerCursor : MonoBehaviour
//{
//    public int myPlayerIndex = 0;
//    private Image cursorImg;
//    private GameObject currentHover;
//    void Start()
//    {
//        cursorImg = GetComponent<Image>();
//        cursorImg.color = GameManager.instance.playerColors[myPlayerIndex];
//    }

//    private void Update()
//    {
//        if (hinput.gamepad[myPlayerIndex].A.justPressed)
//            Click();

//        Vector2 move = new Vector2(hinput.gamepad[myPlayerIndex].leftStick.horizontalRaw, hinput.gamepad[myPlayerIndex].leftStick.verticalRaw);
//        transform.position = new Vector3(transform.position.x + move.x * 3, transform.position.y + move.y * 3, 0);
//    }

//    void Click()
//    {
      
//        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown | MouseOperations.MouseEventFlags.LeftUp);
//    }

   

//}
