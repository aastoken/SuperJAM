using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHandler : MonoBehaviour
{
    #region Public
    public GameManager gameManager;
    #endregion

    #region Private
    TextMeshProUGUI tmp;
    GameObject scoreString;
    GameObject finishedGame;
    int digits;
    int newDigits;
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
        digits = 0;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        percentageToMove = screenWidth * 0.01f;
    }

    void Update()
    {
        tmp.SetText(gameManager.score.ToString());

        newDigits = gameManager.score.ToString().Length;

        if(digits < newDigits)
        {
            digits = newDigits;
            transform.position -= new Vector3(percentageToMove , 0.0f, 0.0f);
        }
        

        if (Input.GetKeyDown(KeyCode.Alpha9))
            gameManager.score += 100;
    }

    #endregion

}
