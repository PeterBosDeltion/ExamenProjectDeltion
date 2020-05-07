using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursorFunctionality : MonoBehaviour
{
    public Vector3 cursorOffset;
    public Sprite cursor;
    public Sprite crosshair;

    private bool cursorActive;
    private Image cursorImage;

    public Color cursorColor;
    public Color croshairColor;


    private void Start()
    {
        Cursor.visible = false;
        cursorImage = GetComponent<Image>();
        GameManager.cursorEvent += UpdateCursorArt;
        UpdateCursorArt();
    }

    private void OnDestroy()
    {
        GameManager.cursorEvent -= UpdateCursorArt;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        if (cursorActive)
            mousePos += cursorOffset;

        transform.position = mousePos;
    }

    public void UpdateCursorArt()
    {
        switch (GameManager.instance.curentCursorState)
        {
            case GameManager.CursorState.Cursor:
                cursorImage.gameObject.SetActive(true);
                cursorImage.sprite = cursor;
                cursorImage.color = cursorColor;
                cursorActive = true;
                break;
            case GameManager.CursorState.Crosshair:
                cursorImage.gameObject.SetActive(true);
                cursorImage.sprite = crosshair;
                cursorImage.color = croshairColor;
                cursorActive = false;
                break;
            case GameManager.CursorState.Empty:
                cursorImage.gameObject.SetActive(false);
                break;
        }
    }
}