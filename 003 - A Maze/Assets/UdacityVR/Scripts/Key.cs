using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour 
{
    public GameObject poof;
    public GameObject door;
    public GameObject treasure;
    public static bool keyCollect = false;

	public void OnKeyClicked() {
        Instantiate(poof, transform.position, Quaternion.Euler(-90, 0, 0));
        door.GetComponent<Door>().Unlock();
        treasure.GetComponent<Treasure>().Unlock();
        keyCollect = true;
        Destroy(gameObject);
    }

}
