using UnityEngine;
using TMPro;

namespace PocketHeroes
{
    public class HealthBarTester : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _maxInputField;
        [SerializeField] private TMP_InputField _currentInputField;
        [SerializeField] private HealthBar _healthBar;

        public void Initialize()
        {
            bool parseSuccessful = float.TryParse(_maxInputField.text, out float max);
            if (!parseSuccessful) return;

            _healthBar.Initialize(max);
        }

        public void SetFill()
        {
            bool parseSuccessful = float.TryParse(_currentInputField.text, out float amount);
            if (!parseSuccessful) return;

            _healthBar.SetFill(amount);
        }
    }
}