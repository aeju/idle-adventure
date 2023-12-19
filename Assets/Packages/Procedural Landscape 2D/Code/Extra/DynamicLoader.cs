using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.DynamicLoader
{
    [RequireComponent(typeof(ProceduralLandscape2D))]
    public class DynamicLoader : MonoBehaviour
    {
        private ProceduralLandscape2D _proceduralLandscape2D;
        [SerializeField] private Transform player;
        [SerializeField] private int loadedChunksAroundPlayer = 4;

        private readonly List<ChunkData> _loadedChunks = new List<ChunkData>();

        private void Awake()
        {
            if (_proceduralLandscape2D == null)
                _proceduralLandscape2D = GetComponent<ProceduralLandscape2D>();

            _proceduralLandscape2D.generateOnStart = false;
            _proceduralLandscape2D.chunksPreload = 0;
        }

        private void Start()
        {
            DestroyChildren(_proceduralLandscape2D.transform);
        }

        public void SetPlayer(Transform t)
        {
            player = t;
        }
        
        private void DestroyChildren(Transform target)
        {
            int childCount = target.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject child = target.GetChild(i).gameObject;
                GameObject.Destroy(child);
            }
        }

        private void Update()
        {
            if(player == null || _proceduralLandscape2D == null || loadedChunksAroundPlayer <= 0)
                return;
            
            var requiredToLoad = GetRequiredChunksId();
            ChunksToRemove(requiredToLoad);
            ChunksToLoad(requiredToLoad);
        }

        private int[] GetRequiredChunksId()
        {
            int[] requiredToLoad = new int[loadedChunksAroundPlayer];
            int middleChunkId = Mathf.RoundToInt(((player.position.x - transform.position.x) / _proceduralLandscape2D.ChunkSize));// + player.position.x); * (useParallax ? parallax.BGLayers[layerId].horizontalSpeedOverCam : 1) * (useParallax ? parallax.BGLayers[layerId].horizontalSpeedOverCam : 1)
            int startChunkId = Mathf.RoundToInt(middleChunkId - (loadedChunksAroundPlayer / 2f));
            
            for (int i = 0; i < requiredToLoad.Length; i++)
            {
                requiredToLoad[i] = i + startChunkId;
            }
            
            return requiredToLoad;
        }

        private async void ChunksToLoad(int[] requiredToLoadId)
        {
            for (int i = 0; i < requiredToLoadId.Length; i++)
            {
                if (_loadedChunks.Find(x => x.ID == requiredToLoadId[i]) == null)
                {
                    await Load(requiredToLoadId[i]);
                }
            }
        }

        private async Task Load(int chunkId)
        {
            var c = new ChunkData
            {
                ID = chunkId
            };
            _loadedChunks.Add(c);
            var chunk = await _proceduralLandscape2D.Generate(chunkId);
            if (chunk != null)
                c.Object = chunk.gameObject;
        }

        private void ChunksToRemove(int[] requiredToLoadId)
        {
            var toLoad = requiredToLoadId.ToList();

            for (int i = 0; i < _loadedChunks.Count; i++)
            {
                if (!toLoad.Contains(_loadedChunks[i].ID))
                {
                    Remove(_loadedChunks[i].ID);
                }
            }
        }

        private void Remove(int chunkId)
        {
            for (int i = 0; i < _loadedChunks.Count; i++)
            {
                if (_loadedChunks[i].ID == chunkId)
                {
                    if (_loadedChunks[i].Object != null)
                        DestroyImmediate(_loadedChunks[i].Object);
                    _loadedChunks.RemoveAt(i);
                }
            }
        }
        public void ClearAll()
        {
            for (int i = 0; i < _loadedChunks.Count; i++)
            {
                    if (_loadedChunks[i].Object != null)
                        DestroyImmediate(_loadedChunks[i].Object);
                    _loadedChunks.RemoveAt(i);
            }
        }
    }

    [System.Serializable]
    public class ChunkData
    {
        public int ID;
        public GameObject Object;
    }
}
