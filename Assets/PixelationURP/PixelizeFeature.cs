using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

public class PixelizeFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public float pixelatingSpeed = 0.25f;
        public int screenHeight = 144;
        public int startingRange = 148;
        public int rangeAmount = 6;
        public int[] screenHeightRange;
        public void UpdateRange() {
            screenHeightRange = Enumerable.Range(startingRange, rangeAmount).ToArray();
        }
    }

    [SerializeField] private CustomPassSettings settings;
    private PixelizePass customPass;

    public override void Create()
    {
        customPass = new PixelizePass(settings);
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(customPass);
    }
}


