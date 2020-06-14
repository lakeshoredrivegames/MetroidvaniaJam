using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginState : State
{
    public BeginState(Interact interact) : base(interact)
    {

    }



    public override IEnumerator Start()
    {
        Interact.isHolding = false;
        Debug.Log("in start of begin");
        Interact.SetState(new NoObjectState(Interact));
        yield break;
    }
}
