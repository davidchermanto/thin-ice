using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Interval per ice block
    private float intervalX = 0.4f;

    private float initX = -1.5f;
    private float initY = -4.2f;

    // In-game values

    public bool isPlaying;

    private int currentId = 0;
    private int furthestActivatedIceId = 0;

    //

    [SerializeField] private GameObject iceBlockPrefab;
    [SerializeField] private List<IceBlock> iceblocks;

    [SerializeField] private Transform iceFolder;

    private float maxInitialIceDurability = 60;
    private float minInitialIceDurability = 5;

    private float initialDurabilityPoints = 50;

    private float minimumDurabilityPoints = 10;
    private float durabilityVariation = 7;

    private float durabilityReducedPerGeneration = 0.1f;

    private float initialGenerationCount = 150;

    private void Start()
    {
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
        GenerateStartingIce();
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
            iceBlock.InitIce(id, GetRandomizedDurability());

            newIce.transform.position = new Vector3(newPosX, newPosY, 0);
            iceblocks.Add(iceBlock);

            newIce.transform.SetParent(iceFolder);

            currentId++;
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
