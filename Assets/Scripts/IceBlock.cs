using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D iceCollider;

    // 0 to 100. When reaches 0, ice dies
    [SerializeField] private float durability;

    // What number of ice is this
    [SerializeField] private int id;

    [SerializeField] private Color color;

    void InitIce(int id, float durability)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
