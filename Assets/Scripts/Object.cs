using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An Object can be picked up, carried, dropped, and activated by the player
// Object pickup is based on pickup tag


public abstract class InteractObject : MonoBehaviour
{
//    private Interact interact;
    public GameObject interactObject;
    public bool canPlaceObject;
    public abstract void Pickup(GameObject position);
    public abstract void Putdown(Vector3 forwardDir, float throwStrength);
    public static void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
        }
    }
}

