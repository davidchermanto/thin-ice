using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private List<Sprite> backSprites;
    [SerializeField] private List<Sprite> frontSprites;

    private float parallaxBackRatio = 0.6f;
    private float parallaxFrontRatio = 0.8f;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform front;
    [SerializeField] private Transform back;

    [SerializeField] private GameObject frontPrefab;
    [SerializeField] private GameObject backPrefab;

    [SerializeField] private List<GameObject> fronts;
    [SerializeField] private List<GameObject> backs;

    private float delayPerCPFront = 4.5f;
    private float delayPerCPBack = 5f;

    private float lastCheckPointFront = 0;
    private float lastCheckPointBack = 0;

    private int initialGenerationCount = 15;

    private float initialX = -15;
    private float initialY = -3f;
    private float varY = 0.4f;
    private float generationTolerance = 40;

    public void Initialize()
    {
        for(int i = 0; i < initialGenerationCount; i++)
        {
            GenerateBack();
            GenerateFront();
        }
    }

    void LateUpdate()
    {
        front.localPosition = new Vector3(-cam.transform.position.x * parallaxFrontRatio, 0);
        back.localPosition = new Vector3(-cam.transform.position.x * parallaxBackRatio, 0);

        Vector3 camPos = cam.transform.position;
        if(lastCheckPointFront - camPos.x < generationTolerance)
        {
            GenerateFront();
        }

        if (lastCheckPointBack - camPos.x < generationTolerance)
        {
            GenerateBack();
        }
    }

    private void GenerateFront()
    {
        GameObject newFront = Instantiate(frontPrefab);

        newFront.GetComponent<SpriteRenderer>().sprite = frontSprites[Random.Range(0, frontSprites.Count)];

        newFront.transform.SetParent(front);
        newFront.transform.localPosition = new Vector3(initialX + fronts.Count * delayPerCPFront, initialY + Random.Range(0, varY));

        fronts.Add(newFront);
        lastCheckPointFront += delayPerCPFront;
    }

    private void GenerateBack()
    {
        GameObject newBack = Instantiate(backPrefab);

        newBack.GetComponent<SpriteRenderer>().sprite = backSprites[Random.Range(0, backSprites.Count)];

        newBack.transform.SetParent(back);
        newBack.transform.localPosition = new Vector3(initialX + backs.Count * delayPerCPBack, initialY + Random.Range(0, varY));

        backs.Add(newBack);
        lastCheckPointBack += delayPerCPBack;
    }
}
