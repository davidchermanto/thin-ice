using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Interval per ice block
    private float intervalX = 0.4f;

    private float initX = -1.5f;
    private float initY = -4.2f;

    private int currentId = 0;
    private int furthestActivatedIceId = 0;

    private GameObject iceBlockPrefab;
    private List<IceBlock> iceblocks;

    public bool isPlaying;

    private float maxInitialIceDurability = 60;
    private float minInitialIceDurability = 5;

    private float initialDurabilityPoints = 50;

    private float minimumDurabilityPoints = 10;
    private float durabilityVariation = 7;

    private float durabilityReducedPerGeneration = 0.1f;

    private float initialGenerationCount = 150;

    public void StartGame()
    {
        isPlaying = true;

    }

    public void GenerateStartingIce()
    {
        for(int i = 0; i < initialGenerationCount; i++)
        {
            GenerateIce();
        }
    }

    // Generate ice uses current id to determine where to generate the ice.
    public void GenerateIce()
    {
        if (isPlaying)
        {
            int id = currentId;

            float newPosX = id * intervalX + initX;
            float newPosY = initY;

            GameObject newIce = Instantiate(iceBlockPrefab);
            IceBlock iceBlock = newIce.GetComponent<IceBlock>();

            newIce.transform.position = new Vector3(newPosX, newPosY, 0);
            iceblocks.Add(iceBlock);
        }
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
        if(id > furthestActivatedIceId)
        {
            furthestActivatedIceId = id;
        }
    }
}
