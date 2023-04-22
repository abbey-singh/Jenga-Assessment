using Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DataAccessors
{
    public class NetworkingOperations
    {
        public event Action<List<StackModel>> OnStackDataFetched;

        private UrlHolder _urlHolder;
        private UnityWebRequest _request;

        public NetworkingOperations(UrlHolder urlHolder)
        {
            _urlHolder = urlHolder;
        }

        public void StartFetchingStackData()
        {
            _request = UnityWebRequest.Get(_urlHolder.GetURL());
            var operation = _request.SendWebRequest();
            operation.completed += FetchStackDataCompleted;
        }

        private void FetchStackDataCompleted(AsyncOperation obj)
        {
            string data = _request.downloadHandler.text;
            List<StackModel> stackModels = JsonConvert.DeserializeObject<List<StackModel>>(data);
            _request = null;

            OnStackDataFetched?.Invoke(stackModels);
        }
    }
}
