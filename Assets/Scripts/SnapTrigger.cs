using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{

    public GameObject activateObject;
    private GameObject player;
    private bool played;
    public AudioClip SoundToPlay;
    private BoxCollider boxCollider;
    public bool hasBattery = false;
    private Quaternion oldRotation;

    // Start is called before the first frame update
    void Start()
    {
        played = false;
        player = GameObject.Find("Player");
        boxCollider = GetComponent<BoxCollider>();

        if(activateObject == null)
        {
            Debug.Log("Door Object is empty. Add a door holder object to the trigger object named " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Battery")
        {
            //if (col.gameObject.GetComponent<Interact>().heldObject == null)
            //{
            //rotate held object
                oldRotation = col.gameObject.transform.rotation;
                col.gameObject.transform.rotation = transform.rotation;
                //move held object to right in front of trigger to middle of box collider
                //col.gameObject.transform.position = boxCollider.transform.position;

                if (played == false)
                {
                    GetComponent<AudioSource>().PlayOneShot(SoundToPlay);
                    played = true;
                }
            //}
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "Battery" && !hasBattery)
        {
            //Debug.Log("stay trigger");
            col.gameObject.GetComponent<InteractObject>().canPlaceObject = true;
            col.gameObject.GetComponent<Lerping>().lerpToObject = this.gameObject;
            col.gameObject.GetComponent<Lerping>().actionObject = activateObject;
            //col.gameObject.GetComponent<Lerping>().actionObject = activateObject;


        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Battery")
        {
           // Debug.Log("exit trigger");
            col.gameObject.GetComponent<InteractObject>().canPlaceObject = false;
            col.gameObject.GetComponent<Lerping>().actionObject = null;
            col.gameObject.GetComponent<Lerping>().lerpToObject = null;
            col.gameObject.transform.rotation = oldRotation;


        }
    }
}
