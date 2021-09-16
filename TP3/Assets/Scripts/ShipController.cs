using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private bool crashed = false;
    public AudioSource playingAudioSource;
    public AudioSource crashAudioSource;
    public AudioSource lostAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, -180, 0);
        crashAudioSource.Stop();
        lostAudioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        crashed = true;
        double crashDuration = (double)crashAudioSource.clip.samples / crashAudioSource.clip.frequency;
        double startTime = AudioSettings.dspTime;
        playingAudioSource.Stop();
        crashAudioSource.PlayScheduled(startTime);
        lostAudioSource.PlayScheduled(startTime + crashDuration);
    }

    public bool hasCrashed(){
        return crashed;
    }

    public void restart(){
        transform.Rotate(0, -180, 0);
        crashAudioSource.Stop();
        lostAudioSource.Stop();
        playingAudioSource.PlayScheduled(AudioSettings.dspTime);
        crashed = false;
    }
}
