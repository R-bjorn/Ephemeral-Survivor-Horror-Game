using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    bool muted = false;
    public void MuteAudio () {
        if(!muted) {
            // Mute audio
            GetComponent<AudioSource>().volume = 0;
            muted = true;
        }
        else {
            // Restore audio
            GetComponent<AudioSource>().volume = 1;
            muted = false;
        }
    }
}
