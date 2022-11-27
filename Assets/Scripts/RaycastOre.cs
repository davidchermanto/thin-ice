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
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);

            if (hit.collider != null)
            {
                float dist = Vector3.Distance(hit.collider.transform.position, player.transform.position);

                if (hit.collider.gameObject.CompareTag("Ore") && dist <= hitDistance)
                {
                    Ore ore = hit.collider.GetComponent<Ore>();
                    ore.VibrateDamage();

                    Debug.Log("hit");
                }
            }
        }
    }
}
