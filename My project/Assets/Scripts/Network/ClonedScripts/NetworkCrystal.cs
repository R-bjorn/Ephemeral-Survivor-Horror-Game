using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkCrystal : NetworkBehaviour
{
    public static Crystal Instance { get; private set; }
    public int crystalValue = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NetworkScoreManager.Instance.AddScore(crystalValue);
            Destroy(gameObject);
        }
    }
}
