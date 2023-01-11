using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("Instances")]
        [SerializeField] private List<Obstacle> _waveObstaclePrefabs;
        [SerializeField] private SpawnPointsContainer _spawnPointsContainer;
        [SerializeField] private LivesDisplay _livesDisplay;

        public override void InstallBindings()
        {
            Container.BindFactory<UnityEngine.Object, Actor, Actor.Factory>().FromFactory<PrefabFactory<Actor>>();
            Container.BindFactory<UnityEngine.Object, Obstacle, Obstacle.Factory>().FromFactory<PrefabFactory<Obstacle>>();

            Container.Bind<IActorController>().To<PlayerActorController>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Bind<IObstacleManager>().To<SimpleObstacleManager>().AsSingle();
            Container.Bind<IObstacleSpawner>().To<SimpleObstacleSpawner>().AsSingle();

            Container.Bind<ObservableInt>().AsSingle();

            Container.BindInstance(_waveObstaclePrefabs);
            Container.BindInstance(_spawnPointsContainer);
            Container.BindInstance(_livesDisplay);
        }
    }
}