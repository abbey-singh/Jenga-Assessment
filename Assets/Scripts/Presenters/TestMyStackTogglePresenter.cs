using Constants;
using Managers;
using Models;
using Signals;
using System;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class TestMyStackTogglePresenter : MonoBehaviour
    {
        [SerializeField] TestMyStackView _testMyStackView;

        private SignalBus _signalBus;
        private JengaManager _jengaManager;

        private bool _isUpdating = false;

        private void Start()
        {
            _testMyStackView.TestMyStackToggle.onValueChanged.AddListener(OnTestMyStackToggleChanged);

            _testMyStackView.SetToggleText("Test my Stack");
        }

        private void OnDestroy()
        {
            _testMyStackView.TestMyStackToggle.onValueChanged.RemoveListener(OnTestMyStackToggleChanged);

            _jengaManager.OnSelectedStackChanged -= OnSelectedStackChanged;
        }

        [Inject]
        private void Init(SignalBus signalBus, JengaManager jengaManager)
        {
            _signalBus = signalBus;
            _jengaManager = jengaManager;

            _jengaManager.OnSelectedStackChanged += OnSelectedStackChanged;
        }

        private void OnSelectedStackChanged(StackModel stackModel)
        {
            bool isTesting = stackModel.SelectedMode == StackGameModes.TestMyStack;

            _isUpdating = true;
            UpdateText(isTesting);
            _testMyStackView.TestMyStackToggle.isOn = isTesting;
            _isUpdating = false;
        }

        private void OnTestMyStackToggleChanged(bool isToggled)
        {
            if (_isUpdating) return;

            UpdateText(isToggled);

            var signal = new TestMyStackSignal(isToggled);
            _signalBus.Fire(signal);
        }

        private void UpdateText(bool isToggled)
        {
            if (isToggled)
            {
                _testMyStackView.SetToggleText("Stop Testing");
            }
            else
            {
                _testMyStackView.SetToggleText("Test my Stack");
            }
        }
    }
}
