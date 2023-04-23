using Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using static UnityEngine.EventSystems.PointerEventData;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform _anchorTransform;
        [SerializeField] InputController _inputController;

        private const float SENSITIVITY = 0.15f;
        private const float TRANSITION_DURATION = 0.5f;

        private SignalBus _signalBus;

        private Coroutine _currentTransition;

        private void Start()
        {
            _inputController.OnMouseDrag += RotateCamera;
        }

        private void OnDestroy()
        {
            _inputController.OnMouseDrag -= RotateCamera;
        }

        [Inject]
        public void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;

            _signalBus.Subscribe<BlockClickedSignal>(b => UpdateCameraFocusPoint(b.JengaStackTransform, b.InputButton));
        }

        private void UpdateCameraFocusPoint(Transform target, InputButton inputButton)
        {
            bool isLeftClick = inputButton == InputButton.Left;
            bool isAnchorAtTarget = target.position == _anchorTransform.position;

            if (!isLeftClick || isAnchorAtTarget)
            {
                return;
            }

            if (_currentTransition != null)
            {
                StopCoroutine(_currentTransition);
            }

            _currentTransition = StartCoroutine(TransitionToFocusPoint(target));
        }

        private IEnumerator TransitionToFocusPoint(Transform target)
        {
            Vector3 startPos = _anchorTransform.position;
            float currTime = 0f;

            while (currTime <= TRANSITION_DURATION)
            {
                float t = currTime / TRANSITION_DURATION;
                t = t * t * (3f - 2f * t); // Creates a smooth step
                _anchorTransform.position = Vector3.Lerp(startPos, target.position, t);
                currTime += Time.deltaTime;

                yield return null;
            }

            _anchorTransform.position = target.position;
            _currentTransition = null;
        }

        private void RotateCamera(PointerEventData eventData)
        {
            transform.RotateAround(_anchorTransform.position, Vector3.up, eventData.delta.x * SENSITIVITY);
        }
    }
}