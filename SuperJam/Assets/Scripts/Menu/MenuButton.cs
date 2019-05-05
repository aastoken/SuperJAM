using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButton : MonoBehaviour
{

    #region Public
    #endregion

    #region Private
    TextMeshPro tmp;

    #endregion

    #region Monobehaviour

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        
       
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");

        if(gm)
            Destroy(gm);
    }

    private void OnMouseOver()
    {
        tmp.color = new Color32(255, 134, 124, 255);
    }

    private void OnMouseExit()
    {
        tmp.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (gameObject.tag == "ExitButton")
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
            
        if (gameObject.tag == "NewGameButton")
        {
            Debug.Log("Loading new game...");
            Application.LoadLevel(1);
            //SceneManager.LoadScene("MainScene");
        }
            
    }

    #endregion

    #region Methods

    #endregion
}

