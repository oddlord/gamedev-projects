namespace SpaceMiner
{
    public interface IActorController
    {
        public IActor Actor { get; set; }

        public void SetActor(IActor actor)
        {
            Actor = actor;
        }
    }
}