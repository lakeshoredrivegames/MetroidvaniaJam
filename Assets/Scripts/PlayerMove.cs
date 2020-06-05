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
    Rigidbody rb;

    [Header("Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Range(1,10)] public float jumpVelocity;

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
        rb = GetComponent<Rigidbody>();
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

        
    }

    private void FixedUpdate()
    {
        JumpInput();
        BetterJump();
    }

    private void JumpInput()
    {
        if(Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump Button Pressed");
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;
            Debug.Log("Adding velocity to RB");
        }
    }

    private void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    
}
