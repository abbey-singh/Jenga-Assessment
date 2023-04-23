using Models;
using Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views.Jenga;
using Zenject;
using static UnityEngine.EventSystems.PointerEventData;

namespace Presenters
{
    public class BlockPanelPresenter: MonoBehaviour
    {
        [SerializeField] BlockPanelView _blockPanelView;

        private SignalBus _signalBus;

        private BlockModel _cachedModel;

        [Inject]
        public void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<BlockClickedSignal>(b => DisplayBlockInformationPanel(b.BlockModel, b.InputButton));
        }

        private void DisplayBlockInformationPanel(BlockModel blockModel, InputButton inputButton)
        {
            bool isRightClick = inputButton == InputButton.Right;
            bool isSameModel = blockModel == _cachedModel;
            
            if (!isRightClick || isSameModel)
            {
                return;
            }

            PopulateView(blockModel);
        }

        private void PopulateView(BlockModel blockModel)
        {
            _blockPanelView.SetTitleText($"{blockModel.Grade}: {blockModel.Domain}");
            _blockPanelView.SetClusterText(blockModel.Cluster);
            _blockPanelView.SetDescriptionText($"{blockModel.StandardId}: {blockModel.StandardDescription}");

            gameObject.SetActive(true);

            _cachedModel = blockModel;
        }

        private void OnDisable()
        {
            _cachedModel = null;
        }
    }
}
