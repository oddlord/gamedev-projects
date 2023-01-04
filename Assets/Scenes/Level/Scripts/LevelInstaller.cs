using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public class LevelInstaller : MonoInstaller
    {

        [SerializeField] private IntState _maxLivesState;
        [SerializeField] private IntState _livesState;

        public override void InstallBindings()
        {
            Container.BindFactory<UnityEngine.Object, Actor, Actor.Factory>().FromFactory<PrefabFactory<Actor>>();

            Container.BindInstance(_maxLivesState).WithId(LevelInjectIds.MAX_LIVES_STATE);
            Container.QueueForInject(_maxLivesState);

            Container.BindInstance(_livesState).WithId(LevelInjectIds.LIVES_STATE);
            Container.QueueForInject(_livesState);
        }
    }
}