using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class TestMyStackView : MonoBehaviour
    {
        public Toggle TestMyStackToggle => _testMyStackToggle;

        [SerializeField] Toggle _testMyStackToggle;
        [SerializeField] TextMeshProUGUI _toggleText;

        public void SetToggleText(string text)
        {
            _toggleText.text = text;
        }
    }
}
