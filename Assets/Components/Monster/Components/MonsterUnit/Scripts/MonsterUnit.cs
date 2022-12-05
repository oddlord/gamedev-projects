using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PocketHeroes
{
    public class MonsterUnit : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public HealthBar HealthBar;
            public TextMeshPro Name;
            public CharacterTooltip Tooltip;
        }

        private const float _LONG_PRESS_DURATION = 3;
        private const float _ATTACK_MOVE_TIME = 0.5f;

        [Header("Config")]
        [SerializeField] private _Config _config;

        public Monster Monster;
        public int CurrentHealth;

        private float _lastPressDownTime;

        public void Initialize(Monster monster)
        {
            Monster = monster;
            _lastPressDownTime = 0;

            _config.Name.text = monster.Name;
            SetCurrentHealth(monster.Health);
        }

        public void Attack(HeroUnit enemyUnit, Action onAttackFinished)
        {
            StartCoroutine(AttackCoroutine(enemyUnit, onAttackFinished));
        }

        public void GetDamaged(int damageAmount)
        {
            SetCurrentHealth(CurrentHealth - damageAmount, true);
        }

        public bool IsDead => CurrentHealth == 0;

        private void SetCurrentHealth(int health, bool animate = false)
        {
            CurrentHealth = Math.Max(0, health);
            _config.HealthBar.SetFill(CurrentHealth, Monster.Health, !animate);
        }

        private IEnumerator AttackCoroutine(HeroUnit enemyUnit, Action onAttackFinished)
        {
            Vector3 initialPosition = transform.position;

            float t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime / _ATTACK_MOVE_TIME;
                Vector3 position = Vector3.Lerp(initialPosition, enemyUnit.transform.position, t);
                transform.position = position;
                yield return null;
            }

            enemyUnit.GetDamaged(Monster.AttackPower);

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

            if (Time.time - _lastPressDownTime >= _LONG_PRESS_DURATION) _config.Tooltip.Initialize(TooltipUtils.GetMonsterUnitTooltipRows(this));
        }
    }
}