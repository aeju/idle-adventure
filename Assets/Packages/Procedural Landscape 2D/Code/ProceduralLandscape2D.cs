using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace Packages.Procedural_Landscape_2D.Code
{
    public class ProceduralLandscape2D : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController spriteShapeInstance;
        public SpriteShapeController SpriteShapeInstance => spriteShapeInstance;

        [Space] [Header("General Settings")] [Range(0.1f, 10f)] [SerializeField]
        private float generationSize = 1;

        public float GenerationSize => generationSize;

        public GenerationLayerClass[] GenerationLayers
        {
            get => generationLayers;
            private set => generationLayers = value;
        }

        [SerializeField] private GenerationLayerClass[] generationLayers = new[]
        {
            new GenerationLayerClass()
            {
                regionSetting = new GenerationLayerClass.RegionSettingClass(),
                isActive = true,
                generationScale = 1000,
                verticalPower = 2,
                title = "Mountain"
            },
            new GenerationLayerClass()
            {
                regionSetting = new GenerationLayerClass.RegionSettingClass(),
                isActive = true,
                generationScale = 300,
                verticalPower = 2,
                title = "Details"
            }
        };

        [Space] [Header("Additional Settings")]
        public bool updateRuntimeInEditor = true;

        [SerializeField] internal bool generateOnStart = true;
        [SerializeField] internal int chunksPreload = 3;
        public int ChunksPreload => chunksPreload;
        [SerializeField] private bool useRandomSeed = false;
        [Space] [SerializeField] private AdvancedSettingsClass advancedSettings;
        public AdvancedSettingsClass AdvancedSettings => advancedSettings;

        [Space] [Header("Skip chunks to replace them manually")] [SerializeField]
        private int[] skipChunks;

        public int[] SkipChunks => skipChunks;
        private List<SpriteShapeController> _generatedObjects;

        public float ChunkSize
        {
            get
            {
                float chunkSize = 0f;

                var pointsPerChunk =
                    (((advancedSettings.pointsCountPerChunk)) -
                     (advancedSettings.chunkOverlapDistance));

                var totalSize = pointsPerChunk * (advancedSettings.distanceBetweenPoints) * generationSize;
                chunkSize = totalSize;

                return chunkSize;
            }
        }

        public void SetSpriteShapeInstance(SpriteShapeController spriteShape)
        {
            if (spriteShape is null)
                return;

            spriteShapeInstance = spriteShape;
        }

        private async void Start()
        {
            if (useRandomSeed)
            {
                advancedSettings.seed = Random.Range(0f, 1024f);
            }

            if (generateOnStart)
                await Generate(-1, true);
        }

        public async Task<SpriteShapeController> Generate(int chunkId = -1, bool forceSync = false)
        {
            if (spriteShapeInstance == null)
                return null;

            if (chunkId == -1)
                Clear();

            ClearFromEmpty();

            if (useRandomSeed && !(Application.isEditor && updateRuntimeInEditor))
            {
                advancedSettings.seed = Random.Range(0f, 1024f);
            }

            advancedSettings.pointsCountPerChunk = Mathf.Clamp(advancedSettings.pointsCountPerChunk, 1, Int32.MaxValue);

            SpriteShapeController resultShape = null;
            if (chunkId == -1)
            {
                for (int i = 0; i < chunksPreload; i++)
                {
                    resultShape = await ProceedHeightAndDraw(i);
                }
            }
            else
                resultShape = await ProceedHeightAndDraw(chunkId);


            GenerateBottom();
            SmoothResult();
            SetStartPosition();

            Bake();

            return resultShape;
        }

        private async Task<SpriteShapeController> ProceedHeightAndDraw(int chunkId, bool forceSync = false)
        {
            if (skipChunks != null && skipChunks.Contains(chunkId))
                return null;

            float[] heights = Array.Empty<float>();

            switch (advancedSettings.syncMode)
            {
                case AdvancedSettingsClass.SyncMode.SyncOnSceneAndAsyncInGame:
                    if (!Application.isPlaying || forceSync)
                        ProceedHeightsSync();
                    else
                        await ProceedHeightsAsync();
                    break;

                case AdvancedSettingsClass.SyncMode.Sync:
                    ProceedHeightsSync();
                    break;

                case AdvancedSettingsClass.SyncMode.Async:
                    await ProceedHeightsAsync();
                    break;
            }

            void ProceedHeightsSync()
            {
                heights = GenerateHeights(chunkId);
            }

            async Task ProceedHeightsAsync()
            {
                await Task.Run(() => { heights = GenerateHeights(chunkId); });
            }

            return DrawResult(heights, chunkId);
        }

        private float[] GenerateHeights(int terrainIndex)
        {
            var heights = new float[(int) ((advancedSettings.pointsCountPerChunk))];

            for (int i = 0; i < heights.Length; i++)
            {
                var x = i + (terrainIndex * ((int) ((advancedSettings.pointsCountPerChunk)) - advancedSettings.chunkOverlapDistance));
                for (var layerIndex = 0; layerIndex < generationLayers.Length; layerIndex++)
                {
                    var setting = generationLayers[layerIndex];
                    if (!setting.isActive)
                        continue;

                    x += setting.offset;

                    var verticalPower = setting.verticalPower * generationSize;
                    var finalPower = verticalPower * (setting.generationScale / 100f);
                    
                    var perlinValueX = advancedSettings.seed * (layerIndex + 1) * 693.5347f;
                    var perlinValueY = x / (setting.generationScale / 100f);

                    var pointNum = x + setting.regionSetting.offset;
                    ProceedRegion(ref finalPower, pointNum, setting.regionSetting);
                    
                    heights[i] += Mathf.PerlinNoise(perlinValueX, perlinValueY) * finalPower;
                    //heights[i] = finalPower;
                }
            }

            return heights;
        }

        private void ProceedRegion(ref float finalPower, int pointIndex, GenerationLayerClass.RegionSettingClass regionSetting)
        {
            finalPower *= GetRegionValue(pointIndex, regionSetting.regionSize, regionSetting.interregionalDistance, regionSetting.smoothingSize, regionSetting.smoothingCurve);
        }
        
        public float GetRegionValue(int pointNum, int regionSize, int distanceBetweenRegions, int smoothingPoints, AnimationCurve smoothingCurve)
        {
            // Вычисляем номер региона в котором находится точка
            int regionNum = (int)Mathf.Floor((float)pointNum / (regionSize + distanceBetweenRegions));

            // Вычисляем начало и конец текущего региона
            float currentRegionStart = regionNum * (regionSize + distanceBetweenRegions);
            float currentRegionEnd = currentRegionStart + regionSize;
            
            // Вычисляем начало и конец следующего региона
            float nextRegionStart = (regionNum + 1) * (regionSize + distanceBetweenRegions);
            float nextRegionEnd = nextRegionStart + regionSize;

            // Если точка находится внутри региона, возвращаем 1
            if (pointNum >= currentRegionStart && pointNum < currentRegionEnd)
            {
                return 1;
            }
            
            // Если точка находится справа от региона
            if (pointNum >= currentRegionEnd && pointNum < currentRegionEnd + smoothingPoints)
            {
                float x = pointNum - currentRegionEnd;
                float y = smoothingPoints;
                float t = x / y;
                return smoothingCurve.Evaluate(1 - t);
            }
            
            // Если точка находится слева от региона
            if (pointNum < nextRegionStart && pointNum >= nextRegionStart - smoothingPoints)
            {
                float x = nextRegionStart - pointNum;
                float y = smoothingPoints;
                float t = x / y;
                return smoothingCurve.Evaluate(1 - t);
            }

            return 0;
        }
        
        private SpriteShapeController DrawResult(float[] heights, int terrainIndex)
        {
            var offset =
                new Vector3(
                    terrainIndex * generationSize *
                    (((advancedSettings.pointsCountPerChunk)) - advancedSettings.chunkOverlapDistance) *
                    ((advancedSettings.distanceBetweenPoints)), 0, 0);
            var ground = Instantiate(spriteShapeInstance, offset + transform.position, Quaternion.identity, transform);


            for (int i = 0; i < ground.spline.GetPointCount(); i++)
                ground.spline.RemovePointAt(i);

            if (advancedSettings.orderInLayerType == AdvancedSettingsClass.PrefabParameterTypeEnum.Override)
                ground.spriteShapeRenderer.sortingOrder = advancedSettings.orderInLayer;

            if (advancedSettings.colorModeType == AdvancedSettingsClass.PrefabParameterTypeEnum.Override)
                ground.spriteShapeRenderer.color = advancedSettings.color;

            _generatedObjects.Add(ground);

            for (int i = 2; i < heights.Length; i++)
            {
                try
                {
                    var pointPosition = new Vector3(i * (advancedSettings.distanceBetweenPoints) * generationSize,
                        heights[i],
                        0);
                    ground.spline.InsertPointAt(i, pointPosition);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            // ground.spline.RemovePointAt(1);
            // ground.spline.RemovePointAt(0);

            return ground;
        }

        private void SmoothResult()
        {
            foreach (var ground in _generatedObjects)
            {
                for (int i = 4; i < ground.spline.GetPointCount(); i++)
                {
                    if (i != 0 && i != ground.spline.GetPointCount() - 1)
                    {
                        Vector3 leftPosition = new Vector3();
                        Vector3 rightPosition = new Vector3();

                        var leftPoint = ground.spline.GetPosition(i - 1);
                        var rightPoint = ground.spline.GetPosition(i + 1);

                        var vector = (rightPoint - leftPoint).normalized;

                        float smoothTangentDistance = 0;
                        if (advancedSettings.smoothType == AdvancedSettingsClass.SmoothTypeEnum.Smooth)
                            smoothTangentDistance = (advancedSettings.distanceBetweenPoints) * generationSize / 2f;

                        rightPosition += vector * smoothTangentDistance;
                        leftPosition -= vector * smoothTangentDistance;

                        ground.spline.SetTangentMode(i, ShapeTangentMode.Continuous);

                        ground.spline.SetLeftTangent(i, leftPosition);
                        ground.spline.SetRightTangent(i, rightPosition);
                    }
                }
            }
        }

        private void GenerateBottom()
        {
            var verticalPosition = 0f;

            if (_generatedObjects != null)
                foreach (var ground in _generatedObjects)
                {
                    if (advancedSettings.drawBottom == AdvancedSettingsClass.DrawBottomEnum.Draw)
                    {
                        var p1 = ground.spline.GetPosition(2);
                        p1.y = verticalPosition;
                        var p2 = ground.spline.GetPosition(ground.spline.GetPointCount() - 1);
                        p2.y = verticalPosition;
                        
                        ground.spline.SetPosition(1, p1 + (Vector3.down * advancedSettings.bottomOffset));
                        ground.spline.SetPosition(0, p2 + (Vector3.down * advancedSettings.bottomOffset));

                        ground.spline.isOpenEnded = false;
                    }
                    else
                    {
                        ground.spline.isOpenEnded = true;
                        ground.spline.RemovePointAt(0);
                        ground.spline.RemovePointAt(0);
                    }
                }
        }

        private void SetStartPosition()
        {
            if (_generatedObjects.Count == 0)
                return;

            switch (advancedSettings.positionType)
            {
                case AdvancedSettingsClass.PositionTypeEnum.Local:

                    break;
                case AdvancedSettingsClass.PositionTypeEnum.OnSpawnPointWithOffset:
                    var chunkPointsCount = _generatedObjects[0].spline.GetPointCount();
                    var point = _generatedObjects[0].spline.GetPosition(Mathf.FloorToInt(chunkPointsCount / 2f)) +
                                Vector3.zero;

                    transform.position = -(Vector3) point + advancedSettings.spawnPointOffset;
                    break;
            }


        }

        private void Bake()
        {
            foreach (var ground in _generatedObjects)
            {
                if (ground == null)
                    continue;

                ground.BakeMesh();
                ground.BakeCollider();

                UnityEngine.Rendering.CommandBuffer rc = new UnityEngine.Rendering.CommandBuffer();
                rc.GetTemporaryRT(0, 256, 256, 0);
                rc.SetRenderTarget(0);
                rc.DrawRenderer(ground.spriteShapeRenderer, ground.spriteShapeRenderer.sharedMaterial);
                rc.ReleaseTemporaryRT(0);
                Graphics.ExecuteCommandBuffer(rc);
            }
        }




        private void Clear()
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            if (_generatedObjects != null)
                _generatedObjects.Clear();

            _generatedObjects = new List<SpriteShapeController>();
        }

        private void ClearFromEmpty()
        {
            if (_generatedObjects == null)
                _generatedObjects = new List<SpriteShapeController>();

            for (int i = 0; i < _generatedObjects.Count; i++)
            {
                if (_generatedObjects[i] == null)
                    _generatedObjects.RemoveAt(i);
            }
        }
    }

    [Serializable]
    public class AdvancedSettingsClass
    {
        public SmoothTypeEnum smoothType;
        [Space] public PositionTypeEnum positionType;
        public Vector3 spawnPointOffset;
        [Space] public PrefabParameterTypeEnum orderInLayerType;
        public int orderInLayer;
        [Space] public PrefabParameterTypeEnum colorModeType;
        public Color color;
        [Space] public DrawBottomEnum drawBottom;
        public float bottomOffset = 100;
        [Space] public float seed = 256;
        public int chunkOverlapDistance = 5;
        public float distanceBetweenPoints = 2.5f;
        public int pointsCountPerChunk = 20;
        [Space] public SyncMode syncMode;

        public enum DrawBottomEnum
        {
            Draw,
            None
        }

        public enum SmoothTypeEnum
        {
            Smooth,
            None
        }

        public enum SyncMode
        {
            Sync,
            SyncOnSceneAndAsyncInGame,
            Async
        }

        public enum PositionTypeEnum
        {
            Local,
            OnSpawnPointWithOffset
        }

        public enum PrefabParameterTypeEnum
        {
            FromPrefabInstance,
            Override
        }

        public void CopyTo(AdvancedSettingsClass destination)
        {
            destination.smoothType = smoothType;
            destination.positionType = positionType;
            destination.spawnPointOffset = spawnPointOffset;
            destination.orderInLayerType = orderInLayerType;
            destination.orderInLayer = orderInLayer;
            destination.colorModeType = colorModeType;
            destination.color = color;
            destination.drawBottom = drawBottom;
            destination.bottomOffset = bottomOffset;
            destination.seed = seed;
            destination.chunkOverlapDistance = chunkOverlapDistance;
            destination.distanceBetweenPoints = distanceBetweenPoints;
            destination.pointsCountPerChunk = pointsCountPerChunk;
            destination.syncMode = syncMode;
        }

        public bool Equals(AdvancedSettingsClass compareObject)
        {
            if (compareObject == null)
            {
                return false;
            }

            if (smoothType != compareObject.smoothType)
            {
                return false;
            }

            if (positionType != compareObject.positionType)
            {
                return false;
            }

            if (spawnPointOffset != compareObject.spawnPointOffset)
            {
                return false;
            }

            if (orderInLayerType != compareObject.orderInLayerType)
            {
                return false;
            }

            if (orderInLayer != compareObject.orderInLayer)
            {
                return false;
            }

            if (colorModeType != compareObject.colorModeType)
            {
                return false;
            }

            if (!color.Equals(compareObject.color))
            {
                return false;
            }

            if (drawBottom != compareObject.drawBottom)
            {
                return false;
            }

            if (Math.Abs(bottomOffset - compareObject.bottomOffset) > 0.01f)
            {
                return false;
            }

            if (Math.Abs(seed - compareObject.seed) > 0.01f)
            {
                return false;
            }

            if (chunkOverlapDistance != compareObject.chunkOverlapDistance)
            {
                return false;
            }

            if (Math.Abs(distanceBetweenPoints - compareObject.distanceBetweenPoints) > 0.01f)
            {
                return false;
            }

            if (pointsCountPerChunk != compareObject.pointsCountPerChunk)
            {
                return false;
            }

            if (syncMode != compareObject.syncMode)
            {
                return false;
            }

            return true;
        }
    }

    [Serializable]
    public class GenerationLayerClass
    {

        [System.Serializable]
        public class RegionSettingClass
        {
            public int offset;

            public int regionSize = 1;
            public int interregionalDistance;
            public int smoothingSize;
            public AnimationCurve smoothingCurve;
        }

        //Just for more comfortable usage throw inspector
        public string title = "Mountains";
        public bool isActive = true;
        public int offset;
        public float generationScale = 100;
        public float verticalPower = 1.5f;
        public RegionSettingClass regionSetting;

        public void CopyTo(GenerationLayerClass destination)
        {
            if(destination == null)
                return;
            
            destination.title = title;
            destination.isActive = isActive;
            destination.offset = offset;
            destination.generationScale = generationScale;
            destination.verticalPower = verticalPower;

            destination.regionSetting = new RegionSettingClass();
            destination.regionSetting.offset = regionSetting.offset;
            destination.regionSetting.regionSize = regionSetting.regionSize;
            destination.regionSetting.interregionalDistance = regionSetting.interregionalDistance;
            destination.regionSetting.smoothingSize = regionSetting.smoothingSize;
            destination.regionSetting.smoothingCurve = regionSetting.smoothingCurve;
        }

        public bool Equals(GenerationLayerClass compareObject)
        {
            if (compareObject == null)
            {
                return false;
            }

            if (title != compareObject.title)
            {
                return false;
            }

            if (isActive != compareObject.isActive)
            {
                return false;
            }

            if (offset != compareObject.offset)
                return false;
            
            if (Math.Abs(generationScale - compareObject.generationScale) > 0.001f)
            {
                return false;
            }

            if (Math.Abs(verticalPower - compareObject.verticalPower) > 0.001f)
            {
                return false;
            }

            if (regionSetting.offset != compareObject.regionSetting.offset)
            {
                return false;
            }
            
            if (regionSetting.regionSize != compareObject.regionSetting.regionSize)
            {
                return false;
            }
            
            if (regionSetting.interregionalDistance != compareObject.regionSetting.interregionalDistance)
            {
                return false;
            }
            
            if (regionSetting.smoothingSize != compareObject.regionSetting.smoothingSize)
            {
                return false;
            }
            
            if (regionSetting.smoothingCurve == null || compareObject.regionSetting.smoothingCurve == null)
                return false;
            
            if (!regionSetting.smoothingCurve.Equals(compareObject.regionSetting.smoothingCurve))
            {
                return false;
            }

            return true;
        }

    }

}