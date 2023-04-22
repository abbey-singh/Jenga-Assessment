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
    public class StacksDataAccessor
    {
        public event Action<List<BlockModel>> OnBlockModelsFetched;

        private readonly NetworkingOperations _networkingOperations;

        public StacksDataAccessor(NetworkingOperations networkingOperations)
        {
            _networkingOperations = networkingOperations;
        }

        public void Dispose()
        {
            _networkingOperations.OnStacksDataFetched -= OnDataFetched;
        }

        public void Initialize()
        {
            _networkingOperations.OnStacksDataFetched += OnDataFetched;
            _networkingOperations.StartFetchingStacksData();
        }

        private void OnDataFetched(List<BlockModel> blockModels)
        {
            OnBlockModelsFetched?.Invoke(blockModels); 
        }
    }
}
