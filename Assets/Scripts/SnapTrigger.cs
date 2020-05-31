using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code-review: What does snap trigger do? Please write what this file does.

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
        // code-review: Just make it public and plug it in via inspector.
        player = GameObject.Find("Player");
        boxCollider = GetComponent<BoxCollider>();

        if(doorObject == null)
        {
            Debug.Log("Door Object is empty. Add a door holder object to the trigger object named " + gameObject.name);
        }
    }
    
    // code-review: Remove unused functions

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        // code-review: This can fall apart very easily.
        // You should be checking a tag here.
        if (col.gameObject.name == "Battery")
        {
            // code-review: Remove all commented code.
            //if (col.gameObject.GetComponent<Interact>().heldObject == null)
            //{
                //rotate held object
                col.gameObject.transform.rotation = transform.rotation;
                //move held object to right in front of trigger to middle of box collider
                //col.gameObject.transform.position = boxCollider.transform.position;

                // code-review: Design this in a way where it does not matter
                // if the sound played already. The collided object should
                // have some state indicating that it is 'activated'.
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
        // code-review: Same comment as before.
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
        // code-review: Same comment as before.
        if (col.gameObject.name == "Battery")
        {
            Debug.Log("exit trigger");
            player.GetComponent<Interact>().canPlaceObject = false;
            

        }
    }
}
