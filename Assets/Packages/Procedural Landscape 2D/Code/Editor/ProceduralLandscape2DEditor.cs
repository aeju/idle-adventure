#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Packages.Procedural_Landscape_2D.Code.Editor
{
    [CustomEditor(typeof(ProceduralLandscape2D))]
    public class ProceduralLandscape2DEditor : UnityEditor.Editor
    {
        private SpriteShapeController _spriteShapeInstanceCache;
        private float _generationSizeCache = 1;
        private GenerationLayerClass[] _generationLayersCache;
        private int _chunksPreloadCache = 3;
        private AdvancedSettingsClass _advancedSettingsCache;
        private int[] _skipChunksCache;

        private ProceduralLandscape2D _proceduralLandscape;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            _proceduralLandscape = (ProceduralLandscape2D) target;
            
            if (!_proceduralLandscape.updateRuntimeInEditor)
            {
                if (GUILayout.Button("Generate"))
                {
                    UpdateCache(_proceduralLandscape);
                    FixRegion(_proceduralLandscape);
                    RunGenerator();
                }
            }
            else
            {
                if (DataWasChanged(_proceduralLandscape))
                {
                    UpdateCache(_proceduralLandscape);
                    FixRegion(_proceduralLandscape);
                
                    RunGenerator();
                }
            }

            if (GUILayout.Button("Add dynamic loader"))
            {
                var loader = _proceduralLandscape.gameObject.AddComponent<DynamicLoader.DynamicLoader>();
                if (Camera.main != null) loader.SetPlayer(Camera.main.transform);
            }
        }

        private async void RunGenerator()
        {
            await _proceduralLandscape.Generate();
        }

        private void UpdateCache(ProceduralLandscape2D pl)
        {
            _spriteShapeInstanceCache = pl.SpriteShapeInstance;
            
            _generationSizeCache = pl.GenerationSize;

            _generationLayersCache = new GenerationLayerClass[pl.GenerationLayers.Length];
            for (int i = 0; i < pl.GenerationLayers.Length; i++)
            {
                _generationLayersCache[i] = new GenerationLayerClass();
                pl.GenerationLayers[i].CopyTo(_generationLayersCache[i]);
            }


            _chunksPreloadCache = pl.ChunksPreload;

            _advancedSettingsCache = new AdvancedSettingsClass();
            pl.AdvancedSettings.CopyTo(_advancedSettingsCache);

            _skipChunksCache = pl.SkipChunks;

        }

        private bool DataWasChanged(ProceduralLandscape2D pl)
        {
            if (_spriteShapeInstanceCache != pl.SpriteShapeInstance)
                return true;
            if (_generationSizeCache != pl.GenerationSize)
                return true;
            if (_generationLayersCache == null || _generationLayersCache.Length != pl.GenerationLayers.Length)
                return true;

            for (int i = 0; i < _generationLayersCache.Length; i++)
            {
                if (!_generationLayersCache[i].Equals(pl.GenerationLayers[i]))
                    return true;
            }

            if (_chunksPreloadCache != pl.ChunksPreload)
                return true;
            if (!pl.AdvancedSettings.Equals(_advancedSettingsCache))
                return true;
            if (_skipChunksCache.Length != pl.SkipChunks.Length)
                return true;
            for (int i = 0; i < _skipChunksCache.Length; i++)
            {
                if (_skipChunksCache[i] != pl.SkipChunks[i])
                    return true;
            }

            return false;
        }


        private static void FixRegion(ProceduralLandscape2D proceduralLandscape)
        {
            var layers = proceduralLandscape.GenerationLayers;
            foreach (var layer in layers)
            {
                layer.regionSetting.regionSize = Mathf.Clamp(layer.regionSetting.regionSize, 1, Int32.MaxValue);
            }
        }
    }
}
#endif