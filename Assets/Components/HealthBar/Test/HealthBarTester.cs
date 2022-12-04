using UnityEngine;
using TMPro;

namespace PocketHeroes
{
    public class HealthBarTester : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private HealthBar _healthBar;

        public void SetFill()
        {
            bool parseSuccessful = float.TryParse(_inputField.text, out float percentage);
            if (!parseSuccessful) return;

            _healthBar.SetFill(percentage, true);
        }

        public void AnimateFill()
        {
            bool parseSuccessful = float.TryParse(_inputField.text, out float percentage);
            if (!parseSuccessful) return;

            _healthBar.SetFill(percentage);
        }
    }
}