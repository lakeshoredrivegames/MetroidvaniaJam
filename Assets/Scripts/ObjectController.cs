using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code-review: This object is just for holding an array of gameobjects?
// I would remove it and place this data elsewhere.
// The naming of 'batteryHolders' is also very specific.
// I would name it 'triggerActors' or 'triggerLocations'.

public class ObjectController : MonoBehaviour
{
    public GameObject[] batteryHolders;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
