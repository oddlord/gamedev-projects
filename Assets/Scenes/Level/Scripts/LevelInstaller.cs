using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("Services")]
        [SerializeField] private ActorController _actorControllerPrefab;
        [SerializeField] private ObstacleManager _obstacleManagerPrefab;
        [SerializeField] private ObstacleSpawner _obstacleSpawnerPrefab;

        [Header("States")]
        [SerializeField] private IntState _maxLivesState;
        [SerializeField] private IntState _livesState;
        [SerializeField] private IntState _scoreState;

        [Header("Obstacles")]
        [SerializeField] private List<Obstacle> _waveObstaclePrefabs;
        [SerializeField] private SpawnPointsContainer _spawnPointsContainer;

        public override void InstallBindings()
        {
            Container.BindFactory<UnityEngine.Object, Actor, Actor.Factory>().FromFactory<PrefabFactory<Actor>>();
            Container.BindFactory<UnityEngine.Object, Obstacle, Obstacle.Factory>().FromFactory<PrefabFactory<Obstacle>>();

            Container.Bind<ActorController>().FromComponentInNewPrefab(_actorControllerPrefab).AsSingle();
            Container.Bind<ObstacleManager>().FromComponentInNewPrefab(_obstacleManagerPrefab).AsSingle();
            Container.Bind<ObstacleSpawner>().FromComponentInNewPrefab(_obstacleSpawnerPrefab).AsSingle();

            Container.BindInstance(_maxLivesState).WithId(LevelInjectIds.MAX_LIVES_STATE);
            Container.QueueForInject(_maxLivesState);

            Container.BindInstance(_livesState).WithId(LevelInjectIds.LIVES_STATE);
            Container.QueueForInject(_livesState);

            Container.BindInstance(_scoreState).WithId(LevelInjectIds.SCORE_STATE);
            Container.QueueForInject(_scoreState);

            Container.BindInstance(_waveObstaclePrefabs);
            Container.QueueForInject(_waveObstaclePrefabs);

            Container.BindInstance(_spawnPointsContainer);
            Container.QueueForInject(_spawnPointsContainer);
        }
    }
}