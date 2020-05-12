using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{

    [SerializeField] private GameObject placeObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            Debug.Log("fire trigger");
            col.gameObject.GetComponent<Interactable>().canPlaceObject = true;
            col.gameObject.GetComponent<Interactable>().snapObject = placeObject;
            
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            Debug.Log("exit trigger");
            col.gameObject.GetComponent<Interactable>().canPlaceObject = false;
            
        }
    }
}
