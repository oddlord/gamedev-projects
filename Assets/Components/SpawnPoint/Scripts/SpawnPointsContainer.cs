using UnityEngine;

namespace SpaceMiner
{
    public class SpawnPointsContainer : MonoBehaviour
    {
        [HideInInspector] public SpawnPoint[] SpawnPoints;

        void Awake()
        {
            SpawnPoints = GetComponentsInChildren<SpawnPoint>();
        }
    }
}