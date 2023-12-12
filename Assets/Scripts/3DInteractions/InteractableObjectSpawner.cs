using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class InteractableObjectSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> _objectsToSpawn = new List<GameObject>();
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] InteractionSlider _slider;

    public void SpawnRandomObject()
    {
        if (_objectsToSpawn.Count == 0)
        {
            return;
        }

        float spawnedObjectScaleMultipler = _slider.HorizontalSliderPercent;
        spawnedObjectScaleMultipler += 0.5f;
        int randomObjectsToSpawnIndex = new System.Random().Next(0, this._objectsToSpawn.Count);
        int randomSpawnPointsIndex = new System.Random().Next(0, this._spawnPoints.Count);
        GameObject SpawnedObject = Instantiate(
            this._objectsToSpawn[randomObjectsToSpawnIndex], 
            this._spawnPoints[randomSpawnPointsIndex].position, 
            this._spawnPoints[randomSpawnPointsIndex].rotation
            );
        SpawnedObject.transform.localScale *= spawnedObjectScaleMultipler;
    }
}
