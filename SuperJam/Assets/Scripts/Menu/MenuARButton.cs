using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuARButton : MonoBehaviour
{
    TextMeshPro tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
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
            SceneManager.LoadScene(2);
            //SceneManager.LoadScene("MainScene");
        }

    }
}
