using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerping : MonoBehaviour
{

    private bool isLerping = false;
    public float lerpTimer = 0;
    public float lerpSeconds = 3;
    private Vector3 startPosition;
    private Quaternion startRotation;
    public GameObject lerpToObject;
    public Vector3 lerpToPosition;
    public Quaternion lerpToRotation;
    private GameObject objectCamera;
    private GameObject mainCamera;
    private GameObject player;
    private bool activateCam = false;
    //private Camera objectCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        objectCamera = GameObject.Find("ObjectCamera");
        player = GameObject.Find("Player");
       // objectCam = objectCamara.GetComponent<Camera>();
    }

    public void StartLerp(bool showCam)
    {
        mainCamera.GetComponent<PlayerLook>().enabled = false;
        player.GetComponent<PlayerMove>().enabled = false;
        isLerping = true;
        lerpTimer = 0;
        startPosition = transform.position;
        startRotation = transform.rotation;
        lerpToRotation = lerpToObject.transform.rotation;
        transform.parent = null;
        if (showCam)
        {
            activateCam = true;
            objectCamera.SetActive(true);
        }
    }

    public void StopLerp()
    {
        isLerping = false;
        transform.position = lerpToPosition;
        transform.rotation = lerpToRotation;
        transform.SetParent(lerpToObject.transform);
        if(!activateCam)
            objectCamera.SetActive(false);
        player.GetComponent<PlayerMove>().enabled = true;
        mainCamera.GetComponent<PlayerLook>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (isLerping)
        {
            lerpTimer += Time.deltaTime;
            if (lerpTimer > lerpSeconds)
            {
                StopLerp();
            }
            else
            {
                float ratio = lerpTimer / lerpSeconds;
                transform.position = Vector3.Lerp(startPosition, lerpToPosition, ratio);
                transform.rotation = Quaternion.Slerp(startRotation, lerpToRotation, ratio);
            }
        }
    }
}
