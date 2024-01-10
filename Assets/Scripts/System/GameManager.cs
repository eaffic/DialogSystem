using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class GameManager : UnitySingleton<GameManager>
{
    private PlayerInputActions _playerInputActions;
    public PlayerInputActions GetPlayerInputActions => _playerInputActions;

    protected override void Awake()
    {
        base.Awake();

        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
