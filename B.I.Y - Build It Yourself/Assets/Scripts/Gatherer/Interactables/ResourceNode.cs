using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceNode : Interactable {

    public Resource material;
    public int content;
    public float rechargeRate;
    private float dur;

    public override void ReceiveInput(Gatherer g, Button b)
    {
        switch(b)
        {
            case Button.A:
                if (material == Resource.Stahl)
                    SendResource(g);
                else
                    GathererUI.Instance.SendTextMessage(g.team, "Falsches Werkzeug");
                break;
            case Button.B:
                if (material == Resource.Sand)
                    SendResource(g);
                else
                    GathererUI.Instance.SendTextMessage(g.team, "Falsches Werkzeug");
                break;
            case Button.X:
                if (material == Resource.Wasser)
                    SendResource(g);
                else
                    GathererUI.Instance.SendTextMessage(g.team, "Falsches Werkzeug");
                break;
            case Button.Y:
                if (material == Resource.Holz)
                    SendResource(g);
                else
                    GathererUI.Instance.SendTextMessage(g.team, "Falsches Werkzeug");
                break;
            default:
                break;
        }

    }

    private void SendResource(Gatherer g)
    {
        if (content > 0)
        {
            g.ReceiveResource(material);
            content--;
        }
    }



    protected override void InheritedUpdate()
    {
        if (content == 0)
            transform.GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).gameObject.SetActive(true);
        if (content < 3)
        {
            dur += Time.deltaTime;
            if (dur >= rechargeRate)
            {
                dur = 0;
                content++;
            }
        }
        transform.Rotate(Vector3.up, 0.5f);
    }
}
