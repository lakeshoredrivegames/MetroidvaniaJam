using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoObjectState : State
{
    public NoObjectState(Interact interact) : base(interact)
    {

    }

    public override IEnumerator Start()
    {
        Interact.isHolding = false;
        Debug.Log("in start of no object");        
        yield return new WaitForSeconds(1f);
    }

    public override IEnumerator InteractObject(Camera playerCam, float raycastDistance, LayerMask layerMaskPickup)
    {
        Debug.Log("no object so find object");
        bool foundObject = false;

        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, layerMaskPickup))
        {
            
            if (hit.collider.gameObject.GetComponent<InteractObject>() != null)
            {
                foundObject = true;
                Debug.Log("found an interact object");
                Interact.heldObject = hit.collider.gameObject.GetComponent<InteractObject>();
                //Interact.heldObject.interactObject = hit.collider.gameObject.GetComponent<InteractObject>;
            }

            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * hit.distance, Color.red, 20);
            Debug.Log("hit name: " + hit.collider.gameObject.name);
        }
        
        if(foundObject)
            Interact.SetState(new HasObjectState(Interact));

        yield break;
    }

}
