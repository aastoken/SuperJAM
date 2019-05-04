using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip MainTheme1;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySingle(MainTheme1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
