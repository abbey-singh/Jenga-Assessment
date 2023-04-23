using Helpers;
using Models;
using Models.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class BlockController : MonoBehaviour
    {
        public BlockModel BlockModel { get { return _blockModel; } }
        public Transform JengaTowerTransform { get { return transform.parent.parent; } }

        [SerializeField] MeshRenderer _blockRenderer;

        private BlockModel _blockModel;

        private BlockMaterialsHelper _blockMaterialsHelper;

        [Inject]
        public void Init(BlockMaterialsHelper blockMaterialsHelper)
        {
            _blockMaterialsHelper = blockMaterialsHelper;
        }

        public void SetBlockModel(BlockModel model)
        {
            _blockModel = model;

            UpdateBlockMaterial();
        }

        private void UpdateBlockMaterial()
        {
            Material material;
            switch (_blockModel.Mastery)
            {
                case BlockTypes.Glass:
                    material = _blockMaterialsHelper.GlassMaterial;
                    break;
                case BlockTypes.Wood:
                    material = _blockMaterialsHelper.WoodMaterial;
                    break;
                case BlockTypes.Stone:
                    material = _blockMaterialsHelper.StoneMaterial;
                    break;
                default:
                    throw new NotImplementedException($"BlockTypes '{_blockModel.Mastery}' not implemented");
            }

            _blockRenderer.material = material;
        }

        public class Factory : PlaceholderFactory<BlockController>
        {
        }
    }
}