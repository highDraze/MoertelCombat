using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class SceneLoader : MonoBehaviour{

    #region Singleton

    static SceneLoader _instance;

    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null) _instance = new SceneLoader();
            return _instance;
        }
    }

    private SceneLoader() { }

    #endregion

    public GameObject cameras;
    public GameObject canvas;
    public Text victoryMessage;
    public GameObject blend;
    private bool running;
    public void SetupGame()
    {
        if (!running)
        {
            cameras.SetActive(false);
            canvas.SetActive(false);
            SceneManager.LoadScene("BuildWorld", LoadSceneMode.Additive);
            SceneManager.LoadScene("GatherWorld", LoadSceneMode.Additive);
            running = true;
        }
    }

    public void DestroyGame()
    {
        if (running)
        {
            SceneManager.UnloadSceneAsync("BuildWorld");
            SceneManager.UnloadSceneAsync("GatherWorld");
            cameras.SetActive(true);
            canvas.SetActive(true);
            running = false;
        }

    }

    public void EndGame(Team t)
    {
        DestroyGame();
        StartCoroutine(ShowWinner(t));

    }
	// Use this for initialization
	void Start ()
    {
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
            SetupGame();
        if (Input.GetKeyDown(KeyCode.R))
            DestroyGame();
    }

    IEnumerator ShowWinner(Team t)
    {
        blend.SetActive(true);
        if (t == Team.A)
        {
            victoryMessage.color = new Color(139f / 255f, 35f / 255f, 35f / 255f,1f);
            victoryMessage.text = "Team A won\nCongratulations!";
        }
        else
        {
            victoryMessage.color = new Color(14f / 255f, 115f / 255f, 148f / 255f,1f);
            victoryMessage.text = "Team B won\nCongratulations!";
        }
        yield return new WaitForSeconds(10f);
        blend.SetActive(false);
        victoryMessage.color = new Color(0f, 0f, 0f, 0f);
    }
    
}
