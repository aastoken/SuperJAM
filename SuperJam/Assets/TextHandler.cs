using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHandler : MonoBehaviour
{
    #region Public
    public GameManager gameManager;
    public TextMeshProUGUI lives;
    public TextMeshProUGUI finalScore;
    #endregion

    #region Private
    TextMeshProUGUI tmp;
    GameObject scoreString;
    GameObject finishedGame;
    bool isFinalScorePrinted;
    float screenWidth;
    float screenHeight;
    float percentageToMove;
    #endregion

    #region Monobehaviour


    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        isFinalScorePrinted = false;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        percentageToMove = screenWidth * 0.01f;
    }

    void Update()
    {
        tmp.SetText(gameManager.score.ToString());
        lives.SetText(gameManager.GetHealth().ToString());        

        if (Input.GetKeyDown(KeyCode.Alpha9))
            gameManager.score += 100;
    }

    public void FinishScreen()
    {
        Debug.Log("BRO SOS FAMOSO!!1");
        for (int i = 0; i < transform.parent.childCount; i++)
            transform.parent.GetChild(i).gameObject.SetActive(false);

        if(!isFinalScorePrinted)
        finalScore.SetText(finalScore.text + gameManager.score.ToString());

        isFinalScorePrinted = true;

        finalScore.gameObject.SetActive(true);
    }

    #endregion

}
