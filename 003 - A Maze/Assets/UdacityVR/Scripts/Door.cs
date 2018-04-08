using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public bool locked = true;
    public bool opening = false;
    public AudioSource audioSource;
    public AudioClip lockedDoor;
    public AudioClip openingDoor;
    public Animator animDoor;
    public BoxCollider boxCollider;

    private void Update() {
        if(opening == true) {
            animDoor.SetBool("opening", true);
        }
    }

    public void OnDoorClicked() {
        if (locked == false) {
            opening = true;
            audioSource.clip = openingDoor;
            audioSource.Play();
            boxCollider.enabled = false;
        } else {
            audioSource.clip = lockedDoor;
            audioSource.Play();
        }
    }

    public void Unlock() {
        locked = false;
        audioSource.clip = openingDoor;
        audioSource.Play();
    }
}
