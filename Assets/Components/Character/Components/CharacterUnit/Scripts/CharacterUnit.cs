using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PocketHeroes
{
    public class CharacterUnit : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public HealthBar HealthBar;
            public TextMeshPro Name;
            public Tooltip Tooltip;
        }

        private const float _LONG_PRESS_DURATION = 3;
        private const float _ATTACK_MOVE_TIME = 0.5f;

        public Character Character;
        public int CurrentHealth;

        [Header("Config")]
        [SerializeField] private _Config _config;

        // TODO refactor so that this functionality is shared between Units and HeroCards
        public Action<CharacterUnit> OnPress;

        private float _lastPressDownTime;

        public void Initialize(Character character)
        {
            Character = character;
            _lastPressDownTime = 0;

            _config.Name.text = character.Name;
            SetCurrentHealth(character.Health);
        }

        public void Attack(CharacterUnit enemyUnit, Action onAttackFinished)
        {
            StartCoroutine(AttackCoroutine(enemyUnit, onAttackFinished));
        }

        public void GetDamaged(int damageAmount)
        {
            SetCurrentHealth(CurrentHealth - damageAmount, true);
        }

        public virtual void OnBattleOver(bool won) { }

        public bool IsDead => CurrentHealth == 0;

        // TODO this function doesn't belong with either the CharacterUnit or the tooltip classes
        // Find the proper place for this function, same for the other similar functions
        protected virtual string[] GetTooltipRows()
        {
            return new string[]{
                $"Name: {Character.Name}",
                $"Health: {CurrentHealth}/{Character.Health}",
                $"Attack Power: {Character.AttackPower}",
            };
        }

        private void SetCurrentHealth(int health, bool animate = false)
        {
            CurrentHealth = Math.Max(0, health);
            _config.HealthBar.SetFill(CurrentHealth, Character.Health, !animate);
        }

        private IEnumerator AttackCoroutine(CharacterUnit enemyUnit, Action onAttackFinished)
        {
            Vector3 initialPosition = transform.position;

            float t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime / _ATTACK_MOVE_TIME;
                // TODO use some nicer easing here rather than a linear one
                Vector3 position = Vector3.Lerp(initialPosition, enemyUnit.transform.position, t);
                transform.position = position;
                yield return null;
            }

            enemyUnit.GetDamaged(Character.AttackPower);

            Vector3 attackPosition = transform.position;
            t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime / _ATTACK_MOVE_TIME;
                Vector3 position = Vector3.Lerp(attackPosition, initialPosition, t);
                transform.position = position;
                yield return null;
            }

            onAttackFinished();
        }

        public void OnMouseDown()
        {
            _lastPressDownTime = Time.time;
        }

        public void OnMouseUp()
        {
            if (_lastPressDownTime == 0) return;

            if (Time.time - _lastPressDownTime < _LONG_PRESS_DURATION) OnPress?.Invoke(this);
            else _config.Tooltip.Initialize(GetTooltipRows());
        }
    }
}