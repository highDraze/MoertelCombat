using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBeam : Part {
    protected override void GenerateButtons(Team t)
    {
        switch (t)
        {
            case Team.A:
                buttons.Add(Button.Y);
                buttons.Add(Button.A);
                buttons.Add(Button.Y);
                break;
            case Team.B:
                buttons.Add(Button.B);
                buttons.Add(Button.B);
                buttons.Add(Button.X);
                break;
        }

    }

    protected override void GenerateMinigame()
    {
        minigame = Minigame(2, 2f, Button.B);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
