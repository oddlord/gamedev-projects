using System;
using System.Collections.Generic;
using System.Linq;
using Oddlord.RequireInterface;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LevelInstaller : MonoInstaller
    {
        [Serializable]
        private struct _IObstacleListElement
        {
            [RequireInterface(typeof(IObstacle))]
            [SerializeField] private UnityEngine.Object _obstacle;
            public IObstacle Obstacle => _obstacle as IObstacle;
        }

        [Header("Instances")]
        [SerializeField] private _IObstacleListElement[] _waveObstaclePrefabs;
        private IObstacle[] _iWaveObstaclePrefabs => _waveObstaclePrefabs.Select(a => a.Obstacle).ToArray();
        [SerializeField] private SpawnPointsContainer _spawnPointsContainer;
        [SerializeField] private LivesDisplay _livesDisplay;

        public override void InstallBindings()
        {
            Container.BindFactory<UnityEngine.Object, IActor, IActor.Factory>().FromFactory<PrefabFactory<IActor>>();
            Container.BindFactory<UnityEngine.Object, IObstacle, IObstacle.Factory>().FromFactory<PrefabFactory<IObstacle>>();

            Container.Bind<IActorController>().To<PlayerActorController>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Bind<IObstacleManager>().To<SimpleObstacleManager>().AsSingle();
            Container.Bind<IObstacleSpawner>().To<SimpleObstacleSpawner>().AsSingle();

            Container.Bind<ObservableInt>().AsSingle();

            Container.BindInstance(_iWaveObstaclePrefabs);
            Container.BindInstance(_spawnPointsContainer);
            Container.BindInstance(_livesDisplay);
        }
    }
}