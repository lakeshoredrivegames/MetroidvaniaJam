using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{

    [SerializeField] private GameObject placeObject;
    private bool played;
    public AudioClip SoundToPlay;
    // Start is called before the first frame update
    void Start()
    {
        played = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            if (col.gameObject.GetComponent<Interact>().heldObject == null)
            {
                if (played == false)
                {
                    GetComponent<AudioSource>().PlayOneShot(SoundToPlay);
                    played = true;
                }
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            //Debug.Log("fire trigger");
            col.gameObject.GetComponent<Interact>().canPlaceObject = true;
            col.gameObject.GetComponent<Interact>().snapObject = placeObject;
            col.gameObject.GetComponent<Interact>().actionObject = this.gameObject;
            
            
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            Debug.Log("exit trigger");
            col.gameObject.GetComponent<Interact>().canPlaceObject = false;
            
        }
    }
}
