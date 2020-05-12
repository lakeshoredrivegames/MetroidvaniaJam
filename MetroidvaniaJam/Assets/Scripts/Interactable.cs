using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    
    [SerializeField] private float throwStrength;
    [SerializeField] private GameObject HoldPosition;

    public bool canPlaceObject;
    public GameObject snapObject;
    public GameObject heldObject;

    private Camera playerCam;
    private Camera objectCam;
    private GameObject objectCamara;
    private bool isHolding;
    
    void Start()
    {
        isHolding = false;
        playerCam = Camera.main;
        objectCamara = GameObject.Find("ObjectCamera");
        objectCamara.SetActive(false);
        objectCam = objectCamara.GetComponent<Camera>();
        canPlaceObject = false;

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) { Interact(); }
    }

    private void Interact()
    {
        Debug.Log("interacting");
        if (isHolding == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 10.0f))
            {
                Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * hit.distance, Color.red, 20);
                Debug.Log("hit name: " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "Pickup")
                {
                    //GetComponent<AudioSource>().Play();
                    heldObject = hit.collider.gameObject;
                    hit.collider.transform.SetParent(HoldPosition.transform);
                    hit.collider.transform.localPosition = Vector3.zero;
                    hit.collider.transform.localRotation = Quaternion.identity;
                    hit.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                    isHolding = true;
                    objectCamara.SetActive(true);

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
                heldObject.transform.SetParent(snapObject.transform);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;
                heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                heldObject.tag = "Untagged";
                canPlaceObject = false;
            }
            else
            {
                heldRigidBody.constraints = RigidbodyConstraints.None;
                heldObject.transform.parent = null;
                heldRigidBody.velocity = playerCam.transform.forward * throwStrength;
            }

            heldObject = null;
            isHolding = false;
            objectCamara.SetActive(false);
            snapObject = null;
        }
    }
}
