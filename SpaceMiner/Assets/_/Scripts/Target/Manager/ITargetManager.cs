using System;

namespace SpaceMiner
{
    public interface ITargetManager
    {
        public Action<Target> OnTargetDestroyed { get; set; }
        public Action OnAllTargetsDestroyed { get; set; }
    }
}