using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PocketHeroes
{
    public class CharacterTooltip : MonoBehaviour, IPointerClickHandler
    {
        [Serializable]
        private struct _Config
        {
            public TextMeshProUGUI Content;
        }

        [Header("Config")]
        [SerializeField] private _Config _config;

        public void Initialize(string[] rows)
        {
            gameObject.SetActive(true);

            _config.Content.text = "";
            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                _config.Content.text += row;
                if (i < rows.Length - 1) _config.Content.text += "\n";
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }
    }
}