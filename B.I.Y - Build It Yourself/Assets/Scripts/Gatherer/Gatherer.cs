using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;



[RequireComponent(typeof(Collider))]
public class Gatherer : MonoBehaviour {

    public Team team;
    public float mvSpd;
    private int offset;

    public Interactable interactableTarget;


    Rigidbody rig;
    bool aP, bP, xP, yP;

    public List<Resource> inventory;
    // Use this for initialization
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        inventory = new List<Resource>();
        offset = 2;

        #if UNITY_EDITOR
        offset = 0;
        #endif

    }
    void Start () {
        GathererUI.Instance.UpdateInventory(team, inventory);
    }
	
	// Update is called once per frame
	void Update () {

        UpdateMovement();
        //Debug.Log(team.ToString());
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.A ==ButtonState.Pressed && !aP)
        {
            SendInput(Button.A);
            aP = true;
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.B == ButtonState.Pressed && !bP)
        {
            SendInput(Button.B);
            bP = true;
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.X == ButtonState.Pressed && !xP)
        {
            SendInput(Button.X);
            xP = true;
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.Y == ButtonState.Pressed && !yP)
        {
            SendInput(Button.Y);
            yP = true;
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.LeftShoulder == ButtonState.Pressed)
        {
            ClearInventory();
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.RightShoulder == ButtonState.Pressed)
        {
            GathererUI.Instance.ShowInstructionSheet(team);
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.RightShoulder == ButtonState.Released)
        {
            GathererUI.Instance.HideInstructionSheet(team);
        }

        //--------------------------

        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.A == ButtonState.Released)
        {
            aP = false;
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.B == ButtonState.Released)
        {
            bP = false;
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.X == ButtonState.Released)
        {
            xP = false;
        }
        if (GamePad.GetState((PlayerIndex)((int)team) + offset).Buttons.Y == ButtonState.Released)
        {
            yP = false;
        }
    }

    public void ClearInventory()
    {
        inventory.Clear();
        GathererUI.Instance.UpdateInventory(team, inventory);
    }

    void UpdateMovement()
    {
        float horiz = GamePad.GetState((PlayerIndex)((int)team + offset)).ThumbSticks.Left.Y;
        float verti = GamePad.GetState((PlayerIndex)((int)team + offset)).ThumbSticks.Left.X;

        if (horiz < 0) horiz = -1;
        else if (horiz > 0) horiz = 1;
        if (verti < 0) verti = -1;
        else if (verti > 0) verti = 1;

        Vector3 mov = new Vector3(verti * mvSpd, 0, horiz * mvSpd);

        rig.AddForce(mov, ForceMode.VelocityChange);
    }

    void SendInput(Button b)
    {
        if (interactableTarget != null)
            interactableTarget.ReceiveInput(this, b);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Interactable")
        {
            interactableTarget = other.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactableTarget = null;
    }

    public void ReceiveResource(Resource m)
    {
        if (inventory.Count < 3)
        {
            inventory.Add(m);
            GathererUI.Instance.UpdateInventory(team,inventory);
        }
    }


}
