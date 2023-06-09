using Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

namespace DataAccessors
{
    public class NetworkingOperations
    {
        public event Action<List<BlockModel>> OnStacksDataFetched;

        private UrlHolder _urlHolder;
        private UnityWebRequest _request;

        public NetworkingOperations(UrlHolder urlHolder)
        {
            _urlHolder = urlHolder;
        }

        public void StartFetchingStacksData()
        {
            _request = UnityWebRequest.Get(_urlHolder.GetURL());
            var operation = _request.SendWebRequest();
            operation.completed += FetchStacksDataCompleted;
        }

        private void FetchStacksDataCompleted(AsyncOperation obj)
        {
            if(_request.result == UnityWebRequest.Result.ProtocolError || _request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"Error receiving UnityWebRequest '{_request.error}'");
                return;
            }

            string data = _request.downloadHandler.text;
            List<BlockModel> blockModels = JsonConvert.DeserializeObject<List<BlockModel>>(data);
            
            _request = null;

            OnStacksDataFetched?.Invoke(blockModels);
        }
    }
}
