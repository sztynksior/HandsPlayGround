using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> _objectsToSpawn = new List<GameObject>();
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();

    public void SpawnRandomObject()
    {
        if (_objectsToSpawn.Count == 0)
        {
            return;
        }

        int randomObject = new System.Random().Next(0, this._objectsToSpawn.Count);
        int randomSpawnPoint = new System.Random().Next(0, this._spawnPoints.Count);

        Instantiate(this._objectsToSpawn[randomObject], this._spawnPoints[randomSpawnPoint].position, this._spawnPoints[randomSpawnPoint].rotation);
    }
}
