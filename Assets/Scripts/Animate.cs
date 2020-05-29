using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Animation()
    {
        foreach (GameObject batteryHolder in batteryHolders)
        {
          
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

        Debug.Log("in animation");
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Open", hasAllBatteries);
    }
}
