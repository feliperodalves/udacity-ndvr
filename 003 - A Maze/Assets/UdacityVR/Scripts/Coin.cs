using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    public GameObject poof;

    public void OnCoinClicked() {
        Instantiate(poof, transform.position, Quaternion.Euler(-90, 0, 0));
        DestroyObject(gameObject);
    }

}
