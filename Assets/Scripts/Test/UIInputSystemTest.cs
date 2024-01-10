using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class UIInputSystemTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;      // ゲームパッド
        var keyboard = Keyboard.current;    // キーボード
        var mouse = Mouse.current;          // マウス
        var pointer = Pointer.current;      // ポインター

        if (gamepad != null)
        {
            Debug.Log("[gamepad]" + " LeftStick " + gamepad.leftStick.ReadValue());
            if (gamepad.bButton.wasPressedThisFrame)
            {
                Debug.Log("[gamepad] B");
            }
        }

        if (keyboard != null)
        {
            // 実行順:
            // isPressed = false ->
            // 入力 wasPressedThisFrame = true ->
            // ボールド isPressed = true ->
            // 解除 wasReleasedThisFrame = true ->
            // isPressed = false
            if (keyboard.wKey.wasPressedThisFrame)
                Debug.Log("[keyboard] w Enter");
            if (keyboard.wKey.wasReleasedThisFrame)
                Debug.Log("[keyboard] w Release");
            if (keyboard.wKey.isPressed)
                Debug.Log("[keyboard] w Pressed");
        }

        if (mouse != null)
        {
            // スクロールの値(前スクロール>0)
            if (mouse.scroll.ReadValue() != Vector2.zero)
                Debug.Log("[mouse]" + " scroll " + mouse.scroll.ReadValue());

            if (mouse.leftButton.wasPressedThisFrame)
                Debug.Log("[mouse] left Enter");
            if (mouse.leftButton.wasReleasedThisFrame)
                Debug.Log("[mouse] left Release");
            if (mouse.leftButton.isPressed)
                Debug.Log("[mouse] left Pressed");

            if (mouse.middleButton.wasPressedThisFrame)
                Debug.Log("[mouse] middle Enter");
        }

        if (pointer != null)
        {
            //Debug.Log(pointer.delta.ReadValue());       // 前フレームのオフセット
            //Debug.Log(pointer.position.ReadValue());    // 空間中の座標
        }
    }
}
