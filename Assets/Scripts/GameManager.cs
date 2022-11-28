using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Interval per ice block
    private float intervalX = 0.4f;

    private float initX = -1.5f;
    private float initY = -4.2f;

    [SerializeField] Player player;

    // In-game values

    public bool isPlaying;

    private int currentId = 0;
    private int furthestActivatedIceId = 0;

    private int currentOhCount = 0;

    private int currentOreCount = 0;

    // Prefabs

    [SerializeField] private GameObject iceBlockPrefab;
    [SerializeField] private GameObject overheadPrefab;
    [SerializeField] private List<Sprite> overheadSprites;

    [SerializeField] private GameObject orePrefab;
    [SerializeField] private List<Sprite> oreSprites;

    //

    [SerializeField] private List<IceBlock> iceblocks;

    [SerializeField] private Transform iceFolder;
    [SerializeField] private Transform rockFolder;

    [SerializeField] private List<Ore> ores;

    [SerializeField] private Transform oreFolder;

    // Game Constants

    private float maxInitialIceDurability = 90;
    private float minInitialIceDurability = 15;

    private float initialDurabilityPoints = 30;

    private float minimumDurabilityPoints = 15;
    private float durabilityVariation = 15;

    private float durabilityReducedPerGeneration = 0.07f;

    private int initialIceGenerationCount = 150;

    private float overheadInitX = 4.44f;
    private float overheadInitY = 0.97f;

    private float oreHeight = -1.9f;

    private float xOverheadInterval = 10.24f;
    private int initialOhGenerationCount = 6;

    private int icePerOh = 25;

    private void Start()
    {
        GenerateStartingOverheads();
        GenerateStartingIce();

        player.Initialize();

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
        newIce.name = "ice " + currentId;
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
        newOh.name = "overheadRock " + currentOhCount;
        newOh.GetComponent<SpriteRenderer>().sprite = overheadSprites[Random.Range(0, overheadSprites.Count)];
        newOh.transform.position = new Vector3(posX, posY, 0);

        currentOhCount++;

        newOh.transform.SetParent(rockFolder);

        // Generate 1 or 2 ores as well on the rock.
        float oposX = overheadInitX + currentOhCount * xOverheadInterval - Random.Range(0.5f, 0.8f) * xOverheadInterval;
        float oposY = oreHeight;

        GenerateRandomOre(oposX, oposY);

        if(Random.Range(0, 10) > 1)
        {
            oposX = overheadInitX + currentOhCount * xOverheadInterval + Random.Range(0.5f, 0.8f) * xOverheadInterval;

            GenerateRandomOre(oposX, oposY);
        }
    }

    public void GenerateRandomOre(float x, float y)
    {
        // Choices are emerald, silver, gold, moldalium, electrite, heat ruby, and diamond
        GameObject newOre = Instantiate(orePrefab);

        newOre.name = "ore "+currentOreCount;
        newOre.transform.position = new Vector3(x, y);
        newOre.transform.SetParent(oreFolder);

        Ore ore = newOre.GetComponent<Ore>();
        ore.Initialize("emerald", oreSprites[Random.Range(0, oreSprites.Count)], new Color(0.5f, 1f, 0.5f), 2, 3, 1, this);

        currentOreCount++;

        ores.Add(ore);
    }

    public void RequestIceDie(IceBlock iceBlock)
    {
        iceblocks.Remove(iceBlock);
    }

    public void RequestOreDie(Ore ore)
    {
        ores.Remove(ore);
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
