using System;
using System.Linq;
using Oddlord.RequireInterface;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("Instances")]
        [SerializeField] private Target[] _waveTargetPrefabs;
        [SerializeField] private SpawnPointsContainer _spawnPointsContainer;
        [SerializeField] private LivesDisplay _livesDisplay;

        public override void InstallBindings()
        {
            Container.BindFactory<UnityEngine.Object, Actor, Actor.Factory>().FromFactory<PrefabFactory<Actor>>();
            Container.BindFactory<UnityEngine.Object, Target, Target.Factory>().FromFactory<PrefabFactory<Target>>();

            Container.Bind<IActorController>().To<PlayerActorController>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Bind<ITargetManager>().To<TargetCounter>().AsSingle();
            Container.Bind<ITargetSpawner>().To<SpawnPointsTargetSpawner>().AsSingle();

            Container.Bind<ObservableInt>().AsSingle();

            Container.BindInstance(_waveTargetPrefabs);
            Container.BindInstance(_spawnPointsContainer);
            Container.BindInstance(_livesDisplay);
        }
    }
}