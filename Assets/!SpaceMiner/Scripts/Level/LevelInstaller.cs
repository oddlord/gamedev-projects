using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LevelInstaller : MonoInstaller
    {
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

            Container.Bind<IActorController>().To<PlayerActorController>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Bind<IObstacleManager>().To<SimpleObstacleManager>().AsSingle();
            Container.Bind<IObstacleSpawner>().To<SimpleObstacleSpawner>().AsSingle();

            Container.BindInstance(_maxLivesState).WithId(LevelInjectIds.MAX_LIVES_STATE);
            Container.BindInstance(_livesState).WithId(LevelInjectIds.LIVES_STATE);
            Container.BindInstance(_scoreState).WithId(LevelInjectIds.SCORE_STATE);

            Container.BindInstance(_waveObstaclePrefabs);
            Container.BindInstance(_spawnPointsContainer);
        }
    }
}