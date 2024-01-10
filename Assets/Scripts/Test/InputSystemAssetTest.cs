using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputSystemAssetTest : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Test.Tab.started += TabStarted;
        _playerInputActions.Test.Tab.performed += TabPreformed;
        _playerInputActions.Test.Tab.canceled += TabCanceled;

        // _playerInputActions.Test.Tab.started += ctx =>
        // {
        //     Debug.Log("操作開始");
        // };
        // _playerInputActions.Test.Tab.performed += ctx =>
        // {
        //     if (ctx.interaction is MultiTapInteraction)
        //     {
        //         Debug.Log("ダブルクリック");
        //     }
        //     else if (ctx.interaction is HoldInteraction)
        //     {
        //         Debug.Log("長押し");
        //     }
        //     else
        //     {
        //         Debug.Log("例外");
        //     }
        // };
        // _playerInputActions.Test.Tab.canceled += ctx =>
        // {
        //     if (ctx.interaction is MultiTapInteraction)
        //     {
        //         Debug.Log("クリック");
        //     }
        // };
    }

    private void OnEnable()
    {
        _playerInputActions.Enable(); // 全てのインプットマップを有効化
    }

    private void OnDisable()
    {
        _playerInputActions.Disable(); // 全てのインプットマップを無効化
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void TabStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("操作開始");
    }

    void TabPreformed(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is MultiTapInteraction)
        {
            Debug.Log("ダブルクリック");
        }
        else if (ctx.interaction is HoldInteraction)
        {
            Debug.Log("長押し");
        }
        else
        {
            Debug.Log("例外");
        }
    }

    void TabCanceled(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is MultiTapInteraction)
        {
            Debug.Log("クリック");
        }
    }
}
