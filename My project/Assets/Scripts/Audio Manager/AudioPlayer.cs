using System;
using UnityEngine;

namespace Audio_Manager
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField][Tooltip("The player in the game scene.")]
        private Transform player;
        [SerializeField][Tooltip("maximum distance the player can here this sound.")]
        private float maxDistance = 20f;
        [SerializeField][Tooltip("minimum distance the player can fully here this sound.")]
        private float minDistance = 1f;
        [SerializeField][Tooltip("The audio source that is played by the script")]
        private AudioSource audioSource;

        private void Start()
        {
            GetComponent<AudioSource>().volume = 0;
            // player = GetComponent<Transform>()
        }

        private void Update()
        {
            // Getting the distance between audio source object and player and use mathf.lerp to get a value between 0-1 which is the audio and the t = (distance - minDist) / (maxDist - minDIst)
            float volume = Mathf.Lerp(1f, 0f, (Vector3.Distance(transform.position, player.transform.position) - minDistance) / (maxDistance - minDistance));
            volume = Mathf.Clamp(volume, 0f, 1f);
            GetComponent<AudioSource>().volume = volume;
        }
    }
}
