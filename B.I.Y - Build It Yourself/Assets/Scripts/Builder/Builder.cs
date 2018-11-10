using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public enum Button { A, B, X, Y }

[System.Serializable]
public class Builder : MonoBehaviour
{

    public Team team;
    public BluePrint blueprint;
    public BuilderUI bUI;

    public Vector2 side;
    Vector3 basePosition;
    Vector3 curGaze;
    Vector3 curPosition;
    public bool transformInAction;
    Queue<CamMovement> movementQueue;
    bool aP, bP, xP, yP;
    private KeyCode teamButton;


    private struct CamMovement
    {
        public Vector3 startPos, newPos, startGaze, newGaze;
        public float duration;
        public bool smooth;
    }

    private void Awake()
    {
        bUI.team = team;
        movementQueue = new Queue<CamMovement>();
        side = new Vector2(-1, 0);
        basePosition = new Vector3(-16, 4, 0);
        curPosition = basePosition;
        curGaze = blueprint.transform.position;
        transformInAction = false;
        transform.position = blueprint.transform.position + curPosition;
        teamButton = (team == Team.A) ? KeyCode.A : KeyCode.B;

    }
    void Start()
    {
        blueprint.bUI = bUI;
        blueprint.bI = this;
    }

    void Update()
    {
        UpdatePosition();

        if (!transformInAction)
        {
            //if (Input.GetKeyDown(KeyCode.R))
            //{

            //    rotate(true);
            //}
            //if (Input.GetKeyDown(KeyCode.T))
            //{

            //    rotate(false);
            //}
            //if (Input.GetKeyDown(KeyCode.N))
            //{

            //    moveNext(blueprint.getNextPart());
            //}

            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.A == ButtonState.Pressed && !aP)
            {
                blueprint.SendInput(Button.A);
                aP = true;
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.B == ButtonState.Pressed && !bP)
            {
                blueprint.SendInput(Button.B);
                bP = true;
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.X == ButtonState.Pressed && !xP)
            {
                blueprint.SendInput(Button.X);
                xP = true;
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.Y == ButtonState.Pressed && !yP)
            {
                blueprint.SendInput(Button.Y);
                yP = true;
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.RightShoulder == ButtonState.Pressed)
            {
                bUI.ShowInstructionSheet();
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.RightShoulder == ButtonState.Released)
            {
                bUI.HideInstructionSheet();
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.LeftShoulder == ButtonState.Pressed)
            {
                blueprint.ClearCombo();
            }
            //---------------------------------------------------------------------------

            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.A == ButtonState.Released)
            {
                aP = false;
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.B == ButtonState.Released)
            {
                bP = false;
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.X == ButtonState.Released)
            {
                xP = false;
            }
            if (GamePad.GetState((PlayerIndex)((int)team)).Buttons.Y == ButtonState.Released)
            {
                yP = false;
            }


        }


        if (Input.GetKeyDown(teamButton))
        {
            blueprint.Skip();
        }

    }

    void UpdatePosition()
    {
        if (movementQueue.Count > 0)
        {
            transformInAction = true;
            StartCoroutine(ApplyMovement(movementQueue.Dequeue()));
        }
    }


    void NewPosition(float duration, Vector3 newPos, Vector3 newGaze, bool smooth)
    {
        CamMovement n;
        n.duration = duration;
        n.startPos = curPosition;
        n.newPos = newPos;
        n.startGaze = curGaze;
        n.newGaze = newGaze;
        n.smooth = smooth;
        movementQueue.Enqueue(n);
    }

    /// <summary>
    /// Rotiert die Kamera nach rechts oder links
    /// </summary>
    /// <param name="dir">True für Rechts, false für Links</param>
    private void Rotate(bool dir)
    {
        Vector3 newPos;


        if (side.x != 0)
        {
            newPos = dir ? new Vector3(0, basePosition.y, basePosition.x) : new Vector3(0, basePosition.y, basePosition.x * -1f);
            side = dir ? new Vector2(0, side.x) : new Vector2(0, side.x * -1f);
        }
        else
        {
            newPos = dir ? new Vector3(basePosition.z * -1f, basePosition.y, 0) : new Vector3(basePosition.z, basePosition.y, 0);
            side = dir ? new Vector2(side.y * -1f, 0) : new Vector2(side.y, 0);
        }


        basePosition = newPos;

    }

    public void MoveNext(Part p)
    {
        if (p != null)
        {
            if (side.x != p.side.x || side.y != p.side.y)
            {
                if (side.x == p.side.x * -1 || side.y == p.side.y * -1)
                {
                    Rotate(false);
                    Rotate(false);
                }
                else if ((side.x == -1 && p.side.y == 1) || (side.y == 1 && p.side.x == 1) || (side.x == 1 && p.side.y == -1) || (side.y == -1 && p.side.x == -1))
                {
                    Rotate(false);
                }
                else
                {
                    Rotate(true);
                }
            }
            Vector3 newPos = p.transform.position + new Vector3(10 * side.x, 3, 10 * side.y);

            NewPosition(0.3f, newPos, p.transform.position, false);

            curPosition = newPos;
            curGaze = p.transform.position;

        }
    }

    private IEnumerator ApplyMovement(CamMovement cm)
    {
        float l = (cm.startPos.magnitude + cm.newPos.magnitude) / 2f;
        for (float i = 0; i < cm.duration; i += Time.deltaTime)
        {
            Vector3 nPos = Vector3.Lerp(cm.startPos, cm.newPos, i / cm.duration);

            if (cm.smooth)
                transform.position = new Vector3(nPos.normalized.x * l, nPos.y, nPos.normalized.z * l);
            else
                transform.position = nPos;
            transform.LookAt(Vector3.Lerp(cm.startGaze, cm.newGaze, i / cm.duration));
            yield return new WaitForEndOfFrame();
        }
        transform.position = cm.newPos;
        transform.LookAt(cm.newGaze);

        transformInAction = false;
    }
}
