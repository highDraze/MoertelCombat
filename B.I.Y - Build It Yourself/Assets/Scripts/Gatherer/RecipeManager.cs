using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public struct MaterialRecipe
{
    public Material material;
    public int wood;
    public int water;
    public int sand;
    public int steel;
}

public class RecipeManager : MonoBehaviour {


    #region Singleton

    static RecipeManager _instance;

    public static RecipeManager Instance
    {
        get
        {
            if (_instance == null) _instance = new RecipeManager();
            return _instance;
        }
    }

    private RecipeManager()
    {
    }

    #endregion

    private MaterialRecipe[][] recipes;
    public MaterialRecipe[] aRecipes;
    public MaterialRecipe[] bRecipes;


    public Material ParseInventory(List<Resource> inv, Team t)
    {
        Material o = Material.Müll;
        int wood = 0;
        int water = 0;
        int sand = 0;
        int steel = 0;

        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i] == Resource.Holz) wood++;
            else if (inv[i] == Resource.Stahl) steel++;
            else if (inv[i] == Resource.Sand) sand++;
            else if (inv[i] == Resource.Wasser) water++;
        }
        for (int i = 0; i < recipes[(int)t].Length; i++)
        {
            if (recipes[(int)t][i].wood == wood &&
                recipes[(int)t][i].sand == sand &&
                recipes[(int)t][i].steel == steel &&
                recipes[(int)t][i].water == water )
            {
                o = recipes[(int)t][i].material;
            }
        }
        return o;
        }
    // Use this for initialization

    private void Awake()
    {
        _instance = this;
        recipes = new MaterialRecipe[2][];
        recipes[0] = aRecipes;
        recipes[1] = bRecipes;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateRecipes()
    {

    }


}
