using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shingle : Part {

    protected override void GenerateButtons(Team t)
    {
        switch (t)
        {
            case Team.A:
                buttons.Add(Button.X);
                buttons.Add(Button.B);
                buttons.Add(Button.Y);
                break;
            case Team.B:
                buttons.Add(Button.X);
                buttons.Add(Button.Y);
                buttons.Add(Button.A);
                break;
        }
    }

    protected override void GenerateMinigame()
    {
        
        minigame = Minigame(1, 1f, Button.A);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
