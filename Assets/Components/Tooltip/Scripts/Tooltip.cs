using System;
using UnityEngine;

namespace PocketHeroes
{
    public class Tooltip : MonoBehaviour
    {
        [Serializable]
        private struct _Config
        {
            public TooltipPanel Panel;
        }

        [Header("Config")]
        [SerializeField] private _Config _config;

        private ITooltipSubject _subject;

        public void Initialize(ITooltipSubject subject)
        {
            _subject = subject;
        }

        public void Show()
        {
            string[] rows = _subject.GetTooltipRows();
            _config.Panel.Show(rows);
        }
    }
}