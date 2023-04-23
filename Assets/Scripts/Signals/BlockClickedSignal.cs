using Models;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;

namespace Signals
{
    public class BlockClickedSignal
    {
        public BlockModel BlockModel { get; private set; }
        public Transform BlockTransform { get; private set; }
        public Transform JengaTowerTransform { get; private set; }
        public InputButton InputButton { get; private set; }

        public BlockClickedSignal(BlockModel blockModel, Transform blockTransform, Transform jengaTowerTransform, InputButton inputButton)
        {
            BlockModel = blockModel;
            BlockTransform = blockTransform;
            JengaTowerTransform = jengaTowerTransform;
            InputButton = inputButton;
        }
    }
}
