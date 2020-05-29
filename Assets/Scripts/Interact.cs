using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        isHolding = false;
        playerCam = Camera.main;
        objectCamara = GameObject.Find("ObjectCamera");
        objectCamara.SetActive(true);
        objectCam = objectCamara.GetComponent<Camera>();
        canPlaceObject = false;
        layerMask = LayerMask.GetMask("Pickup");

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) { Interacting(); }
    }
    

    


    private void Interacting()
    {
        Debug.Log("interacting");
        if (isHolding == false)
        {

            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, layerMask))
            {
                Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * hit.distance, Color.red, 20);
                Debug.Log("hit name: " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "Pickup")
                {
                    GetComponent<AudioSource>().Play();
                    heldObject = hit.collider.gameObject;
                    heldObject.layer = 11;
                    
                    //check if battery is in a holder
                    //if so, reduce battery holder count 
                    if(heldObject.GetComponent<Lerping>().inBatteryHolder == true)
                    {
                        heldObject.GetComponent<Lerping>().inBatteryHolder = false;
                        heldObject.GetComponent<Lerping>().SetOldHeldObject();
                        heldObject.GetComponent<Lerping>().oldHeldObject.GetComponent<SnapTrigger>().hasBattery = false;
                        heldObject.GetComponent<Lerping>().oldHeldObject.GetComponent<SnapTrigger>().doorObject.GetComponent<Animate>().Animation();
                    }
                    

                    //let object know it's time to lerp
                    heldObject.GetComponent<Lerping>().lerpToObject = HoldPosition;
                    heldObject.GetComponent<Lerping>().lerpToPosition = HoldPosition.transform.position;
                    heldObject.GetComponent<Lerping>().StartLerp(true);

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
            //transform.parent.gameObject.GetComponent<AudioSource>().Play();
            //heldObject = HoldPosition.transform.GetChild(0).gameObject;
            Rigidbody heldRigidBody = heldObject.GetComponent<Rigidbody>();
            

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
