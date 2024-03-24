using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : MonoBehaviour, IInteract{
    public void Interact() {
        //TODO: Pooling
        gameObject.SetActive(false);
    }
}