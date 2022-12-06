using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PocketHeroes
{
    // TODO make a Unit superclass so that the MonsterUnit can extend it too
    public class HeroUnit : CharacterUnit
    {
        private Hero Hero => Character as Hero;

        public override void OnBattleOver(bool won)
        {
            base.OnBattleOver(won);
            if (!IsDead) Hero.GainExperience();
        }

        protected override string[] GetTooltipRows()
        {
            return new string[]{
                $"Name: {Character.Name}",
                $"Health: {CurrentHealth}/{Character.Health}",
                $"Attack Power: {Character.AttackPower}",
                $"Level: {Hero.Level}",
                $"Experience: {Hero.Experience}",
            };
        }
    }
}