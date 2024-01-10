using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ObjectInputSystemTest : MonoBehaviour
{
    [SerializeField]
    InputAction m_Movement;

    private void OnEnable()
    {
        m_Movement.Enable();
    }

    private void OnDisable()
    {
        m_Movement.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Movement != null)
        {
            if (m_Movement.ReadValue<Vector2>() != Vector2.zero)
            {
                float horizontal = m_Movement.ReadValue<Vector2>().x;
                float vertical = m_Movement.ReadValue<Vector2>().y;
                Debug.Log($"[object] ({horizontal} , {vertical})");
            }
        }
    }
}
