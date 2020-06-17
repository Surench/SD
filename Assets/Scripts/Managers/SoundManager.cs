using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource PerfectAudio;

    public static int perfectSoundTimes = 0;

    public void InitSoundManager()
    {
        fixPitch();
    }

    public void StairSound()
    {
        if (SoundEnabled) perfectSoundTimes += 1;

    }

    public void StopSound()
    {
        perfectSoundTimes  = 0;
    }

    public void Start()
    {
        StartCoroutine(StairSoundRoutine());
    }

    IEnumerator StairSoundRoutine()
    {

        while (true)
        {

            if (SoundEnabled && perfectSoundTimes > 0)
            {
                float pitch = PerfectAudio.pitch;

                pitch += 0.1f;
                if (pitch < 4f) PerfectAudio.pitch = pitch;

                PerfectAudio.Play();

                perfectSoundTimes -= 1;

            }

            yield return new WaitForSeconds(0.08f);
        }

    }


    public void fixPitch()
    {
        PerfectAudio.pitch = 0.9f;
    }



    [SerializeField]
    private GameObject SoundLines;
    [SerializeField]
    private GameObject MuteLine;

    public static bool SoundEnabled = true;
    public void ToggleSound()
    {
        SoundEnabled = !SoundEnabled;

        MuteLine.SetActive(!SoundEnabled);
        SoundLines.SetActive(SoundEnabled);

    }

}
