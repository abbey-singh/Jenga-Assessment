using Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Controllers
{
    public class InputController : MonoBehaviour, IDragHandler, IPointerClickHandler
    {
        public event Action<PointerEventData> OnMouseDrag;

        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnMouseDrag?.Invoke(eventData);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform != null)
                {
                    var hitObject = raycastHit.transform.gameObject;

                    if (hitObject.tag == "Block")
                    {
                        var blockController = hitObject.GetComponent<BlockController>();
                        var blockModel = blockController.BlockModel;
                        var towerTransform = blockController.JengaStackTransform;

                        var signal = new BlockClickedSignal(blockModel, hitObject.transform, towerTransform, eventData.button);
                        _signalBus.Fire(signal);
                    }
                }
            }
        }
    }
}
