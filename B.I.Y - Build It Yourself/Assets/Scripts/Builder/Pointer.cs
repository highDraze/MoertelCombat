using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

    private GameObject target;
    public Target Target
    {
        get
        {
            if (target != null)
            return target.GetComponent<Target>();
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Target>() != null)
            target = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
