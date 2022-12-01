using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private GameObject tutorialCanvas;

    [SerializeField] private TextMeshProUGUI wealthNum;
    [SerializeField] private TextMeshProUGUI frostNum;
    [SerializeField] private TextMeshProUGUI depthNum;
    [SerializeField] private TextMeshProUGUI sonarNum;
    [SerializeField] private TextMeshProUGUI constructNum;

    [SerializeField] private TextMeshProUGUI lastScore;
    [SerializeField] private TextMeshProUGUI highScore;

    // Gems
    [Header("Wealth Menu")]
    [SerializeField] private TextMeshProUGUI emeraldNum;
    [SerializeField] private TextMeshProUGUI emeraldValue;
    [SerializeField] private TextMeshProUGUI silverNum;
    [SerializeField] private TextMeshProUGUI silverValue;
    [SerializeField] private TextMeshProUGUI goldNum;
    [SerializeField] private TextMeshProUGUI goldValue;
    [SerializeField] private TextMeshProUGUI elecNum;
    [SerializeField] private TextMeshProUGUI elecValue;
    [SerializeField] private TextMeshProUGUI molNum;
    [SerializeField] private TextMeshProUGUI molValue;
    [SerializeField] private TextMeshProUGUI rubyNum;
    [SerializeField] private TextMeshProUGUI rubyValue;
    [SerializeField] private TextMeshProUGUI diaNum;
    [SerializeField] private TextMeshProUGUI diaValue;
    [SerializeField] private TextMeshProUGUI stelNum;
    [SerializeField] private TextMeshProUGUI stelValue;

    public void NewScore(int score)
    {
        lastScore.SetText(score + "");
    }

    public void TurnOnTutorial()
    {
        StartCoroutine(TutorialOn());
    }

    private IEnumerator TutorialOn()
    {
        MusicManager.Instance.Play(0);

        float blackTime = 0.3f;
        float blackHold = 0.5f;

        float time = 0;
        while(time < 1)
        {
            time += Time.deltaTime / blackTime;
            blackScreen.alpha = Mathf.Lerp(0, 1, time);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(blackHold);
        tutorialCanvas.SetActive(true);

        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime / blackTime;
            blackScreen.alpha = Mathf.Lerp(1, 0, time);

            yield return new WaitForEndOfFrame();
        }
    }

    public void TurnOffTutorial()
    {
        StartCoroutine(TutorialOff());
    }

    private IEnumerator TutorialOff()
    {
        float blackTime = 0.3f;
        float blackHold = 0.5f;

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime / blackTime;
            blackScreen.alpha = Mathf.Lerp(0, 1, time);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(blackHold);
        tutorialCanvas.SetActive(false);

        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime / blackTime;
            blackScreen.alpha = Mathf.Lerp(1, 0, time);

            yield return new WaitForEndOfFrame();
        }
    }
    
    public void UpdateGameValues(int wealth, float frost, float depth, int sonar, int construct)
    {
        wealthNum.SetText("$" + wealth);
        frostNum.SetText(System.Math.Round(frost, 1) + "%");
        depthNum.SetText(depth + "m");
        sonarNum.SetText(sonar + "");
        constructNum.SetText(construct + "");
    }

    public void UpdateWealthMenu(int emeNum, int emeVal, int silNum, int silVal, int golNum, int golVal, int eleNum,
        int eleVal, int moNum, int moVal, int rubNum, int rubVal, int diNum, int diVal, int steNum, int steVal)
    {
        emeraldNum.SetText(emeNum + "");
        emeraldValue.SetText("$" + emeVal);
        silverNum.SetText(silNum + "");
        silverValue.SetText("$" + silVal);
        goldNum.SetText(golNum + "");
        goldValue.SetText("$" + golVal);
        elecNum.SetText(eleNum + "");
        elecValue.SetText("$" + eleVal);
        molNum.SetText(moNum + "");
        molValue.SetText("$" + moVal);
        rubyNum.SetText(rubNum + "");
        rubyValue.SetText("$" + rubVal);
        diaNum.SetText(diNum + "");
        diaValue.SetText("$" + diVal);
        stelNum.SetText(steNum + "");
        stelValue.SetText("$" + steVal);
    }
}
