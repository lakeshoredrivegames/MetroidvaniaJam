using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : InteractObject
{

    //TODO: create states for beingHeld, notBeingHeld


    public override void Pickup(GameObject holdPosition)
    {
        Debug.Log("pick battery up. move to: " + holdPosition.name);
        ChangeLayersRecursively(this.transform, "Holding");
        this.GetComponent<Lerping>().oldHeldObject = this.GetComponent<Lerping>().lerpToObject;
        this.GetComponent<Lerping>().lerpToObject = holdPosition;

        if (this.GetComponent<Lerping>().inBatteryHolder == true)
        {
            Debug.Log("has battery");
            GameObject oldHeldObject = this.GetComponent<Lerping>().oldHeldObject;
            if (oldHeldObject)
            {
                // call animation
                //TODO fix this
                Animate animate = oldHeldObject.GetComponent<SnapTrigger>().activateObject.GetComponent<Animate>();
                if (animate)
                {
                    animate.Animation();
                }

                //stop sound
                //TODO fix this
                AudioSource audio = oldHeldObject.GetComponent<AudioSource>();
                if (audio)
                {
                    audio.Stop();
                }
            }
           
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
            heldRigidBody.velocity = forwardDir * throwStrength;
            AudioSource audio = gameObject.GetComponent<AudioSource>();
            if(!audio.isPlaying)
            {
                audio.time = 0.2f;
                audio.Play();
                audio.SetScheduledEndTime(AudioSettings.dspTime + (0.8f - 0.02f));
            }
            this.GetComponent<Lerping>().actionObject = null;
        }
        
    }

}
