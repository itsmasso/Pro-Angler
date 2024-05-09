using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // The custom cursor texture
    public Vector2 hotspot = Vector2.zero; // The cursor's hotspot

    void Start()
    {
        // Set the custom cursor
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
