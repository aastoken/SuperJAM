using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TextHandler : MonoBehaviour
{
    Text txt;
    public GameManager gameManager;

    void Start()
    {
        txt = GetComponent<Text>();
    }

    void Update()
    {
        txt.text = gameManager.score.ToString();
    }

}
