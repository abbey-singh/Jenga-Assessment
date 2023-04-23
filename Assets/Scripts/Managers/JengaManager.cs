using Constants;
using DataAccessors;
using Models;
using Models.Constants;
using Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Views.Jenga.Factory;
using Zenject;

namespace Managers
{
    public class JengaManager: IInitializable, IDisposable
    {
        public event Action<StackModel> OnSelectedStackChanged;

        private readonly StacksDataAccessor _stackDataAccessor;
        private readonly JengaViewFactory _jengaViewFactory;
        private readonly SignalBus _signalBus;

        private Dictionary<string, List<BlockModel>> _gradeBlockModelsPairs = new Dictionary<string, List<BlockModel>>();
        private List<StackModel> _jengaStacks = new List<StackModel>();

        private GameObject _jengaHolder;
        private StackModel _selectedTower;

        public JengaManager(
            StacksDataAccessor stackDataAccessor,
            JengaViewFactory jengaViewFactory,
            SignalBus signalBus)
        {
            _stackDataAccessor = stackDataAccessor;
            _jengaViewFactory = jengaViewFactory;
            _signalBus = signalBus;

            _signalBus.Subscribe<BlockClickedSignal>(b => UpdateSelectedStack(b.JengaStackTransform));
            _signalBus.Subscribe<TestMyStackSignal>(s => SetTestMyStack(s.IsEnabled));
        }

        public void Dispose()
        {
            _stackDataAccessor.Dispose(); 
            _stackDataAccessor.OnBlockModelsFetched -= OnReceivedBlockModels;
        }

        public void Initialize()
        {
            _jengaHolder = GameObject.Find("JengaHolder");

            _stackDataAccessor.OnBlockModelsFetched += OnReceivedBlockModels;
            _stackDataAccessor.Initialize();
        }

        private void OnReceivedBlockModels(List<BlockModel> blockModels)
        {
            Assert.IsTrue(_gradeBlockModelsPairs.Count == 0, "Already received data");
            ParseListByGrade(blockModels);

            foreach (var gradeStackPair in _gradeBlockModelsPairs)
            {
                StackModel stackModel = _jengaViewFactory.CreateJenga(gradeStackPair.Value, ref _jengaHolder);
                _jengaStacks.Add(stackModel);
            }

            _selectedTower = _jengaStacks[0];
        }

        private void ParseListByGrade(List<BlockModel> blockModels)
        {    
            foreach (var blockModel in blockModels)
            {
                string grade = blockModel.Grade;

                if (!IsValidGrade(grade))
                {
                    Debug.LogWarning($"Received invalid grade '{grade}', ignoring entry");
                    continue;
                }

                if (_gradeBlockModelsPairs.ContainsKey(grade))
                {
                    _gradeBlockModelsPairs[grade].Add(blockModel);
                }
                else
                {
                    _gradeBlockModelsPairs.Add(grade, new List<BlockModel>() { blockModel });
                }
            }
        }

        private bool IsValidGrade(string grade)
        {
            return grade.Contains("Grade");
        }

        private void SetTestMyStack(bool isEnabled)
        {
            _selectedTower.SelectedMode = isEnabled ? StackGameModes.TestMyStack : StackGameModes.None;

            if (isEnabled)
            {
                foreach (var block in _selectedTower.Blocks)
                {
                    if (block.BlockModel.Mastery == BlockTypes.Glass)
                    {
                        block.gameObject.SetActive(false);
                    }

                    block.SetPhysics(true);
                }
            }
            else
            {
                foreach (var block in _selectedTower.Blocks)
                {
                    block.SetPhysics(false);
                    block.ApplyDefaultTransform();

                    block.gameObject.SetActive(true);
                }
            }
        }

        private void UpdateSelectedStack(Transform jengaTowerTransform)
        {
            _selectedTower = _jengaStacks.Find(stack => stack.StackObject.transform == jengaTowerTransform);
            OnSelectedStackChanged?.Invoke(_selectedTower);
        }
    }
}