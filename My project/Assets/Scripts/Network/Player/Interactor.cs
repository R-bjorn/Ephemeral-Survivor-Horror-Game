using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

interface IInteractable
{
    public void Interact();
}

public class Interactor : NetworkBehaviour
{
    public float InteractRange;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, InteractRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    Debug.Log("TEST.");
                    interactObj.Interact();
                }
            }
        }
    }
}
