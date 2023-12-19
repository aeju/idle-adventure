#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

namespace Packages.Procedural_Landscape_2D.Code.Editor
{
    using UnityEditor;
    public class ProceduralLandscapeMenuInteractions : Editor
    {
        private const string SpriteShapeInstancePath = "Prefabs/DefaultSpriteShapeWithCollider.prefab";
        
        
        [MenuItem("GameObject/2D Object/Landscape")]
        public static void CreateLandscape()
        {
            var go = new GameObject();
            var landscape2D = go.AddComponent<ProceduralLandscape2D>();
            
            var spriteShape = AssetDatabase.LoadAssetAtPath<SpriteShapeController>(GetPackageFolderPath() + SpriteShapeInstancePath);
            landscape2D.SetSpriteShapeInstance(spriteShape);
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        
        private static string GetPackageFolderPath()
        {
            string[] res = Directory.GetFiles(Application.dataPath, "ProceduralLandscapeMenuInteractions.cs", SearchOption.AllDirectories);
            if (res.Length == 0)
            {
                //Debug.LogError("Can not locate Sprite Gradient Package folder...");
                return null;
            }
            
            string path = res[0].Replace("Code/Editor/ProceduralLandscapeMenuInteractions.cs", "").Replace(@"Code/Editor/ProceduralLandscapeMenuInteractions.cs", "").Replace("\\", "/").Replace(@"\", "/");
            path = "Assets" + path.Replace(Application.dataPath, "");
            path = path.Replace("/", @"\");
            
            return path;
        }
    }
}
#endif