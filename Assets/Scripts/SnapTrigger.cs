using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{

    public GameObject doorObject;
    private GameObject player;
    private bool played;
    public AudioClip SoundToPlay;
    private BoxCollider boxCollider;
    public bool hasBattery = false;

    // Start is called before the first frame update
    void Start()
    {
        played = false;
        player = GameObject.Find("Player");
        boxCollider = GetComponent<BoxCollider>();

        if(doorObject == null)
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
        if (col.gameObject.name == "Battery")
        {
            //Debug.Log("fire trigger");
            player.GetComponent<Interact>().canPlaceObject = true;
            player.GetComponent<Interact>().snapObject = this.gameObject;
            player.GetComponent<Interact>().actionObject = doorObject;
            col.gameObject.GetComponent<Lerping>().actionObject = doorObject;


        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Battery")
        {
            Debug.Log("exit trigger");
            player.GetComponent<Interact>().canPlaceObject = false;
            

        }
    }
}
