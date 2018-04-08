using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour {
    public bool lockedTreasure = true;
    public bool openingTreasure = false;
    public Animator animTreasure;
    public GameObject poof;
    public GameObject chest;

    private void Update() {
        if (openingTreasure == true) {
            animTreasure.SetBool("opening", true);
        }
    }

    public void OnTreasureClicked() {
        if (lockedTreasure == false) {
            Vector3 pos = new Vector3(chest.transform.position.x, chest.transform.position.y, chest.transform.position.z);
            Quaternion rot = new Quaternion(chest.transform.rotation.x, chest.transform.rotation.y, chest.transform.rotation.z, chest.transform.rotation.w);
            openingTreasure = true;
            Instantiate(poof, pos + new Vector3(2f, 0f, 0f), rot);
            Instantiate(poof, pos + new Vector3(-2f, 0f, 0f), rot);
        }
    }

    public void Unlock() {
        lockedTreasure = false;
    }
}
