using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIElements
{
    public Image instructionSheet;
    public Text message;
    public IEnumerator messageActive;
    public Image[] inventorySpaces;
    public Text progress;

}
public class GathererUI : MonoBehaviour {

    private UIElements[] teamInterfaces;
    public Image[] aInventorySpaces;
    public Image[] bInventorySpaces;
    public Image aInstructionSheet;
    public Image bInstructionSheet;
    public Text aMessage;
    public Text bMessage;
    public Sprite[] inventorySprites;
    public Text aProgress;
    public Text bProgress;
    public Gatherer AGatherer;
    public Gatherer BGatherer;


    #region Singleton

    static GathererUI _instance;

    public static GathererUI Instance
    {
        get
        {
            if (_instance == null) _instance = new GathererUI();
            return _instance;
        }
    }

    private GathererUI()
    {
        Awake();
    }

    #endregion

    private void Awake()
    {
        _instance = this;
        teamInterfaces = new UIElements[2];
        teamInterfaces[0].inventorySpaces = aInventorySpaces;
        teamInterfaces[1].inventorySpaces = bInventorySpaces;

        teamInterfaces[0].instructionSheet = aInstructionSheet;
        teamInterfaces[1].instructionSheet = bInstructionSheet;

        teamInterfaces[0].message = aMessage;
        teamInterfaces[1].message = bMessage;
        //teamInterfaces[0].message.text = "";
        //teamInterfaces[1].message.text = "";
        //teamInterfaces[0].message.color = new Color(1f, 0f, 0f, 1f);
        //teamInterfaces[1].message.color = new Color(1f, 0f, 0f, 1f);
        teamInterfaces[0].messageActive = ShowTextMessage(teamInterfaces[0].message);
        teamInterfaces[1].messageActive = ShowTextMessage(teamInterfaces[1].message);
        teamInterfaces[0].progress = aProgress;
        teamInterfaces[1].progress = bProgress;

    }

    private float CalculateOffset(Team t)
    {
        return ((t == Team.A) ? -200 : 200);
    }
    // Use this for initialization
    void Start ()
    {
        //tex.GetComponent<RectTransform>().anchoredPosition = new Vector2(sideOffset, -200);
	}

    private void Update()
    {
        teamInterfaces[0].progress.text = "Baufortschritt: " + string.Format("{0:0.##\\%}", ResourceManager.Instance.GetProgress(Team.A) * 100);
        teamInterfaces[1].progress.text = "Baufortschritt: " + string.Format("{0:0.##\\%}", ResourceManager.Instance.GetProgress(Team.B) * 100);
    }

    // Update is called once per frame
    public void UpdateInventory(Team t,List<Resource> r)
    {
        for (int i = 0; i < r.Count; i++)
        {
            teamInterfaces[(int)t].inventorySpaces[i].enabled = true;
            teamInterfaces[(int)t].inventorySpaces[i].sprite = inventorySprites[(int)r[i]];
        }
        for (int i = r.Count; i < 3; i++)
        {
            teamInterfaces[(int)t].inventorySpaces[i].enabled = false;
        }
    }

    public void ShowInstructionSheet(Team t)
    {
        RectTransform r = teamInterfaces[(int)t].instructionSheet.GetComponent<RectTransform>();
        r.anchoredPosition = new Vector2((t == Team.A)?-290 : 290, 0);
    }

    public void HideInstructionSheet(Team t)
    {
        RectTransform r = teamInterfaces[(int)t].instructionSheet.GetComponent<RectTransform>();
        r.anchoredPosition = new Vector2((t == Team.A) ? -513 : 513, 0);
    }

    public void SendTextMessage(Team t,string m)
    {
        if (teamInterfaces[(int)t].messageActive != null)
            StopCoroutine(teamInterfaces[(int)t].messageActive);
        teamInterfaces[(int)t].message.text = m;
        teamInterfaces[(int)t].messageActive = ShowTextMessage(teamInterfaces[(int)t].message);
        StartCoroutine(teamInterfaces[(int)t].messageActive);
    }

    IEnumerator ShowTextMessage(Text t)
    {
        Color c = t.color;
        c.a = 1f;
        t.color = c;
        for (float i = 0; i < 1f; i+= Time.deltaTime)
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

    public void Finish()
    {
        AGatherer.enabled = false;
        BGatherer.enabled = false;
    }
}
