using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour {

    public GameObject[] buttons;
    List<GameObject> visibleButtons;
    public Vector2 screenSize;
    public Team team;
    public Text progress;
    public Text phase;
    public Text storage;
    public GameObject instructionSheet;
    public Text message;
    private IEnumerator messageActive;

    public GameObject minigame;
    public GameObject minigameTarget;
    private Part minigamePart;
    private int remainingTargets;
    private Button mgButton;
    public Pointer minigamePointer;
    private GameObject minigameButton;
    private List<GameObject> targets;

    private void Awake()
    {
        visibleButtons = new List<GameObject>();
        targets = new List<GameObject>();
        message.color = new Color(1f, 0f, 0f, 0f);
        messageActive = ShowTextMessage(message);
    }
    private void Start()
    {

    }
    public void Clear()
    {
        foreach (GameObject g in visibleButtons)
        {
            GameObject.Destroy(g);
        }
        visibleButtons.Clear();

    }
	

	void Update ()
    {
        storage.text = "Lager\nZiegel: " + ResourceManager.Instance.GetMaterial(team,Material.Ziegel) + 
                        " Schindeln: " + ResourceManager.Instance.GetMaterial(team, Material.Schindel) +
                        "\nDachstuhlbalken: " + ResourceManager.Instance.GetMaterial(team, Material.Balken) +
                        " Bauelemente: " + ResourceManager.Instance.GetMaterial(team, Material.Bauelement);
	}

    public void UpdatePhase(int p)
    {
        phase.text = "Phase " + (p + 1) + " : " + Enum.GetNames(typeof(Phase))[p];
    }

    public void UpdateProgress(float progress, int phase)
    {
        this.progress.text = "Gesamt: " + string.Format("{0:0.##\\%}", progress * 100);
        this.progress.text += "\nPhase: " + string.Format("{0:0.##\\%}", ((progress * 100) - 25 * phase) * 4);

    }

    public void UpdateButtonInterface(List<Button> buttons = null, int progress = 0)
    {
        Clear();
        if (buttons != null)
        {
            foreach(Button b in buttons)
            {
                AddButton(b);
            }
        }
        //float xOffset = 400 / (visibleButtons.Count + 1);
        for (int i = 0; i < visibleButtons.Count; i++)
        {
            RectTransform r = visibleButtons[i].GetComponent<RectTransform>();
            r.anchoredPosition = new Vector2( i * 110 - 110, 0);
            //visibleButtons[i].GetComponent<RectTransform>().rect.Set((i + 1) * xOffset + sideOffset, 0, 100f, 100f);
            visibleButtons[i].GetComponent<RawImage>().color = (progress > i) ? Color.gray : Color.white;


        }
    }

    public void AddButton (Button b)
    {
        GameObject nButton = GameObject.Instantiate(buttons[(int)b], this.transform);
        visibleButtons.Add(nButton);
    }

    public void ShowInstructionSheet()
    {
        //RectTransform r = instructionSheet.GetComponent<RectTransform>();
        instructionSheet.transform.localPosition = new Vector3((team == Team.A) ? -84 : 84, 0,0);
    }

    public void HideInstructionSheet()
    {
        //RectTransform r = instructionSheet.GetComponent<RectTransform>();
        instructionSheet.transform.localPosition = new Vector3((team == Team.A) ? -318 : 318, 0,0);
    }

    public void SendTextMessage(string m)
    {
        StopCoroutine(messageActive);
        message.text = m;
        messageActive = ShowTextMessage(message);
        StartCoroutine(messageActive);
    }

    IEnumerator ShowTextMessage(Text t)
    {
        Color c = t.color;
        c.a = 1f;
        t.color = c;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
        }
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            c.a = 1f - i;
            t.color = c;
            yield return new WaitForEndOfFrame();
        }
    }

    public void CreateMinigame(int tar, Button b, Part mgPArt)
    {
        minigame.SetActive(true);
        mgButton = b;
        minigamePart = mgPArt;
        remainingTargets = tar;
        targets = new List<GameObject>();
        System.Random r = new System.Random();
        int area = 99 * 2 / tar;
        minigameButton = GameObject.Instantiate(buttons[(int)b], minigame.transform);
        minigameButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -64);
        minigameButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        for (int i = 0; i < tar; i++)
        {
            targets.Add(GameObject.Instantiate(minigameTarget, minigame.transform));
            targets[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(r.Next(area * i - 99, area * (i+1) - 99), 0);
            targets[i].GetComponent<Target>().index = i;
        }
        minigamePointer.transform.SetSiblingIndex(transform.childCount);



        
    }

    public void UpdateMinigame(float pos)
    {
        minigamePointer.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos * 115, 0);
    }

    public void ButtonPressMinigame(Button b)
    {
        if (b == mgButton)
        {
            Target t = minigamePointer.Target;
            if (t != null)
            {
                targets[t.index].SetActive(false);
                remainingTargets--;
            }
            else
            {
                minigamePart.Abort();
                SendTextMessage("Daneben!");
            }
        }
        else
        {
            minigamePart.Abort();
            SendTextMessage("Falscher Knopf!");

        }

    }

    public void DestroyMinigame()
    {
        foreach(GameObject g in targets)
        {
            GameObject.Destroy(g);
        }
        GameObject.Destroy(minigameButton);
        minigame.SetActive(false);
    }

    public int RemainingTargets()
    {
        return remainingTargets;
    }
}
