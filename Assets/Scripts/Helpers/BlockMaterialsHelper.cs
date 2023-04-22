using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Helpers
{
    public class BlockMaterialsHelper : IInitializable
    {
        public Material GlassMaterial;
        public Material WoodMaterial;
        public Material StoneMaterial;

        private const string MATERIALS_PATH = "Materials/";

        public void Initialize()
        {
            GlassMaterial = Resources.Load<Material>(MATERIALS_PATH + "Glass");
            WoodMaterial = Resources.Load<Material>(MATERIALS_PATH + "Wood");
            StoneMaterial = Resources.Load<Material>(MATERIALS_PATH + "Stone");
        }
    }
}