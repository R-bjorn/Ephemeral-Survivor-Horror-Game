using UnityEngine;

namespace CursorControl
{
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
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            _isCursorLocked = true;
        }
    
        private void UnlockCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
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
}
