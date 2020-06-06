using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Interact Interact;

    public State(Interact interact)
    {
        Interact = interact;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }


    public virtual IEnumerator InteractObject(Camera playerCam, float raycastDistance, LayerMask layerMaskPickup)
    {
        yield break;
    }
}
