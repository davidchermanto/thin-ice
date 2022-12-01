using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wealthNum;
    [SerializeField] private TextMeshProUGUI frostNum;
    [SerializeField] private TextMeshProUGUI depthNum;
    [SerializeField] private TextMeshProUGUI sonarNum;
    [SerializeField] private TextMeshProUGUI constructNum;

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateGameValues(int wealth, float frost, float depth, int sonar, int construct)
    {
        wealthNum.SetText("$" + wealth);
        frostNum.SetText(System.Math.Round(frost, 1) + "%");
        depthNum.SetText(depth + "m");
        sonarNum.SetText(sonar + "");
        constructNum.SetText(construct + "");
    }
}
