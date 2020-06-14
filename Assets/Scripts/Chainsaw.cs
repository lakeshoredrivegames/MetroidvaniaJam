
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : InteractObject
{

    //TODO: create states for beingHeld, notBeingHeld
    ParticleSystem exp;
    public AudioClip audioChainsawSlashing;
    public AudioClip audioChainsawIdle;
    public float timeToCut;
    AudioSource audio;
    public Animator animator;

    private bool isTurnedOn;
    float triggerTimer = 0;

    void Start()
    {
        exp = GetComponent<ParticleSystem>();
        audio = GetComponent<AudioSource>();
        timeToCut = 5;
        isTurnedOn = false;
        if(!animator)
        {

            Debug.Log("set animator on chainsaw to the saw");
        }
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

        //if has battery start chainsaw
        if (this.gameObject.transform.GetChild(0).gameObject.GetComponent<SnapTrigger>().hasBattery)
        {
            isTurnedOn = true;
            audio.Play();
            //start animation

            animator.SetBool("On", true);
        }

        this.GetComponent<Lerping>().StartLerp(true);

        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    public override void Putdown(Vector3 forwardDir, float throwStrength)
    {
        isTurnedOn = false;
        Debug.Log("determine where to put down object");
        ChangeLayersRecursively(this.transform, "Pickup");
        Rigidbody heldRigidBody = this.GetComponent<Rigidbody>();


        audio.Stop();
        animator.SetBool("On", false);
        //GetComponent<AudioSource>().Stop();
        GameObject batteryHolder = this.gameObject.transform.GetChild(0).gameObject;
        //batteryHolder.GetComponent<AudioSource>().Stop();
        //TODo - need to fix this 
        batteryHolder.layer = LayerMask.NameToLayer("Pickup");


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

    //TODO: move audio to separate method
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Vine" && isTurnedOn)
        {
            Debug.Log("start chainsaw");
            triggerTimer = 0;

            audio.clip = audioChainsawSlashing;
            audio.loop = false;
            audio.Play();
        }
    }

    void OnTriggerStay(Collider col)
    {

        if (col.gameObject.tag == "Vine" && isTurnedOn)
        {

            exp.Play();

            triggerTimer += Time.deltaTime;
            if (triggerTimer > timeToCut)
            {
                exp.Stop();
                audio.Stop();
                audio.clip = audioChainsawIdle;
                audio.loop = true;
                audio.Play();
                Destroy(col.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Vine" && isTurnedOn)
        {
            exp.Stop();
            audio.Stop();
            audio.clip = audioChainsawIdle;
            audio.loop = true;
            audio.Play();
            Debug.Log("stop chopping vine");

        }
    }

}

