using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    private void Awake()
    {
        InheritedAwake();
    }

    // Update is called once per frame
    void Update () {
        InheritedUpdate();
	}

    public abstract void ReceiveInput(Gatherer g, Button b);
    protected virtual void InheritedUpdate() { }
    protected virtual void InheritedAwake() { }

}
