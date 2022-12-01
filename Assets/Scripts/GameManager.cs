using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Interval per ice block
    private float intervalX = 0.4f;

    private float initX = -1.5f;
    private float initY = -4.2f;

    [SerializeField] Transform startPos;
    [SerializeField] Player player;
    [SerializeField] Parallax parallax;
    [SerializeField] UIManager uiManager;

    // In-game values

    public bool isPlaying;

    private int currentId = 0;
    private int furthestActivatedIceId = 0;

    private int currentOhCount = 0;

    private int currentOreCount = 0;

    private float currentFrost = 0;
    private float currentDepth = 0;

    private int sonarCount = 3;
    private int constructCount = 3;

    private int currentEmerald;
    private int currentGold;
    private int currentSilver;
    private int currentMoldalium;
    private int currentElectrite;
    private int currentRuby;
    private int currentDiamond;
    private int currentStellium;

    private float lastDurability = 0;
    private bool durabilityDown = false;

    private int wealth;

    // Prefabs
    [Header("Prefab")]
    [SerializeField] private GameObject iceBlockPrefab;
    [SerializeField] private GameObject overheadPrefab;
    [SerializeField] private List<Sprite> overheadSprites;

    [SerializeField] private GameObject orePrefab;
    [SerializeField] private List<Sprite> oreSprites;

    [SerializeField] private SpriteRenderer frost;

    //
    [Header("Folders")]
    [SerializeField] private List<IceBlock> iceblocks;

    [SerializeField] private Transform iceFolder;
    [SerializeField] private Transform rockFolder;

    [SerializeField] private List<Ore> ores;

    [SerializeField] private Transform oreFolder;

    [SerializeField] private GameObject vCam;

    // Game Constants

    private float maxInitialIceDurability = 45;
    private float minInitialIceDurability = 16;

    private float initialDurabilityPoints = 30;

    private float minBaseDurability = 25;
    private float durabilityJumpVariationMax = 5;

    private float durabilityReducedPerGeneration = 0.07f;

    private int initialIceGenerationCount = 25;

    private float overheadInitX = 4.44f;
    private float overheadInitY = 0.97f;

    private float oreHeight = -1.9f;

    private float xOverheadInterval = 10.24f;
    private int initialOhGenerationCount = 1;

    private int icePerOh = 25;

    private float frostPerSecond = 1.4f;

    // Gems?
    private float frostDecreasePerRubyHit = 5f;

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
        new OreType("emerald", -10, 50, new Color(0.5f, 1f, 0.5f), 2, 3, 1, 3),
        new OreType("gold", 0, 75, new Color(1f, 0.8f, 0f), 3, 4, 1.3f, 5),
        new OreType("silver", -10, 50, new Color(0.9f, 0.9f, 1f), 2, 2, 2, 3),
        new OreType("moldalium", 20, 100, new Color(0.3f, 0.3f, 1f), 2, 8, 1.5f, 7),
        new OreType("electrite", 30, 100, new Color(1f, 0.45f, 0.75f), 3, 6, 1.7f, 7),
        new OreType("heat ruby", 50, 150, new Color(1f, 0.2f, 0.2f), 4, 5, 2f, 8),
        new OreType("diamond", 70, 250, new Color(0.6f, 1f, 1f), 5, 4, 1, 10),
        new OreType("stellium", 200, 500, new Color(1f, 1f, 1f), 7, 3, 2.4f, 15)
    };

    private void Start()
    {

    }

    private void Update()
    {
        if (isPlaying)
        {
            currentDepth = (float)System.Math.Round(player.transform.position.x + 2.55f, 1);
            currentFrost += Time.deltaTime * frostPerSecond;
            frost.color = new Color(frost.color.r, frost.color.g, frost.color.b, Mathf.Lerp(0, 0.1f, currentFrost / 100));

            if(currentFrost > 100) { DieFrost(); }

            if (!player.moving)
            {
                currentFrost = 0;
            }

            uiManager.UpdateGameValues(wealth, currentFrost, currentDepth, sonarCount, constructCount);
            uiManager.UpdateWealthMenu(currentEmerald, currentEmerald * oreTypes[0].value, currentSilver, currentSilver * oreTypes[2].value,
                currentGold, currentGold * oreTypes[1].value, currentElectrite, currentElectrite * oreTypes[4].value, currentMoldalium,
                currentMoldalium * oreTypes[3].value, currentRuby, currentRuby * oreTypes[5].value, currentDiamond, currentDiamond *
                oreTypes[6].value, currentStellium, currentStellium * oreTypes[7].value);
        }
    }

    public void StartGame()
    {
        uiManager.TurnOffTutorial();
        ResetAll();

        GenerateStartingOverheads();
        GenerateStartingIce();

        player.Initialize();
        parallax.Initialize();

        isPlaying = true;
        player.isPlaying = true;
        vCam.SetActive(true);
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

    public void ActivateSonar()
    {
        if (isPlaying)
        {
            if(sonarCount > 0)
            {
                sonarCount--;
                StartCoroutine(Sonar());
            }
        }
    }

    private IEnumerator Sonar()
    {
        int iceDeleteRange = 40;

        int higherCounter = furthestActivatedIceId;
        int lowerCounter = furthestActivatedIceId - 1;

        bool higherEnd = false;
        bool lowerEnd = false;

        for (int i = 0; i < iceDeleteRange; i++)
        {
            if (!higherEnd)
            {
                if (iceblocks[higherCounter].isActiveAndEnabled)
                {
                    iceblocks[higherCounter].BlinkDurability();
                }

                higherCounter++;
            }

            if (!lowerEnd)
            {
                if (iceblocks[lowerCounter].isActiveAndEnabled)
                {
                    iceblocks[lowerCounter].BlinkDurability();
                }

                lowerCounter--;
            }

            if (higherCounter > iceblocks.Count) { higherEnd = true; }
            if (lowerCounter < 1) { lowerEnd = true; }

            yield return new WaitForSeconds(0.03f);
        }
    }

    public void ActivateConstruct()
    {
        if (isPlaying)
        {
            if (constructCount > 0)
            {
                constructCount--;
                StartCoroutine(Construct());
            }
        }
    }

    private IEnumerator Construct()
    {
        int iceDeleteRange = 20;

        int higherCounter = furthestActivatedIceId;
        int lowerCounter = furthestActivatedIceId - 1;

        bool higherEnd = false;
        bool lowerEnd = false;

        for (int i = 0; i < iceDeleteRange; i++)
        {
            if (!higherEnd)
            {
                if (iceblocks[higherCounter].isActiveAndEnabled)
                {
                    iceblocks[higherCounter].Construct();
                }

                higherCounter++;
            }

            if (!lowerEnd)
            {
                if (iceblocks[lowerCounter].isActiveAndEnabled)
                {
                    iceblocks[lowerCounter].Construct();
                }

                lowerCounter--;
            }

            if (higherCounter > iceblocks.Count) { higherEnd = true; }
            if (lowerCounter < 1) { lowerEnd = true; }

            yield return new WaitForSeconds(0.03f);
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
        float oposX = overheadInitX + currentOhCount * xOverheadInterval - Random.Range(0.45f, 0.65f) * xOverheadInterval;
        float oposY = oreHeight;

        GenerateRandomOre(oposX, oposY);

        if(Random.Range(0, 10) > 1)
        {
            oposX = overheadInitX + currentOhCount * xOverheadInterval - Random.Range(0.1f, 0.3f) * xOverheadInterval;

            GenerateRandomOre(oposX, oposY);
        }

        if (Random.Range(0, 10) > 1)
        {
            oposX = overheadInitX + currentOhCount * xOverheadInterval - Random.Range(0.75f, 0.9f) * xOverheadInterval;

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

        float depth = currentDepth;

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
            selectedOre.hitsToDestroy, selectedOre.damageToIce, selectedOre.damageRadius, this, selectedOre.value);

        currentOreCount++;

        ores.Add(ore);
    }

    // Player loses?
    public void DieFall(IceBlock iceBlock)
    {
        if (!player.accLeft)
        {
            isPlaying = false;
            player.isPlaying = false;
            player.StopAllCoroutines();
            vCam.SetActive(false);

            StartCoroutine(DeathAnimation());
        }
    }

    public void DieFrost()
    {
        if (!player.accLeft)
        {
            isPlaying = false;
            player.isPlaying = false;
            player.StopAllCoroutines();
            vCam.SetActive(false);

            StartCoroutine(FreezeAnimation());
        }
    }

    public void ResetAll()
    {
        isPlaying = false;
        currentId = 0;
        furthestActivatedIceId = 0;
        currentOhCount = 0;
        currentOreCount = 0;
        sonarCount = 3;
        constructCount = 3;

        currentFrost = 0;
        currentDepth = 0;

        currentEmerald = 0;
        currentGold = 0;
        currentSilver = 0;
        currentMoldalium = 0;
        currentElectrite = 0;
        currentRuby = 0;
        currentDiamond = 0;
        currentStellium = 0;

        lastDurability = 0;
        durabilityDown = false;

        wealth = 0;

        for(int i = 0; i < iceblocks.Count; i++) { Destroy(iceblocks[i].gameObject); }
        iceblocks = new List<IceBlock>();

        for(int i = 0; i < ores.Count; i++) { Destroy(ores[i].gameObject); }
        ores = new List<Ore>();

        foreach(Transform child in rockFolder.transform) { Destroy(child.gameObject); }

        frost.color = new Color(frost.color.r, frost.color.g, frost.color.b, 0);
    }

    // Death by falling
    private IEnumerator DeathAnimation()
    {
        StartCoroutine(DeleteIces());

        yield return new WaitForSeconds(2f);

        float time = 0;
        float fallDuration = 1;
        float fallSpeed = 15;
        while(time < 1)
        {
            time += Time.deltaTime * fallDuration;

            player.transform.position = new Vector3(player.transform.position.x, 
                player.transform.position.y - fallSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        uiManager.NewScore(wealth);
        uiManager.TurnOnTutorial();

        player.transform.position = startPos.transform.position;
    }

    private IEnumerator FreezeAnimation()
    {
        float time = 0;
        float freezeTime = 1;
        while (time < 1)
        {
            time += Time.deltaTime * freezeTime;

            frost.color = new Color(frost.color.r, frost.color.g, frost.color.b, Mathf.Lerp(0.1f, 0.3f, time));

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        time = 0;
        float fallDuration = 1;
        float fallSpeed = 15;
        while (time < 1)
        {
            time += Time.deltaTime * fallDuration;

            player.transform.position = new Vector3(player.transform.position.x,
                player.transform.position.y - fallSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        uiManager.NewScore(wealth);
        uiManager.TurnOnTutorial();

        player.transform.position = startPos.transform.position;
    }

    private IEnumerator DeleteIces()
    {
        int iceDeleteRange = 30;

        int higherCounter = furthestActivatedIceId;
        int lowerCounter = furthestActivatedIceId - 1;

        bool higherEnd = false;
        bool lowerEnd = false;

        for(int i = 0; i < iceDeleteRange; i++)
        {
            if (!higherEnd)
            {
                if (iceblocks[higherCounter].isActiveAndEnabled)
                {
                    iceblocks[higherCounter].iceCollider.enabled = false;
                    iceblocks[higherCounter].Explode();
                }

                higherCounter++;
            }

            if (!lowerEnd)
            {
                if (iceblocks[lowerCounter].isActiveAndEnabled)
                {
                    iceblocks[lowerCounter].iceCollider.enabled = false;
                    iceblocks[lowerCounter].Explode();
                }

                lowerCounter--;
            }

            if(higherCounter > iceblocks.Count) { higherEnd = true; }
            if(lowerCounter < 1) { lowerEnd = true; }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RequestOreDie(Ore ore)
    {
        ores.Remove(ore);
        wealth += ore.value;

        switch (ore.oreName)
        {
            case "emerald":
                currentEmerald++;
                break;
            case "gold":
                currentGold++;
                break;
            case "silver":
                currentSilver++;
                break;
            case "moldalium":
                currentMoldalium++;
                constructCount++;
                break;
            case "electrite":
                currentElectrite++;
                sonarCount++;
                break;
            case "heat ruby":
                currentRuby++;
                currentFrost -= frostDecreasePerRubyHit;
                break;
            case "diamond":
                currentDiamond++;
                break;
            case "stellium":
                currentStellium++;
                break;
            default:
                break;
        }
    }

    public float GetRandomizedDurability()
    {
        if(lastDurability == 0) { lastDurability = initialDurabilityPoints; }

        float durability = lastDurability;
        if (durabilityDown)
        {
            durability -= Random.Range(0, durabilityJumpVariationMax);
        }
        else
        {
            durability += Random.Range(0, durabilityJumpVariationMax);
        }

        if(durability < minInitialIceDurability) 
        { 
            durabilityDown = false; 
        }
        else if(durability > Mathf.Max((maxInitialIceDurability - 
            currentId * durabilityReducedPerGeneration), minBaseDurability)) 
        { 
            durabilityDown = true; 
        }

        float finDurability = durability;

        if(finDurability < minInitialIceDurability) 
        { 
            finDurability = minInitialIceDurability; 
        }

        if(finDurability > maxInitialIceDurability) 
        { 
            finDurability = maxInitialIceDurability; 
        }

        lastDurability = finDurability;

        return finDurability;
    }

    public void UpdateFurthestActiveIce(int id)
    {
        if (id > furthestActivatedIceId)
        {
            furthestActivatedIceId = id;

            if(currentId - id < 40)
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
