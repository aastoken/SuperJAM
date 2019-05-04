using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data.

public class ButtonRotate : MonoBehaviour
{
    public DropPointLogic dropPointLogic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pointer()
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");
        dropPointLogic.ExecuteClick();
    }

    private void OnMouseDown()
    {
        Debug.LogWarning("aaaaJODEEEER");

    }
}
