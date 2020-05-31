using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code-review: There should be a description of what this file does.

public class Interact : MonoBehaviour
{
    
    [SerializeField] private float throwStrength;
    [SerializeField] private GameObject HoldPosition;
    [SerializeField] private float raycastDistance = 20.0f;

    public bool canPlaceObject;
    public GameObject snapObject;
    public GameObject heldObject;
    public GameObject actionObject;
    
    private Camera playerCam;
    private Camera objectCam;
    private GameObject objectCamara;
    private bool isHolding;
    // Bit shift the index of the layer (10) to get a bit mask
    private int layerMask;



    void Start()
    {
        // code-review: Move booleans like these to state machines.
        isHolding = false;
        playerCam = Camera.main;
        // code-review: Bad practice. Don't search for objects by name.
        // Plug them in using the inspector or create them at runtime.
        objectCamara = GameObject.Find("ObjectCamera");
        objectCamara.SetActive(true);
        objectCam = objectCamara.GetComponent<Camera>();
        canPlaceObject = false;
        // code-review: I would use a tag rather than a layer. 
        layerMask = LayerMask.GetMask("Pickup");

    }

    void Update()
    {
        // code-review: Will the player be able to rebind their keys?
        // This is also input that is independent of the player.
        // I would change the name of this file to something useful as
        // 'interact' could mean a lot of things.
        if (Input.GetButtonDown("Fire1")) { Interacting(); }
    }
    

    


    private void Interacting()
    {
        Debug.Log("interacting");
        // code-review: Move this whole thing to a state machine.
        if (isHolding == false)
        {

            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, layerMask))
            {
                // code-review: Remove debug drawing from the final game.
                Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * hit.distance, Color.red, 20);
                Debug.Log("hit name: " + hit.collider.gameObject.name);
                // code-review: Isnt this redundant? Why check for a tag if
                // you only hit something that is under the appropriate
                // layer?
                if (hit.collider.gameObject.tag == "Pickup")
                {
                    // code-review: Might be useful to store this in a
                    // member variable instead of allocating a local every time.
                    GetComponent<AudioSource>().Play();
                    heldObject = hit.collider.gameObject;
                    // code-review: This is not clear. What is layer 11?
                    // I assume it is the 'Pickup' layer. Assumptions are
                    // bad.
                    heldObject.layer = 11;
                    
                    // code-review: This lerping mechanism is confusing.
                    // Why don't you just lerp to the desired rotation
                    // of the holder when battery is inside the trigger?
                    
                    //check if battery is in a holder
                    //if so, reduce battery holder count 
                    if(heldObject.GetComponent<Lerping>().inBatteryHolder == true)
                    {
                        heldObject.GetComponent<Lerping>().inBatteryHolder = false;
                        heldObject.GetComponent<Lerping>().SetOldHeldObject();
                        heldObject.GetComponent<Lerping>().oldHeldObject.GetComponent<SnapTrigger>().hasBattery = false;
                        heldObject.GetComponent<Lerping>().oldHeldObject.GetComponent<SnapTrigger>().doorObject.GetComponent<Animate>().Animation();
                    }
                    
                    // code-review: Again, this is more complicated than it
                    // needs to be.
                    //let object know it's time to lerp
                    heldObject.GetComponent<Lerping>().lerpToObject = HoldPosition;
                    heldObject.GetComponent<Lerping>().lerpToPosition = HoldPosition.transform.position;
                    heldObject.GetComponent<Lerping>().StartLerp(true);

                    // code-review: Remove commented code.
                    // Also, this should be moved to its own function,
                    // its purpose is to 'prepare' the object to be carried.
                    //hit.collider.transform.SetParent(HoldPosition.transform);
                    hit.collider.transform.localPosition = Vector3.zero;
                    hit.collider.transform.localRotation = Quaternion.identity;
                    hit.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                    isHolding = true;
                   

                }
            }
        }
        else if (isHolding == true) // is holding object
        {
            // code-review: Remove commented code.
            // Also, all prior statements hold true here for the review.
            
            //transform.parent.gameObject.GetComponent<AudioSource>().Play();
            //heldObject = HoldPosition.transform.GetChild(0).gameObject;
            Rigidbody heldRigidBody = heldObject.GetComponent<Rigidbody>();
            
            // code-review: How would you know if the player can place an
            // object? Its not important to the player interaction if the
            // object can be placed. All the interaction should care about
            // is picking up and dropping things. The logic on the trigger
            // could check if object is currently held, if it is not, then
            // it is placed inside of the trigger and activated.]
            
            //are you on trigger and object can be snapped in place
            if (canPlaceObject)
            {
                //let object know it's time to lerp
                heldObject.GetComponent<Lerping>().lerpToObject = snapObject;
                heldObject.GetComponent<Lerping>().lerpToPosition = snapObject.transform.position;
                heldObject.GetComponent<Lerping>().actionObject = actionObject;
                heldObject.GetComponent<Lerping>().StartLerp(false);
                canPlaceObject = false;
                actionObject = null;
                heldObject.GetComponent<Lerping>().actionObject = null;
            }
            else
            {
                heldRigidBody.constraints = RigidbodyConstraints.None;
                heldObject.transform.parent = null;
                heldRigidBody.useGravity = true;
                heldRigidBody.velocity = playerCam.transform.forward * throwStrength;
                objectCamara.SetActive(false);
                actionObject = null;
                heldObject.GetComponent<Lerping>().actionObject = null;
            }

            heldObject.layer = 10;
            heldObject = null;
            isHolding = false;
            
            snapObject = null;
        }
    }
}
