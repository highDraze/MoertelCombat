using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Phase { Mauerwerk, Dachstuhl, Dach, Bauelemente}
[System.Serializable]
public class BluePrint : MonoBehaviour
{

    public BuilderUI bUI;
    public Builder bI;
    float progress;
    public Queue<Part>[] parts;
    Part activePart;
    int currentPhase;
    public BluePrint other;

    public Team Team
    {
        get
        {
            return bI.team;
        }
    }


    private void Awake()
    {
        currentPhase = 0;
        progress = 0f;

        parts = new Queue<Part>[Enum.GetValues(typeof(Phase)).Length];

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = new Queue<Part>();
        }

        

    }
    void Start()
    {
        for (int n = 0; n < parts.Length; n++)
        {
            int phaseSteps = 0;
            for (int i = 0; i < transform.GetChild(n).childCount; i++)
            {
                Part p = transform.GetChild(n).GetChild(i).GetComponent<Part>();
                p.gameObject.SetActive(false);
                p.id = i;
                phaseSteps += p.steps;
                parts[n].Enqueue(p);
            }
            foreach (Part p in parts[n])
            {
                p.Setup(this, (float)p.steps / (float)phaseSteps / 4f);
            }
        }
        activePart = parts[currentPhase].Peek();
        bI.MoveNext(GetNextPart());
    }

    void Update()
    {
        if (activePart != null)
        {
            bUI.UpdateProgress(progress + activePart.GetOwnProgress() * ((float)activePart.GetCurrentSteps() / (float)activePart.steps), currentPhase);
            ResourceManager.Instance.UpdateProgress(bI.team, progress);
            //bUI.UpdateButtonInterface(progress: activePart.GetCurrentSteps());
            if (activePart.finished)
            {
                ResourceManager.Instance.DecrementMaterial(bI.team, activePart.material);
                progress += activePart.GetOwnProgress();
                bUI.Clear();
                bI.MoveNext(GetNextPart());
            }
        }

    }

    public Part GetNextPart()
    {
        bUI.UpdatePhase(currentPhase);
        if (parts[currentPhase].Count > 0)
        {
            activePart = parts[currentPhase].Peek();
            activePart.gameObject.SetActive(true);
            //bUI.UpdateButtonInterface(activePart.GetButtons());
            bUI.UpdatePhase(currentPhase);
            return parts[currentPhase].Dequeue();

        }
        else if (currentPhase + 1 < parts.Length)
        {
            currentPhase++;
            activePart = parts[currentPhase].Peek();
            activePart.gameObject.SetActive(true);
            //bUI.UpdateButtonInterface(activePart.GetButtons());
            bUI.UpdatePhase(currentPhase);
            return parts[currentPhase].Dequeue();
        }
        else
        {
            bUI.SendTextMessage("Finished");
            bUI.UpdateProgress(1, 4);
            ResourceManager.Instance.UpdateProgress(bI.team, 1);
            GathererUI.Instance.SendTextMessage(bI.team,"Finished");
            this.enabled = false;
            other.enabled = false;
            bI.enabled = false;
            other.bI.enabled = false;
            GathererUI.Instance.Finish();
            return null;
        }

        

    }

    public void SendInput(Button b)
    {
        if (ResourceManager.Instance.GetMaterial(bI.team, activePart.material) > 0)
            activePart.ReceiveButton(b);
        else
            bUI.SendTextMessage("Nicht genügend " + activePart.material.ToString());



    }

    public void Skip()
    {
        activePart.Finish();
        progress += activePart.GetOwnProgress();
        bUI.Clear();
        bI.MoveNext(GetNextPart());
    }

    public void ClearCombo()
    {
        activePart.Clear();
    }

}
