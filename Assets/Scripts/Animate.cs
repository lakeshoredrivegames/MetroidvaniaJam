using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code-review: This file needs a description of what it does.

public class Animate : MonoBehaviour
{
    private ObjectController objectController;
    private GameObject[] batteryHolders;
    public bool hasAllBatteries;


    // Start is called before the first frame update
    void Start()
    {
        objectController = GetComponent<ObjectController>();
        batteryHolders = objectController.batteryHolders;
        hasAllBatteries = false;
    }

    // code-review: Remove this method.
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Animation()
    {
        foreach (GameObject batteryHolder in batteryHolders)
        {
          // code-review: Here you have two code paths, one when the
          // the holder has all of the batteries, and one when it does
          // not have all of the batteries.
          //
          // This can be simplified by creating a animate state.
          // Animate will then hold an AnimateState where the state could be
          // the two options here.
          //
          // code-review: Searching through this batteryHolder is more complicated
          // than it could be. I would create a list of bools from the SnapTriggers.
          // When you check if any of the triggers do not have a battery it then
          // gets simplified to
          //    if (batteriesInTrigger.Contains(false) { ... }
            if (batteryHolder.GetComponent<SnapTrigger>().hasBattery)
            {
                Debug.Log("true batteries: " + batteryHolder.name);
                hasAllBatteries = true;
            }
            else
            {
                Debug.Log("false batteries: " + batteryHolder.name);
                hasAllBatteries = false;
                break;
            }
        }

        // code-review: Why is this an animation? You could move the door using code.
        // Also, Does every animated object have an 'Open' variable?
        Debug.Log("in animation");
        // code-review: This should be allocated when the object is created. Unless the
        // animator will be changed at run-time.
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Open", hasAllBatteries);
    }
}
