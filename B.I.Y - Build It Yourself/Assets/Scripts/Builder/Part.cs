using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

delegate float MovementFunction(float s, float t);
public abstract class Part : MonoBehaviour {

    public Material material;
    public int id;
    protected BluePrint bluePrint;

    public Vector2 side;
    public bool broken;

    public int steps;
    protected int currentStep;
    protected List<Button> buttons;
    public List<Button> input;
    public float ownProgress;
    private bool ready;
    public bool finished;
    protected IEnumerator minigame;

    

    private void Awake()
    {



    }

    public void Setup (BluePrint bp, float op)
    {
        currentStep = 0;
        buttons = new List<Button>();
        input = new List<Button>();
        SetVisible(false);
        bluePrint = bp;
        ownProgress = op;
        GenerateButtons(bp.Team);
        GenerateMinigame();


    }

    public float GetOwnProgress()
    {
        return ownProgress;
    }


    public int GetCurrentSteps()
    {
        return currentStep;
    }

    public void ReceiveButton(Button b)
    {

            if (!ready)
            {
            if (input.Count < 3)
            {
                input.Add(b);
                bluePrint.bUI.UpdateButtonInterface(input);
                if (input.Count >= buttons.Count)
                {
                    bool same = true;
                    for (int i = 0; i < input.Count; i++)
                    {
                        if (input[i] != buttons[i])
                            same = false;
                    }
                    ready = same;
                    if (!ready)
                    {
                        bluePrint.bUI.SendTextMessage("Falsche Kombination!");
                    }
                    else
                    {
                        input.Clear();
                        bluePrint.bUI.UpdateButtonInterface(input);
                        StartCoroutine(minigame);
                    }

                }
            }
            else
            {
                bluePrint.bUI.SendTextMessage("Kombofeld voll!");
            }
            }
            else
            {
                bluePrint.bUI.ButtonPressMinigame(b);
            }
        


    }

    public List<Button> GetButtons()
    {
        return buttons.ToList();
    }

    public void Finish()
    {
        currentStep = steps;
        finished = true;
        SetVisible(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        if (minigame != null)
        StopCoroutine(minigame);
        bluePrint.bUI.DestroyMinigame();
    }

    protected abstract void GenerateButtons(Team t);
    protected abstract void GenerateMinigame();

    protected IEnumerator Minigame(int targets, float speed, Button b)
    {
        BuilderUI bUI = bluePrint.bUI;
        bUI.CreateMinigame(targets, b, this);
        float startTime = Time.time;
        MovementFunction mf;
        System.Random r = new System.Random(Time.renderedFrameCount);
        switch (r.Next(0,3))
        {
            case 0:
                mf = MovFunc1;
                break;
            case 1:
                mf = MovFunc2;
                break;
            default:
                mf = MovFunc3;
                break;
        }
        while(bUI.RemainingTargets() > 0)
        {
            bUI.UpdateMinigame(mf(startTime, speed));
            yield return new WaitForEndOfFrame();
        }
        bUI.DestroyMinigame();
        Finish();

    }

    static float MovFunc1(float s, float t)
    {
        return Mathf.PingPong((Time.time - s) * t, 2) - 1;
    }

    static float MovFunc2(float s, float t)
    {
        return Mathf.Sin((Time.time - s) * t);
    }

    static float MovFunc3(float s, float t)
    {
        return ((((Time.time - s)) * t) % 2f) - 1;
    }



    public void DestroyPart()
    {
        ResourceManager.Instance.DecrementMaterial(bluePrint.Team, material);
    }

    public void Abort()
    {
        if (minigame != null)
            StopCoroutine(minigame);
        GenerateMinigame();
        bluePrint.bUI.DestroyMinigame();
        DestroyPart();
        input.Clear();
        ready = false;
        finished = false;
    }

    protected virtual void SetVisible(bool s)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = s;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(s);
        }

    }

    public void Clear()
    {
        input.Clear();
        bluePrint.bUI.UpdateButtonInterface(input);
    }
}
