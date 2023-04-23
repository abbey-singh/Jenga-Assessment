using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Views.Jenga
{
    public class BlockPanelView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _titleText;
        [SerializeField] TextMeshProUGUI _clusterText;
        [SerializeField] TextMeshProUGUI _descriptionText;

        public void SetTitleText(string title)
        {
            _titleText.text = title;
        }

        public void SetClusterText(string cluster)
        {
            _clusterText.text = cluster;
        }

        public void SetDescriptionText(string description)
        {
            _descriptionText.text = description;
        }
    }
}
