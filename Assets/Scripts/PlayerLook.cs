using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public string mouseXInputName, mouseYInputName;
    public float mouseSensitivity;
    public Transform playerBody;

    private float xAxisClamp;

    public GameObject pauseMenu;

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0;
    }

    private void LockCursor()
    {
        // code-review: You probably want to hide the cursor as well.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private bool isPaused = false;
    private void CheckForPause()
    {
        // code-review: Should be made into a state machine.
        if (Input.GetButtonDown("Cancel"))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                isPaused = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                isPaused = false;
            }
        }
    }

    
    private void Update()
    {
        CheckForPause();
        if(isPaused == false)
            CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90f;
            mouseY = 0.0f;
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;

    }
}
