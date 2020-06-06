using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : InteractObject
{

    //TODO: create states for beingHeld, notBeingHeld
    ParticleSystem exp;

    float triggerTimer = 0;

    void Start()
    {
        exp = GetComponent<ParticleSystem>();
    }
    public override void Pickup(GameObject holdPosition)
    {
        ChangeLayersRecursively(this.transform, "Holding");
        this.GetComponent<Lerping>().oldHeldObject = this.GetComponent<Lerping>().lerpToObject;
        this.GetComponent<Lerping>().lerpToObject = holdPosition;

        if (this.GetComponent<Lerping>().inBatteryHolder == true)
        {

            this.GetComponent<Lerping>().oldHeldObject.GetComponent<SnapTrigger>().activateObject.GetComponent<Animate>().Animation();
            this.GetComponent<Lerping>().inBatteryHolder = false;

        }

        this.GetComponent<Lerping>().StartLerp(true);

        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    public override void Putdown(Vector3 forwardDir, float throwStrength)
    {
        Debug.Log("determine where to put down object");
        ChangeLayersRecursively(this.transform, "Pickup");
        Rigidbody heldRigidBody = this.GetComponent<Rigidbody>();



        GetComponent<AudioSource>().Stop();
        GameObject batteryHolder = this.gameObject.transform.GetChild(0).gameObject;
        //batteryHolder.GetComponent<AudioSource>().Stop();
        //TODo - need to fix this 
        batteryHolder.tag = "Pickup";


        //are you on trigger and object can be snapped in place
        if (canPlaceObject)
        {

            this.GetComponent<Lerping>().StartLerp(false);
            canPlaceObject = false;

        }
        else
        {
            heldRigidBody.constraints = RigidbodyConstraints.None;
            this.transform.parent = null;
            heldRigidBody.useGravity = true;
            //TODO: create throw state            
            //heldRigidBody.velocity = forwardDir * throwStrength;
            this.GetComponent<Lerping>().actionObject = null;
        }

    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Vine")
        {
            Debug.Log("start chainsaw");
            triggerTimer = 0;
            GameObject batteryHolder = this.gameObject.transform.GetChild(0).gameObject;
            batteryHolder.GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().Play();
        }
    }   

    void OnTriggerStay(Collider col)
    {

        if(col.gameObject.tag == "Vine")
        {
            
            exp.Play();
           
            triggerTimer += Time.deltaTime;
            if (triggerTimer > 11)
            {
                Destroy(col.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {

        if(col.gameObject.tag == "Vine")
        {
            exp.Play();
            GetComponent<AudioSource>().Stop();
            GameObject batteryHolder = this.gameObject.transform.GetChild(0).gameObject;
            batteryHolder.GetComponent<AudioSource>().Play();
            
        }
    }

}
