using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("ズーム速度")] private int _zoomSpeed = 1;
    [SerializeField, Tooltip("カメラ移動速度")] private int _cameraMoveSpeed = 1;
    [SerializeField, Tooltip("カメラ回転速度")] private int _cameraRotateSpeed = 1;
    [Tooltip("垂直回転最小角度"), SerializeField, Range(-89f, 89f)] private float _minVerticalAngle = -30f;
    [Tooltip("垂直回転最大角度"), SerializeField, Range(-89f, 89f)] private float _maxVerticalAngle = 60f;
    [Tooltip("カメラ移動方向反転"), SerializeField] private bool _reverseCameraMovement = false;
    [Tooltip("カメラ回転方向反転"), SerializeField] private bool _reverseCmaeraRotate = false;

    private UserInputAction _userInputAction = default;
    private Camera _camera = default;

    private Vector3 _movement = new Vector3(0, 0, 0);
    [SerializeField] private Vector2 _orbitAngles = new Vector2(90f, 0f);

    private bool _isPressedMove = false;
    private bool _isPressedRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        _userInputAction = new UserInputAction();
        _userInputAction.Enable();
        if (_userInputAction != null)
        {
            _userInputAction.Camera.Move.started += PressedMove;
            _userInputAction.Camera.Move.canceled += UnPressedMove;
            _userInputAction.Camera.Rotate.started += PressedRotate;
            _userInputAction.Camera.Rotate.canceled += UnPressedRotate;
            _userInputAction.Camera.Zoom.performed += ScrollZoom;
            _userInputAction.Camera.Mouse.performed += ChangeMousePosition;
        }

        TryGetComponent(out _camera);

        _movement = transform.position;
        _orbitAngles.x = transform.rotation.eulerAngles.x;
        _orbitAngles.y = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (_isPressedRotate)
        {
            transform.rotation = Quaternion.Euler(_orbitAngles);
        }
    }

    private void OnDestroy()
    {
        if (_userInputAction != null)
        {
            _userInputAction.Camera.Move.started -= PressedMove;
            _userInputAction.Camera.Move.canceled -= UnPressedMove;
            _userInputAction.Camera.Rotate.started -= PressedRotate;
            _userInputAction.Camera.Rotate.canceled -= UnPressedRotate;
            _userInputAction.Camera.Zoom.performed -= ScrollZoom;
            _userInputAction.Camera.Mouse.performed -= ChangeMousePosition;
        }
    }

    void PressedMove(InputAction.CallbackContext ctx)
    {
        _isPressedMove = true;
    }

    void UnPressedMove(InputAction.CallbackContext ctx)
    {
        _isPressedMove = false;
    }

    void PressedRotate(InputAction.CallbackContext ctx)
    {
        _isPressedRotate = true;
    }

    void UnPressedRotate(InputAction.CallbackContext ctx)
    {
        _isPressedRotate = false;
    }

    void ScrollZoom(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<float>();
        transform.position += transform.forward * _zoomSpeed * scroll;
    }

    void ChangeMousePosition(InputAction.CallbackContext ctx)
    {
        //Debug.Log(ctx.ReadValue<Vector2>());

        if (_isPressedMove == false && _isPressedRotate == false)
        {
            return;
        }

        Vector2 delta = ctx.ReadValue<Vector2>();
        if (_isPressedMove)
        {
            delta *= (_reverseCameraMovement) ? -1 : 1;

            Vector3 verticalInput = delta.y * _cameraMoveSpeed * transform.up * Time.deltaTime;
            Vector3 horizontalInput = delta.x * _cameraMoveSpeed * transform.right * Time.deltaTime;
            Vector3 movement = verticalInput + horizontalInput;
            transform.position += movement;
        }
        else if (_isPressedRotate)
        {
            //float verticalInput = delta.y * _cameraRotateSpeed * Time.deltaTime;
            //float horizontalInput = delta.x * _cameraRotateSpeed * Time.deltaTime;
            // 入力方向を回転軸に変更
            Vector2 inverseDelta = new Vector2(delta.y, delta.x);
            inverseDelta *= (_reverseCmaeraRotate) ? -1 : 1;

            _orbitAngles += _cameraRotateSpeed * Time.unscaledDeltaTime * inverseDelta;
            //Quaternion angle = Quaternion.AngleAxis(verticalInput, Vector3.right) * Quaternion.AngleAxis(horizontalInput, Vector3.up);
            //transform.rotation *= angle;
            //transform.Rotate(Vector3.right, verticalInput);
            //transform.Rotate(Vector3.up, horizontalInput, Space.World);
            _orbitAngles.x = Mathf.Clamp(_orbitAngles.x, _minVerticalAngle, _maxVerticalAngle);
            if (_orbitAngles.y < 0f)
            {
                _orbitAngles.y += 360f;
            }
            else if (_orbitAngles.y >= 360f)
            {
                _orbitAngles.y -= 360f;
            }
        }
    }

    void CheckMouseInGameWindow(InputAction.CallbackContext ctx)
    {
        Vector2 mousePosition = ctx.ReadValue<Vector2>();
        var view = _camera.ScreenToViewportPoint(mousePosition);
        var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        if (isOutside == false)
        {
            Debug.Log(ctx.ReadValue<Vector2>());
        }
    }
}
