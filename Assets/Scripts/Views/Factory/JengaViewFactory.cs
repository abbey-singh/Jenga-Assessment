using Controllers;
using DataAccessors;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Views.Jenga.Factory
{
    public class JengaViewFactory: IInitializable
    {
        private List<float> _rowXPositions = new List<float>() 
        { 
            -0.025f - OFFSET_BETWEEN_BLOCKS, 
            0, 
            0.025f + OFFSET_BETWEEN_BLOCKS
        };

        private float _blockHeight;
        private int _numJengas = 0;

        private const float ROW_HEIGHT_BASE_OFFSET = 0.0075f;
        private const float OFFSET_BETWEEN_BLOCKS = 0.0005f;
        private const float OFFSET_BETWEEN_JENGAS = 0.3f;

        private GameObject _jengaBlockPrefab;
        private GameObject _jengaGradeLabelPrefab;

        private DiContainer Container;

        public JengaViewFactory(DiContainer container)
        {
            Container = container;
        }

        public void Initialize()
        {
            _jengaBlockPrefab = Resources.Load<GameObject>("Prefabs/Block");
            _jengaGradeLabelPrefab = Resources.Load<GameObject>("Prefabs/GradeLabel");
            _blockHeight = _jengaBlockPrefab.transform.localScale.y;
        }

        public StackModel CreateJenga(List<BlockModel> blockModels, ref GameObject parent)
        {
            Assert.IsTrue(blockModels != null && blockModels.Count != 0, "Cannot create empty Jenga stack");

            StackModel stackModel = new StackModel();
            stackModel.Blocks = new List<BlockController>();

            GameObject jengaStack = new GameObject("Jenga Stack");
            jengaStack.transform.parent = parent.transform;
            jengaStack.transform.localPosition = new Vector3(OFFSET_BETWEEN_JENGAS * _numJengas, 0, 0);
            jengaStack.transform.localRotation = Quaternion.identity;
            _numJengas++;

            stackModel.StackObject = jengaStack;

            CreateTower(blockModels, ref jengaStack, ref stackModel);

            GameObject label = GameObject.Instantiate(_jengaGradeLabelPrefab, jengaStack.transform);
            label.GetComponent<TextMeshPro>().text = blockModels[0].Grade;

            return stackModel;
        }

        private GameObject CreateTower(List<BlockModel> blockModels, ref GameObject parent, ref StackModel stackModel)
        {
            GameObject tower = new GameObject("Tower");
            tower.transform.parent = parent.transform;
            tower.transform.localPosition = Vector3.zero;
            tower.transform.localRotation = Quaternion.identity;

            List<BlockModel> sortedBlockModels = SortJengaModels(blockModels);

            // Create tower rows
            for (int i = 0; i < sortedBlockModels.Count; i += 3)
            {
                List<BlockModel> rowModels = new List<BlockModel>();
                rowModels.Add(sortedBlockModels[i]);

                if (i + 1 < sortedBlockModels.Count)
                {
                    rowModels.Add(sortedBlockModels[i + 1]);
                }

                if (i + 2 < sortedBlockModels.Count)
                {
                    rowModels.Add(sortedBlockModels[i + 2]);
                }

                GameObject rowObject = CreateTowerRow(rowModels, ref tower, ref stackModel);

                float rowHeight = ROW_HEIGHT_BASE_OFFSET + (i / 3) * (_blockHeight + OFFSET_BETWEEN_BLOCKS);
                rowObject.transform.localPosition = new Vector3(0, rowHeight, 0);

                // Alternating rows should be perpendicular to one another
                if (i % 2 == 0)
                {
                    rowObject.transform.localEulerAngles = new Vector3(0, 90f, 0);
                }
            }

            return tower;
        }

        private GameObject CreateTowerRow(List<BlockModel> row, ref GameObject parent, ref StackModel stackModel)
        {
            Assert.IsTrue(row.Count > 0 && row.Count <= 3, "Row needs between 1 and 3 blocks");

            GameObject towerRow = new GameObject("Tower Row");
            towerRow.transform.parent = parent.transform;
            towerRow.transform.localPosition = Vector3.zero;
            towerRow.transform.localRotation = Quaternion.identity;

            for (int i = 0; i < row.Count; i++)
            {
                var block = CreateBlock(row[i], ref towerRow, ref stackModel);
                block.transform.localPosition = new Vector3(_rowXPositions[i], 0, 0);
                block.GetComponent<BlockController>().SaveDefaultTransform();
            }

            return towerRow;
        }

        private GameObject CreateBlock(BlockModel blockModel, ref GameObject parent, ref StackModel stackModel)
        {
            BlockController blockController = Container.InstantiatePrefabForComponent<BlockController>(_jengaBlockPrefab, parent.transform);
            blockController.SetBlockModel(blockModel);
            blockController.SetJengaStack(stackModel.StackObject);

            stackModel.Blocks.Add(blockController);

            return blockController.gameObject;
        }

        private List<BlockModel> SortJengaModels(List<BlockModel> blockModels)
        {
            return blockModels.OrderBy(b => b.Domain)
                .ThenBy(b => b.Cluster)
                .ThenBy(b => b.StandardId)
                .ToList();
        } 
    }
}
