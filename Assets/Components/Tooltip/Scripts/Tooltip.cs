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

        [SerializeField]
        [RequireInterface(typeof(ITooltipSubject))]
        private UnityEngine.Object _subject;
        public ITooltipSubject Subject => _subject as ITooltipSubject;

        [Header("Config")]
        [SerializeField] private _Config _config;

        public void Show()
        {
            string[] rows = Subject.GetTooltipRows();
            _config.Panel.Show(rows);
        }
    }
}