using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DataAccessors
{
    public class StackDataAccessor: IInitializable, IDisposable
    {
        private readonly NetworkingOperations _networkingOperations;

        public StackDataAccessor(NetworkingOperations networkingOperations)
        {
            _networkingOperations = networkingOperations;
        }

        public void Dispose()
        {
            _networkingOperations.OnStackDataFetched -= OnDataFetched;
        }

        public void Initialize()
        {
            _networkingOperations.OnStackDataFetched += OnDataFetched;
            _networkingOperations.StartFetchingStackData();
        }

        private void OnDataFetched(List<StackModel> stackModels)
        {
            Debug.Log(stackModels);
        }
    }
}
