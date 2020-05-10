using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove: MonoBehaviour
{
    public string horizontalInputName;
    public string verticalInputName;
    public float movementSpeed;

    private CharacterController characterController;

    public bool isJumping;
    public AnimationCurve jumpFallOff;
    public float jumpMultiplier;
    public KeyCode jumpKey;

    [Header("(Juice) Head Left/Right Sway")]
    public Camera playerCamera;
    public bool swayEnabled = false;
    private Vector3 camPos;
    public float swayLength = 8f;
    public float swaySpeed = 4f;

    [Header("(Juice) Head Bob")]
    public bool headBobEnabled = false;
    public float amplitude = 0.01f;
    public float frequency = 1.5f;
    public float speed = 8f;

    [Header("(Juice) Impact")]
    public bool jumpImpactEnabled = false;
    public float impactLen = 2f;
    public float intensity= 10f;


    private void Awake()
    {
        camPos = playerCamera.transform.localPosition;
        characterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        characterController.SimpleMove(forwardMovement + rightMovement);
        { // (Juice) Camera Sway
            if (swayEnabled)
            {
                int n = 1;
                byte ok = Convert.ToByte(Convert.ToBoolean(horizInput));
                float m = 2*(n^ok);
                float s = (swaySpeed+m) * Time.deltaTime;
                float i = Input.GetAxis(horizontalInputName);
                Vector3 bp = playerCamera.transform.localPosition;
                Vector3 endp = new Vector3((camPos.x + (swayLength*.1f)) * i, camPos.y, camPos.z);
                playerCamera.transform.localPosition = ((1 - s) * bp + s * endp);
            }
        }
        { // (Juice) Head Bob
            if (headBobEnabled)
            {
                float s = speed*Time.time;
                playerCamera.transform.localPosition =  new Vector3(
                    playerCamera.transform.localPosition.x,
                    playerCamera.transform.localPosition.y+((amplitude*Mathf.Sin(s*frequency))*
                        (Input.GetAxis(verticalInputName)+Input.GetAxis(horizontalInputName)) ),
                    playerCamera.transform.localPosition.z);
            }
        }

        JumpInput();
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        characterController.slopeLimit = 90.0f;

        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;

        } while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);

        characterController.slopeLimit = 45.0f;

        isJumping = false;
        { // (Juice) Land impact
            if (jumpImpactEnabled)
            {
                float c = impactLen*.2f;
                float cs = intensity*1.5f;
                Vector3 bp = playerCamera.transform.localPosition;
                Vector3 endp = new Vector3(camPos.x, camPos.y - (c), camPos.z);
                playerCamera.transform.localPosition = Vector3.Lerp(bp, endp, cs*Time.deltaTime);
            }
        }
    }
}
