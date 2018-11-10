using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Team { A, B}
public enum Resource { Sand, Stahl, Holz, Wasser, Leer }

public enum Material { Ziegel, Schindel, Balken, Bauelement, Müll}

public class ResourceManager {


    Dictionary<Material, int>[] resources;
    float[] teamProgress;

    #region Singleton

    static ResourceManager _instance;

    public static ResourceManager Instance
    {
        get
        {
            if (_instance == null) _instance = new ResourceManager();
            return _instance;
        }
    }

    private ResourceManager()
    {
        resources = new Dictionary<Material, int>[] { new Dictionary<Material, int>(), new Dictionary<Material, int>() };

        for (int i = 0; i < Enum.GetNames(typeof(Material)).Length; i++)
        {
            resources[0].Add((Material)Enum.GetValues(typeof(Material)).GetValue(i), 0);
            resources[1].Add((Material)Enum.GetValues(typeof(Material)).GetValue(i), 0);
        }

        teamProgress = new float[2];
    }

    #endregion


    public int GetMaterial (Team t, Material r)
    {
        int o = 0;
        resources[(int)t].TryGetValue(r, out o);
        return o;
    }
    public void SetMaterial(Team t, Material r, int value)
    {
        resources[(int)t][r] = value;
    }
    public void IncrementMaterial(Team t, Material r)
    {
        int amount = 0;
        switch(r)
        {
            case Material.Ziegel:
                amount = 3;
                break;
            case Material.Balken:
                amount = 3;
                break;
            case Material.Schindel:
                amount = 5;
                break;
            case Material.Bauelement:
                amount = 1;
                break;
        }
        resources[(int)t][r] += amount;
    }
    public void DecrementMaterial(Team t, Material r)
    {

        resources[(int)t][r]--;
    }

    public void UpdateProgress (Team t, float p)
    {
        teamProgress[(int)t] = p;
    }

    public float GetProgress (Team t)
    {
        return teamProgress[(int)t];
    }

    public void Delete()
    {
        _instance = null;
    }
}
