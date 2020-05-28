using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRespawn : MonoBehaviour
{
    [SerializeField]
    private CharacterController playerController;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Transform respawnPoint;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        playerController.enabled = false;
        Player.transform.position = respawnPoint.transform.position;
        playerController.enabled = true;
    }
}