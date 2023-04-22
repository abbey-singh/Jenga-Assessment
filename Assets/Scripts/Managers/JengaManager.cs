using DataAccessors;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Views.Jenga.Factory;
using Zenject;

namespace Managers
{
    public class JengaManager: IInitializable, IDisposable
    {
        private readonly StacksDataAccessor _stackDataAccessor;
        private readonly JengaViewFactory _jengaViewFactory;

        private Dictionary<string, List<BlockModel>> _gradeBlockModelsPairs = new Dictionary<string, List<BlockModel>>();
        private List<GameObject> _jengaStacks = new List<GameObject>();

        private GameObject _jengaHolder;

        public JengaManager(StacksDataAccessor stackDataAccessor, JengaViewFactory jengaViewFactory)
        {
            _stackDataAccessor = stackDataAccessor;
            _jengaViewFactory = jengaViewFactory;    
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
                GameObject jenga = _jengaViewFactory.CreateJenga(gradeStackPair.Value, ref _jengaHolder);
                _jengaStacks.Add(jenga);
            }
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
    }
}