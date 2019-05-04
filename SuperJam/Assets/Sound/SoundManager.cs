using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.

    //MOVEMENT
    public AudioClip robotMovementSource1;
    public AudioClip robotMovementSource2;

    //Music
    public AudioClip MainTheme1;

    //JOINTS
    public AudioClip robotJointSource1;
    public AudioClip robotJointSource2;


    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.


    void Start()
    {
        PlayMusic(MainTheme1);

    }
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// Plays the single clip
    /// </summary>
    /// <param name="clip">Clip.</param>
    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = clip;

        //Play the clip.
        efxSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        musicSource.clip = clip;

        //Play the clip.
        musicSource.Play();
    }


    /// <summary>
    /// Randomizes the sfx.
    /// </summary>
    /// <param name="clips">Clips.</param>
    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = clips[randomIndex];

        //Play the clip.
        efxSource.Play();
    }

    /// <summary>
    /// Plays the robot sound movement (call from RobotManager).
    /// </summary>
    public void PlayRobotSoundMovement()
    {
        int randomSound = Random.Range(0, 2);

        if(randomSound==0)
        {
            instance.PlaySingle(robotMovementSource1);
        }
        else if(randomSound==1)
        {
            instance.PlaySingle(robotMovementSource2);
        }
        else
        {
            instance.PlaySingle(robotMovementSource1);
        }
             
    }

    /// <summary>
    /// Plays the robot joints
    /// </summary>
    public void PlayRobotSoundJoint()
    {
        int randomSound = Random.Range(0, 2);
        Debug.Log("!");
        if (randomSound == 0)
        {
            instance.PlaySingle(robotJointSource1);
        }
        else if (randomSound == 1)
        {
            instance.PlaySingle(robotJointSource2);
        }
        else
        {
            instance.PlaySingle(robotJointSource1);
        }

    }

    public void PlayRobotSpawn()
    {            
        instance.PlaySingle(robotJointSource2);         
    }


}
