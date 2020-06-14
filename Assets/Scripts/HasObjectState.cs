using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HasObjectState : State
{
    public HasObjectState(Interact interact) : base(interact)
    {

    }

    public override IEnumerator Start()
    {
        Interact.isHolding = true;
        Debug.Log("in start of has object");
        Interact.heldObject.Pickup(Interact.HoldPosition);
        yield break;

        //Interact.SetState(new NoObjectState(Interact));

    }

    public override IEnumerator InteractObject(Camera playerCam, float raycastDistance, LayerMask layerMaskPickup)
    {
        Debug.Log("has object so now what to do");
        Interact.heldObject.Putdown(Interact.playerCam.transform.forward, Interact.throwStrength);
        Interact.SetState(new NoObjectState(Interact));
        yield break;
    }
 
}
