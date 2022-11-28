using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastOre : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;

    // Constant
    private float hitDistance = 2.2f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);

            foreach(RaycastHit2D hitCollider in hit)
            {
                if (hitCollider.collider != null)
                {
                    float dist = Vector3.Distance(hitCollider.collider.transform.position, player.transform.position);

                    if (hitCollider.collider.gameObject.CompareTag("Ore") && dist <= hitDistance)
                    {
                        OreCollider ore = hitCollider.collider.GetComponent<OreCollider>();
                        ore.HitOre();
                    }
                }
            }
        }
    }
}
