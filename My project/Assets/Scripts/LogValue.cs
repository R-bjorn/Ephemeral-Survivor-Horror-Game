using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LogValue : NetworkBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("test");
    }
}
