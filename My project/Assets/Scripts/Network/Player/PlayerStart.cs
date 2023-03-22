using Cinemachine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Network.Player;

public class PlayerStart : NetworkBehaviour
{
    public GameObject main;
    public GameObject follow;

    void Start()
    {
        // Other players will not have input / camera
        // TODO - change this, this is just temp fix
        Invoke("checkOwner", 1);
        
    }

    void checkOwner() {
        if (!IsOwner) return;
        // https://forum.unity.com/threads/isowner-is-not-set-to-true-when-spawning-object-with-ownership.1223982/

        // Enable cameras
        main.GetComponent<Camera>().enabled = true;
        main.GetComponent<AudioListener>().enabled = true;
        main.GetComponent<CinemachineBrain>().enabled = true;
        follow.GetComponent<CinemachineVirtualCamera>().enabled = true;

        // Enable player input after components are enabled
        GetComponent<PlayerInput>().enabled = true;
        GetComponent<NetworkThirdPersonController>().enabled = true;
    }
}