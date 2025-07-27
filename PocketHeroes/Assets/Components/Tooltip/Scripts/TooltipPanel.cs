using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PocketHeroes
{
    public class TooltipPanel : MonoBehaviour, IPointerClickHandler
    {
        [Serializable]
        private struct _Config
        {
            public TextMeshProUGUI Content;
            public VerticalLayoutGroup LayoutGroup;
        }

        [Header("Config")]
        [SerializeField] private _Config _config;

        public void Show(string[] rows)
        {
            gameObject.SetActive(true);

            _config.Content.text = "";
            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                _config.Content.text += row;
                if (i < rows.Length - 1) _config.Content.text += "\n";
            }

            RefreshContentSizeFitter();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Close();
        }

        public void OnMouseDown()
        {
            Close();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void RefreshContentSizeFitter()
        {
            Canvas.ForceUpdateCanvases();
            _config.LayoutGroup.enabled = false;
            _config.LayoutGroup.enabled = true;
        }
    }
}