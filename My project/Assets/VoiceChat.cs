using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoiceChat : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    bool voiceOn = false;
    public TextMeshProUGUI voiceChatText;
    public void VoiceToggle() {
        voiceOn = !voiceOn;
        voiceChatText = GetComponentInChildren<TextMeshProUGUI>();
        voiceChatText.text = voiceOn ? "Voice On" : "Voice Off";
    }
}
