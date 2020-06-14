using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    private ObjectController objectController;
    private GameObject[] batteryHolders;
    public bool hasAllBatteries;
    public bool openDoor = false;

    public AudioSource openDoorAudio;
    public AudioSource closeDoorAudio;

    // Start is called before the first frame update
    void Start()
    {
        objectController = GetComponent<ObjectController>();
        batteryHolders = objectController.batteryHolders;
        hasAllBatteries = false;
        if (openDoor)
        {
            Animation();
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Animation()
    {
        foreach (GameObject batteryHolder in batteryHolders)
        {
          
            if (batteryHolder.GetComponent<SnapTrigger>().hasBattery)
            {
                Debug.Log("true batteries: " + batteryHolder.name);
                hasAllBatteries = true;
                if (!openDoorAudio.isPlaying)
                {
                    openDoorAudio.time = 0.01f;
                    openDoorAudio.Play();
                    openDoorAudio.SetScheduledEndTime(AudioSettings.dspTime + (1.5f - 0.01f));
                }
            }
            else
            {
                Debug.Log("false batteries: " + batteryHolder.name);
                hasAllBatteries = false;
                if (!closeDoorAudio.isPlaying)
                {
                    closeDoorAudio.time = 3f;
                    closeDoorAudio.Play();
                    closeDoorAudio.SetScheduledEndTime(AudioSettings.dspTime + (4.6f - 3f));
                }
                break;
            }
        }

        Debug.Log("in animation");
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Open", hasAllBatteries);
        
    }
}
