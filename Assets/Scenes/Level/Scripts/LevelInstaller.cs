using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LevelInstaller : MonoInstaller
    {
        [Header("Services")]
        [SerializeField] private ActorController _actorControllerPrefab;

        [Header("States")]
        [SerializeField] private IntState _maxLivesState;
        [SerializeField] private IntState _livesState;
        [SerializeField] private IntState _scoreState;

        public override void InstallBindings()
        {
            Container.BindFactory<UnityEngine.Object, Actor, Actor.Factory>().FromFactory<PrefabFactory<Actor>>();

            Container.Bind<ActorController>().FromComponentInNewPrefab(_actorControllerPrefab).AsSingle();

            Container.BindInstance(_maxLivesState).WithId(LevelInjectIds.MAX_LIVES_STATE);
            Container.QueueForInject(_maxLivesState);

            Container.BindInstance(_livesState).WithId(LevelInjectIds.LIVES_STATE);
            Container.QueueForInject(_livesState);

            Container.BindInstance(_scoreState).WithId(LevelInjectIds.SCORE_STATE);
            Container.QueueForInject(_scoreState);
        }
    }
}