using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomSpawnPoint()
    {
        //return new Vector3(Random.Range(-20, 20), 4, Random.Range(-20, 20));
        return new Vector3(Random.Range(-5, 5), 4, Random.Range(-5, 5));
    }
}
