using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickupDist : MonoBehaviour
{
    public int newPickupDistance = 20;
    public AudioSource powerupSound;
    private bool isActivated = false;
    private Vector3 startPos;
    public GameObject powerupText;

    public void Start()
    {
        startPos = transform.position;
    }
    public void Update()
    {
        this.transform.position = new Vector3(transform.position.x, startPos.y +
            Mathf.Sin(4f * Time.time) * 0.08f, transform.position.z);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && isActivated == false)
        {
            GameObject player = col.gameObject;
            Interact playerInteract = player.GetComponent<Interact>();
            powerupSound.Play();
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            playerInteract.raycastDistance = newPickupDistance;
            isActivated = true;
            StartCoroutine(showText());
        }
    }

    IEnumerator showText()
    {
        powerupText.SetActive(true);
        yield return new WaitForSeconds(5);
        powerupText.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
