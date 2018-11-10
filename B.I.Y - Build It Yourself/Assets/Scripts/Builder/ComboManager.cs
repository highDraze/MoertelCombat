using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour {

    #region Singleton

    static ComboManager _instance;

    public static ComboManager Instance
    {
        get
        {
            if (_instance == null) _instance = new ComboManager();
            return _instance;
        }
    }

    private ComboManager()
    {
    }

    #endregion

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
