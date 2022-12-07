using System.Collections;
using Oddlord.Easing;
using UnityEngine;

namespace PocketHeroes
{
    public class HeroUnit : Unit
    {
        private const float _DELTA_FADE_TIME = 5f;

        private Hero _hero;
        private Coroutine _attributeChangeCoroutine;

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            // TODO refactor to avoid this nasty cast
            _hero = Character as Hero;
            _hero.OnExperienceGain += OnHeroExperienceGained;
        }

        public override void OnBattleOver(bool won)
        {
            base.OnBattleOver(won);
            if (!IsDead) _hero.GainExperience();
        }

        public override string[] GetTooltipRows()
        {
            return new string[]{
                $"Name: {Character.Name}",
                $"Health: {CurrentHealth}/{Character.Health}",
                $"Attack Power: {Character.AttackPower}",
                $"Level: {_hero.Level}",
                $"Experience: {_hero.Experience}",
            };
        }

        private void OnHeroExperienceGained(int experienceGained, int healthDelta, int attackPowerDelta, int levelDelta)
        {
            string attributeChangeStr = "";
            if (experienceGained > 0) attributeChangeStr += $"Experience Gained: +{experienceGained}\n";
            if (healthDelta > 0) attributeChangeStr += $"Health: +{healthDelta}\n";
            if (attackPowerDelta > 0) attributeChangeStr += $"Attack Power: +{attackPowerDelta}\n";
            if (levelDelta > 0) attributeChangeStr += $"Level: +{levelDelta}\n";
            attributeChangeStr = attributeChangeStr.TrimEnd('\n');

            if (_attributeChangeCoroutine != null) StopCoroutine(_attributeChangeCoroutine);
            _attributeChangeCoroutine = StartCoroutine(ShowAttributeChangeCoroutine(attributeChangeStr));
        }

        private IEnumerator ShowAttributeChangeCoroutine(string attributeChangeStr)
        {
            _config.AttributeChangeText.text = attributeChangeStr;
            SetAttributeChangeTextAlpha(1);
            SetActiveAttributeChangeText(true);

            float t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime / _DELTA_FADE_TIME;
                float alpha = Easing.Lerp(1, 0, t, EasingCurve.EaseInQuart);
                SetAttributeChangeTextAlpha(alpha);

                yield return null;
            }

            SetActiveAttributeChangeText(false);
            _attributeChangeCoroutine = null;
        }

        private void SetActiveAttributeChangeText(bool active)
        {
            _config.AttributeChangeText.gameObject.SetActive(active);
        }

        private void SetAttributeChangeTextAlpha(float alpha)
        {
            Color deltaColor = _config.AttributeChangeText.color;
            deltaColor.a = alpha;
            _config.AttributeChangeText.color = deltaColor;
        }

        void OnDestroy()
        {
            _hero.OnExperienceGain -= OnHeroExperienceGained;
        }
    }
}