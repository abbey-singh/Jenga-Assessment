using Models;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;

namespace Signals
{
    public class BlockClickedSignal
    {
        public BlockModel BlockModel { get; private set; }
        public Transform BlockTransform { get; private set; }
        public Transform JengaStackTransform { get; private set; }
        public InputButton InputButton { get; private set; }

        public BlockClickedSignal(BlockModel blockModel, Transform blockTransform, Transform jengaStackTransform, InputButton inputButton)
        {
            BlockModel = blockModel;
            BlockTransform = blockTransform;
            JengaStackTransform = jengaStackTransform;
            InputButton = inputButton;
        }
    }
}
