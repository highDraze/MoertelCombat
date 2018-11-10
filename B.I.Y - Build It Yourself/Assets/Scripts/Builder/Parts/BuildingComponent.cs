using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingComponent : Part {
    protected override void GenerateButtons(Team t)
    {
        switch (t)
        {
            case Team.A:
                buttons.Add(Button.A);
                buttons.Add(Button.A);
                buttons.Add(Button.X);
                break;
            case Team.B:
                buttons.Add(Button.X);
                buttons.Add(Button.Y);
                buttons.Add(Button.Y);
                break;
        }
    }

    protected override void GenerateMinigame()
    {
        minigame = Minigame(4, 0.7f, Button.Y);
    }


    protected override void SetVisible(bool s)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(s);
        }

    }
}
