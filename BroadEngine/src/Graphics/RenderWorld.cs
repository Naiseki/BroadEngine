using System;
using System.Collections.Generic;

namespace Broad.Graphics
{
    class RenderWorld: IWorld
    {
        List<RendererBase>[] layers;
        bool isDestroyed = false;

        public bool IsDestroyed => isDestroyed;

        
        internal RenderWorld(int layerCount)
        {
            InitLayers(layerCount);
        }


        internal void InitLayers(int layerCount)
        {
            layers = new List<RendererBase>[layerCount];
            for (int i = 0; i < layerCount; i++) {
                layers[i] = new List<RendererBase>();
            }
        }


        public void AddRenderer(RendererBase renderer, int layer)
        {
            if (0 <= layer && layer < layers.Length) {
                layers[layer].Add(renderer);
            }
            else {
                throw new IndexOutOfRangeException("Invalid index");
            }
        }


        public void RemoveRenderer(RendererBase renderer)
        {
            layers[renderer.GraphicLayer].Remove(renderer);
        }


        public void Step()
        {
            ExecuteOnRender();
            Camera.Update();
            ComputeAabb();
            Render();
        }


        void Render()
        {
            for (int i = 0; i < layers.Length; i++) {
                var renderers = layers[i];
                for (int j = 0; j < renderers.Count; j++) {
                    var ren = renderers[j];
                    if (ren.CanRender) {
                        ren.Render();
                    }
                }
            }
        }


        void ComputeAabb()
        {
            for (int i = 0; i < layers.Length; i++) {
                var renderers = layers[i];
                for (int j = 0; j < renderers.Count; j++) {
                    var ren = renderers[j];
                    ren.ComputeAabb();
                }
            }
        }


        void ExecuteOnRender()
        {
            for (int i = 0; i < layers.Length; i++) {
                var renderers = layers[i];
                for (int j = 0; j < renderers.Count; j++) {
                    var ren = renderers[j];
                    ren.Actor.OnRender();
                }
            }
        }

        
        public void Destroy()
        {
            isDestroyed = true;
            layers = new List<RendererBase>[0];
        }


        internal void ChangeLayer(RendererBase renderer, int layer)
        {
            RemoveRenderer(renderer);
            AddRenderer(renderer, layer);
        }
    }
}