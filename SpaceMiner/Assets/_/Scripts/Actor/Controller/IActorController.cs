namespace SpaceMiner
{
    public interface IActorController
    {
        public Actor Actor { get; set; }

        public void SetActor(Actor actor)
        {
            Actor = actor;
        }
    }
}