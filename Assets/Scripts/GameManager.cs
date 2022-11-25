﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Interval per ice block
    private float intervalX = 0.4f;

    private float initX = -1.5f;
    private float initY = -4.2f;

    Player player;

    // In-game values

    public bool isPlaying;

    private int currentId = 0;
    private int furthestActivatedIceId = 0;

    private int currentOhCount = 0;

    // Prefabs

    [SerializeField] private GameObject iceBlockPrefab;
    [SerializeField] private GameObject overheadPrefab;
    [SerializeField] private List<Sprite> overheadSprites;

    //

    [SerializeField] private List<IceBlock> iceblocks;

    [SerializeField] private Transform iceFolder;
    [SerializeField] private Transform rockFolder;

    // Game Constants

    private float maxInitialIceDurability = 80;
    private float minInitialIceDurability = 5;

    private float initialDurabilityPoints = 50;

    private float minimumDurabilityPoints = 15;
    private float durabilityVariation = 15;

    private float durabilityReducedPerGeneration = 0.1f;

    private int initialIceGenerationCount = 150;

    private float overheadInitX = 4.44f;
    private float overheadInitY = 0.97f;

    private float xOverheadInterval = 10.24f;
    private int initialOhGenerationCount = 6;

    private int icePerOh = 25;

    private void Start()
    {
        GenerateStartingOverheads();
        GenerateStartingIce();

        StartGame();
    }

    private void Update()
    {
        if (isPlaying)
        {

        }
    }

    public void StartGame()
    {
        isPlaying = true;
    }

    public void GenerateStartingIce()
    {
        // First, delete all old ice first

        for(int i = 0; i < initialIceGenerationCount; i++)
        {
            GenerateIce();
        }
    }

    public void GenerateStartingOverheads()
    {
        // First, delete all old rocks first
        for(int i = 0; i < initialOhGenerationCount; i++)
        {
            GenerateRock();
        }
    }

    // Generate ice uses current id to determine where to generate the ice.
    public void GenerateIce()
    {
        int id = currentId;

        float newPosX = id * intervalX + initX;
        float newPosY = initY;

        GameObject newIce = Instantiate(iceBlockPrefab);
        IceBlock iceBlock = newIce.GetComponent<IceBlock>();
        iceBlock.InitIce(id, GetRandomizedDurability(), this);

        newIce.transform.position = new Vector3(newPosX, newPosY, 0);
        iceblocks.Add(iceBlock);

        newIce.transform.SetParent(iceFolder);

        currentId++;
    }

    public void GenerateRock()
    {
        float posX = overheadInitX + currentOhCount * xOverheadInterval;
        float posY = overheadInitY;

        GameObject newOh = Instantiate(overheadPrefab);
        newOh.GetComponent<SpriteRenderer>().sprite = overheadSprites[Random.Range(0, overheadSprites.Count)];
        newOh.transform.position = new Vector3(posX, posY, 0);

        currentOhCount++;

        newOh.transform.SetParent(rockFolder);
    }

    public void RequestIceDie(IceBlock iceBlock)
    {
        iceblocks.Remove(iceBlock);
    }

    public float GetRandomizedDurability()
    {
        float baseDurability = initialDurabilityPoints - currentId * durabilityReducedPerGeneration;
        if(baseDurability < minimumDurabilityPoints) { baseDurability = minimumDurabilityPoints; }

        float randomMod = Random.Range(0, durabilityVariation);
        if(Random.Range(0, 2) == 0)
        {
            randomMod = -randomMod;
        }

        float finDurability = baseDurability + randomMod;

        if(finDurability < minInitialIceDurability) 
        { 
            finDurability = minInitialIceDurability; 
        }

        if(finDurability > maxInitialIceDurability) 
        { 
            finDurability = maxInitialIceDurability; 
        }

        return finDurability;
    }

    public void UpdateFurthestActiveIce(int id)
    {
        Debug.Log(currentId - id);

        if (id > furthestActivatedIceId)
        {
            furthestActivatedIceId = id;
            

            if(currentId - id < 80)
            {
                for(int i = 0; i < icePerOh; i++)
                {
                    GenerateIce();
                }

                GenerateRock();
            }
        }
    }
}
