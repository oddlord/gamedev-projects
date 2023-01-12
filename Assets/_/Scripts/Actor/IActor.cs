using System;
using UnityEngine;
using Zenject;

namespace SpaceMiner
{
    public interface IActor
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, IActor> { }

        public ObservableInt Lives { get; set; }
        public ObservableInt MaxLives { get; set; }

        public Action<IActor> OnDeath { get; set; }

        public void HandleForwardInput(float amount);
        public void HandleSideInput(float amount);
        public void Attack();

        public Sprite GetSprite();

        // Ensures that any implementation is a MonoBehaviour
        public GameObject GetGO();

        public bool IsDead => Lives.Value <= 0;
    }
}
