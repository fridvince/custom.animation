using UnityEngine;
using UnityEngine.UI;

namespace fridvince.Game.Common
{
    [RequireComponent(typeof(AutoHeader))]
    [RequireComponent(typeof(CanvasRenderer))]
    public class BaseRaycastTarget : Graphic 
    {
        public override void SetMaterialDirty() {}
        public override void SetVerticesDirty() {}
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}