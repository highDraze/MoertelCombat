using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Interactable
{
    public float duration;
    public Team t;
    private bool producing;
    public override void ReceiveInput(Gatherer g, Button b)
    {
        if(g.team == t && g.inventory.Count == 3 && !producing)
        {
            Material n = RecipeManager.Instance.ParseInventory(g.inventory, t);
            g.ClearInventory();
            GathererUI.Instance.SendTextMessage(g.team, "Produziere " + n.ToString());
            StartCoroutine(Produce(n));
        }
        else if (g.team != t)
        {
            GathererUI.Instance.SendTextMessage(g.team, "Diese Maschine gehört dir nicht");
        }
        else if (g.inventory.Count != 3)
        {
            GathererUI.Instance.SendTextMessage(g.team, "Du hast noch nicht genügend Material");
        }
        else if (producing)
        {
            GathererUI.Instance.SendTextMessage(g.team, "Diese Maschine läuft bereits");
        }
    }

    IEnumerator Produce(Material m)
    {
        producing = true;

            yield return new WaitForSeconds(25f);
 

        ResourceManager.Instance.IncrementMaterial(t, m);
        producing = false;
    }

    protected override void InheritedUpdate()
    {
        if (producing)
        {
            transform.localScale = new Vector3(0.8f + 0.2f * Mathf.Sin(Time.time * 2), 1, 0.8f + 0.2f * Mathf.Cos(Time.time * 2));
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
