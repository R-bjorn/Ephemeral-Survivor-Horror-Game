using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        Debug.Log("0");
        // NO CLIENTS FOUND ??
        foreach (var client in ServerManager.Instance.ClientData)
        {
            Debug.Log("1");
            var character = characterDatabase.GetCharacterById(client.Value.characterId);
            Debug.Log("2");
            if (character != null)
            {
                Debug.Log("3");
                var spawnPos = new Vector3(Random.Range(-41f, -35f), 4f, Random.Range(125f, 132f));
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
                Debug.Log("4");
                //characterInstance.SpawnWithOwnership(client.Value.clientId);
            }
        }
    }
}
