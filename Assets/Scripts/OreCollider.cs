using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCollider : MonoBehaviour
{
    [SerializeField] public Ore ore;
    public bool visualActive;

    public void HitOre()
    {
        ore.HitOre();
    }
}
