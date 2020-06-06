using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : StateMachine
{
    #region Fields and Properties

    State currentState;
   

    [SerializeField] public float throwStrength;
    [SerializeField] public GameObject HoldPosition;
    [SerializeField] private float raycastDistance = 20.0f;

    public bool canPlaceObject;
    public GameObject snapObject;
    public InteractObject heldObject;
    public GameObject actionObject;
    

    public Camera playerCam;
    private Camera objectCam;
    private GameObject objectCamara;

    private int layerMaskPickup;
    private int layerMaskHolding;

    #endregion

    void Start()
    {
        SetState(new BeginState(this));

        playerCam = Camera.main;
        objectCamara = GameObject.Find("ObjectCamera");
        objectCamara.SetActive(true);
        objectCam = objectCamara.GetComponent<Camera>();
        canPlaceObject = false;
        layerMaskPickup = LayerMask.GetMask("Pickup");
        layerMaskHolding = LayerMask.GetMask("Holding");
    }

    void Update()
    {
        currentState = GetState();
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(currentState.InteractObject(playerCam, raycastDistance, layerMaskPickup));
        }
    }
}
