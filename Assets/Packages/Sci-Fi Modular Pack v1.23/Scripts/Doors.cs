using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Doors : MonoBehaviour
{

    public AudioClip doorSound;
    //  AudioSource audio;
    new AudioSource audio;
    Animator animator;
    bool doorOpen;
    public Serrure _serrure;
    

    // Use this for initialization
    void Start()
    {
        doorOpen = false;
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (_serrure._accessGranted)
        {

            if (col.gameObject.tag == "Player")
            {
                doorOpen = true;
                DoorControl("Open");
                audio.PlayOneShot(doorSound, 0.3f);
            }
        }


        if (col.gameObject.tag == "Enemy")
        {
            doorOpen = true;
            DoorControl("Open");
            audio.PlayOneShot(doorSound, 0.5f);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (doorOpen)
        {
            doorOpen = false;
            DoorControl("Close");
            audio.PlayOneShot(doorSound, 0.3f);
        }
    }

    void DoorControl(string direction)
    {
        animator.SetTrigger(direction);
    }
}
