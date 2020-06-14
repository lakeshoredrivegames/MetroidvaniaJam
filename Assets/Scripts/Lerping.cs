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
    public GameObject oldHeldObject;
    public GameObject actionObject;
    //public Vector3 lerpToPosition;
    public Quaternion lerpToRotation;
    private GameObject objectCamera;
    private GameObject mainCamera;
    private GameObject player;
    public bool activateCam = false;
    public bool inBatteryHolder = false;
    //private Camera objectCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        objectCamera = GameObject.Find("ObjectCamera");
        player = GameObject.Find("Player");
       // objectCam = objectCamara.GetComponent<Camera>();
    }

    public void SetOldHeldObject()
    {
        oldHeldObject = lerpToObject;
    }

    public void StartLerp(bool showCam)
    {
        Debug.Log("Start lerp");
        //mainCamera.GetComponent<PlayerLook>().enabled = false;
        //player.GetComponent<PlayerMove>().enabled = false;
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
        Debug.Log("end Start lerp");
    }

    public void StopLerp()
    {
        Debug.Log("Stop lerp");
        isLerping = false;
        transform.position = lerpToObject.transform.position;
        transform.rotation = lerpToRotation;
        transform.SetParent(lerpToObject.transform);
        if (!activateCam)
        {
            objectCamera.SetActive(false);
            
        }

        if(lerpToObject.GetComponent<AudioSource>() != null)
            lerpToObject.GetComponent<AudioSource>().Play();
        else
            player.GetComponent<AudioSource>().Play();

        if (oldHeldObject != null)
        {
            SnapTrigger snapTrigger = oldHeldObject.GetComponent<SnapTrigger>();
            if (snapTrigger)
            {
                snapTrigger.hasBattery = !snapTrigger.hasBattery;
                Debug.Log("oldHeldObject: " + oldHeldObject.name);
                Debug.Log("remove battery");
            
                
                if (snapTrigger.activateObject.GetComponent<Animate>())
                {
                    snapTrigger.activateObject.GetComponent<Animate>().Animation();
                }
            }

            oldHeldObject = null;

        }
        if (actionObject != null)
        {

            //actionObject.GetComponent<ObjectController>().numBatteriesUsed++;
            SnapTrigger snapTrigger = lerpToObject.GetComponent<SnapTrigger>();
            if (snapTrigger)
            {
                snapTrigger.hasBattery = !snapTrigger.hasBattery;
                inBatteryHolder = !inBatteryHolder;
            }
            Debug.Log("try to animate");
            if (actionObject.GetComponent<Animate>())
            {
                actionObject.GetComponent<Animate>().Animation();
            }
            actionObject = null;
            player.GetComponent<Interact>().actionObject = null;
            player.GetComponent<Interact>().snapObject = null;
        }


        GetComponent<Rigidbody>().useGravity = false;

        //player.GetComponent<PlayerMove>().enabled = true;
        //mainCamera.GetComponent<PlayerLook>().enabled = true;
        

    }

    // Update is called once per frame
    void Update()
    {

        if (isLerping && lerpToObject)
        {
            lerpTimer += Time.deltaTime;
            if (lerpTimer > lerpSeconds)
            {
                StopLerp();
            }
            else
            {
                float ratio = lerpTimer / lerpSeconds;
                transform.position = Vector3.Lerp(startPosition, lerpToObject.transform.position, ratio);
                transform.rotation = Quaternion.Slerp(startRotation, lerpToRotation, ratio);
            }
        }
    }
}
