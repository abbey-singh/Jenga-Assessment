using Constants;
using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class StackModel
    {
        public GameObject StackObject;
        public List<BlockController> Blocks;
        public StackGameModes SelectedMode;
    }
}