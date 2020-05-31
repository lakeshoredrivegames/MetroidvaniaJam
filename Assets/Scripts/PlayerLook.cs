using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public string mouseXInputName, mouseYInputName;
    public float mouseSensitivity;
    public Transform playerBody;

    [Header("Grab 2D UI")]
    public GameObject grabSprite;
    public string grabSpriteTagName;
    public float grabSpriteRaycastLength;

    private float xAxisClamp;

    public GameObject pauseMenu;

    private void Awake()
    {
        LockCursor();
	Cursor.visible = false;
        xAxisClamp = 0;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private bool isPaused = false;
    private void CheckForPause()
    {
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

    private void RaycastThenSetHUD()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.gameObject.transform.position, 
            this.gameObject.transform.forward, out hit, grabSpriteRaycastLength))
        {
            if (hit.transform.gameObject.tag == grabSpriteTagName)
            {
                grabSprite.SetActive(true);
            }
            else
            {
                grabSprite.SetActive(false);
            }
        }
        else
        {
            grabSprite.SetActive(false);
        }
    }

    private void Update()
    {
        CheckForPause();
	RaycastThenSetHUD();

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
