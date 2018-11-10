using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Brick : Part {


    protected override void GenerateButtons(Team t)
    {
        switch (t)
        {
            case Team.A:
                buttons.Add(Button.X);
                buttons.Add(Button.A);
                buttons.Add(Button.B);
                break;
            case Team.B:
                buttons.Add(Button.Y);
                buttons.Add(Button.B);
                buttons.Add(Button.A);
                break;
        }

    }

    protected override void GenerateMinigame()
    {
        minigame = Minigame(2, 1.5f, Button.X);
    }




    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
