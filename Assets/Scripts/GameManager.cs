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
    [SerializeField] Parallax parallax;

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

    private int initialIceGenerationCount = 75;

    private float overheadInitX = 4.44f;
    private float overheadInitY = 0.97f;

    private float oreHeight = -1.9f;

    private float xOverheadInterval = 10.24f;
    private int initialOhGenerationCount = 3;

    private int icePerOh = 25;

    public struct OreType
    {
        public string name;
        public float depth;
        public int value;
        public Color color;
        public int hitsToDestroy;
        public float damageToIce;
        public float damageRadius;
        public float weight;

        public OreType(string name, float depth, int value, Color color, 
            int hitsToDestroy, float damageToIce, float damageRadius, float weight)
        {
            this.name = name;
            this.depth = depth;
            this.value = value;
            this.color = color;
            this.hitsToDestroy = hitsToDestroy;
            this.damageToIce = damageToIce;
            this.damageRadius = damageRadius;
            this.weight = weight;
        }
    }

    // 0/ 0 - 30m = emerald, weight 7
    // 1/ 30+ = gold, weight 5
    // 2/ 30+ = silver, weight 10
    // 3/ 80+ = moldalium, weight 5
    // 4/ 80+ = electrite, weight 5
    // 5/ 150+ = heat ruby, weight 10
    // 6/ 200+ = diamond, weight 15

    [SerializeField] private List<OreType> oreTypes = new List<OreType>() { 
        new OreType("emerald", -10, 50, new Color(0.5f, 1f, 0.5f), 2, 3, 1, 7),
        new OreType("gold", -10, 75, new Color(1f, 0.8f, 0f), 3, 4, 1, 5),
        new OreType("silver", -10, 50, new Color(0.9f, 0.9f, 1f), 2, 2, 2, 10),
        new OreType("moldalium", 80, 100, new Color(0.3f, 0.3f, 1f), 2, 8, 1, 5),
        new OreType("electrite", 80, 100, new Color(1f, 0.45f, 0.75f), 3, 6, 1, 5),
        new OreType("heat ruby", 150, 150, new Color(1f, 0.2f, 0.2f), 4, 5, 1, 10),
        new OreType("diamond", 150, 250, new Color(0.6f, 1f, 1f), 5, 4, 1, 15)
    };

    private void Start()
    {
        GenerateStartingOverheads();
        GenerateStartingIce();

        player.Initialize();
        parallax.Initialize();

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

        float depth = player.transform.position.x;

        List<OreType> possibleOres = new List<OreType>();
        foreach (OreType oreType in oreTypes)
        {
            if (depth >= oreType.depth)
            {
                possibleOres.Add(oreType);
            }
        }

        int selection = Random.Range(0, possibleOres.Count);
        OreType selectedOre = possibleOres[selection];

        ore.Initialize(selectedOre.name, oreSprites[Random.Range(0, oreSprites.Count)], selectedOre.color,
            selectedOre.hitsToDestroy, selectedOre.damageToIce, selectedOre.damageRadius, this);

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
