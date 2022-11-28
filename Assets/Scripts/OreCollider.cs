using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCollider : MonoBehaviour
{
    [SerializeField] private Ore ore;

    public void HitOre()
    {
        ore.HitOre();
    }
}
