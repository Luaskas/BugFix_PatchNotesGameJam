using UnityEngine.InputSystem;

namespace Controller
{
    using UnityEngine;

    public class CursorController : MonoBehaviour
    {
        // Hide the cursor
        public void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Show the cursor
        public void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        void Start()
        {
            // Start with the cursor visible
            ShowCursor();
        }

        void Update()
        {
            // Left-click inside the game → hide cursor
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                HideCursor();
            }

            // Press Escape → show cursor again
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ShowCursor();
            }
        }
    }

}