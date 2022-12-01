using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastOre : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;

    [SerializeField] private GameObject visual;

    // Constant
    private float hitDistance = 2.2f;

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);

        foreach (RaycastHit2D hitCollider in hit)
        {
            if (hitCollider.collider != null)
            {
                float dist = Vector3.Distance(hitCollider.collider.transform.position, player.transform.position);

                if (hitCollider.collider.gameObject.CompareTag("Ore"))
                {
                    OreCollider ore = hitCollider.collider.GetComponent<OreCollider>();

                    if (Input.GetMouseButtonDown(0) && dist <= hitDistance)
                    {
                        ore.HitOre();
                    }

                    if (!ore.visualActive)
                    {
                        GameObject vis = Instantiate(visual);
                        vis.transform.position = ore.transform.position;

                        if (dist <= hitDistance)
                        {
                            vis.GetComponent<VibrateVisual>().Initialize(ore.ore.oreCollider2D.radius, ore, true);
                        }
                        else
                        {
                            vis.GetComponent<VibrateVisual>().Initialize(ore.ore.oreCollider2D.radius, ore, false);
                        }
                    }
                }
            }
        }
    }
}
