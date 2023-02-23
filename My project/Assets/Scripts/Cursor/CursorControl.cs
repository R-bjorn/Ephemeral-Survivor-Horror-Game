using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControl : MonoBehaviour
{
    private bool _isCursorLocked = false;

    [SerializeField] private KeyCode unlockCursorKey = KeyCode.Escape;
    // Start is called before the first frame update
    void Start()
    {
        LockCursor();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(unlockCursorKey))
        {
            ToggleCursorLock();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isCursorLocked = true;
    }
    
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isCursorLocked = false;
    }
    
    private void ToggleCursorLock()
    {
        if (_isCursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }
    
}
