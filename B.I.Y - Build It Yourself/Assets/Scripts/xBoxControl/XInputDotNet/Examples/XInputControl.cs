using System;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class XInputControl : MonoBehaviour {
    [SerializeField]
    public PlayerIndex playerIndex;
    public GamePadState state;

    void Update() {
        //if (!state.IsConnected) {
        foreach (PlayerIndex pi in Enum.GetValues(typeof(PlayerIndex))) {
            GamePadState state = GamePad.GetState(pi);
            if (state.IsConnected) {
                this.state = state;
                break;
            }
        }
        //}
        /*// Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndex1Set || !prevState.IsConnected) {
            for (int i = 0; i < 4; ++i) {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected) {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndex1Set = true;
                }
            }
        }
        if (playerIndex1Set && !playerIndex2Set || !prevState2.IsConnected) {
            for (int i = 0; i < 4; ++i) {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected) {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndex2Set = true;
                }
            }
        }
        prevState2 = state2;
        prevState = state;
        state = GamePad.GetState(playerIndex);

        */
    }
}
