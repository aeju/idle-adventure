#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Packages._2D_Gradient_with_Transparency.Editor
{
    public class SpriteUnlitGradientEditorButtons : UnityEditor.Editor
    {
        private const string ShaderName = "Sprite-Unlit-Gradient";
        
        private const string CircleSpritePath = @"Sprites\Circle.png";
        private const string SquareSpritePath = @"Sprites\Square.png";

        private const string UserMaterialsPath = @"User Materials\";

        [MenuItem("GameObject/2D Object/Gradient Square", false, -501)]
        public static void CreateGradientSquare()
        {
            var go = new GameObject();
            go.name = "Linear Gradient";
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            go.transform.localScale = Vector3.one * 5;
            
            Debug.Log($"Sprite path: {GetPackageFolderPath() + SquareSpritePath}");

            spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + SquareSpritePath);
            spriteRenderer.material = CreateOrFindMaterial(DefaultLinear, "Linear Gradient Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        [MenuItem("GameObject/2D Object/Gradient Circle", false, -501)]
        public static void CreateGradientCircle()
        {
            var go = new GameObject();
            go.name = "Radial Gradient";
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            go.transform.localScale = Vector3.one * 5;
            
            spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + CircleSpritePath);
            spriteRenderer.material = CreateOrFindMaterial(DefaultRadial, "Radial Gradient Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        [MenuItem("GameObject/2D Object/Transparent Gradient Square", false, -500)]
        public static void CreateTransparentGradientSquare()
        {
            var go = new GameObject();
            go.name = "Linear Transparent";
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            go.transform.localScale = Vector3.one * 5;
            
            spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + SquareSpritePath);
            spriteRenderer.material = CreateOrFindMaterial(DefaultLinearTransparent, "Linear Transparent Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        [MenuItem("GameObject/2D Object/Transparent Gradient Circle", false, -500)]
        public static void CreateTransparentGradientCircle()
        {
            var go = new GameObject();
            go.name = "Radial Transparent";
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            go.transform.localScale = Vector3.one * 5;
            
            spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + CircleSpritePath);
            spriteRenderer.material = CreateOrFindMaterial(DefaultRadialTransparent, "Radial Transparent Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        
        
        
        
        
        [MenuItem("GameObject/UI/Gradient Square", false, 1)]
        public static void CreateLinearGradientImage()
        {
            var canvas = FindOrCreateCanvas();
            var go = new GameObject();
            go.transform.SetParent(canvas);
            go.name = "Linear Gradient";
            var image = go.gameObject.AddComponent<Image>();
            image.rectTransform.anchoredPosition = Vector2.zero;
            go.transform.localScale = Vector3.one;

            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + SquareSpritePath);
            image.material = CreateOrFindMaterial(DefaultLinear, "Linear Gradient Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        
        [MenuItem("GameObject/UI/Gradient Circle", false, 2)]
        public static void CreateRadialGradientImage()
        {
            var canvas = FindOrCreateCanvas();
            var go = new GameObject();
            go.transform.SetParent(canvas);
            go.name = "Radial Gradient";
            var image = go.gameObject.AddComponent<Image>();
            image.rectTransform.anchoredPosition = Vector2.zero;
            go.transform.localScale = Vector3.one;

            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + CircleSpritePath);
            image.material = CreateOrFindMaterial(DefaultRadial, "Radial Gradient Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        
        [MenuItem("GameObject/UI/Transparent Gradient Square", false, 3)]
        public static void CreateLinearTransparentImage()
        {
            var canvas = FindOrCreateCanvas();
            var go = new GameObject();
            go.transform.SetParent(canvas);
            go.name = "Linear Transparent";
            var image = go.gameObject.AddComponent<Image>();
            image.rectTransform.anchoredPosition = Vector2.zero;
            go.transform.localScale = Vector3.one;

            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + SquareSpritePath);
            image.material = CreateOrFindMaterial(DefaultLinearTransparent, "Linear Transparent Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }
        
        [MenuItem("GameObject/UI/Transparent Gradient Circle", false, 4)]
        public static void CreateRadialTransparentImage()
        {
            var canvas = FindOrCreateCanvas();
            var go = new GameObject();
            go.transform.SetParent(canvas);
            go.name = "Radial Transparent";
            var image = go.gameObject.AddComponent<Image>();
            image.rectTransform.anchoredPosition = Vector2.zero;
            go.transform.localScale = Vector3.one;

            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(GetPackageFolderPath() + CircleSpritePath);
            image.material = CreateOrFindMaterial(DefaultRadialTransparent, "Radial Transparent Material");
            
            Undo.RegisterCreatedObjectUndo (go, "Created go");
            Selection.objects = new Object[] {go};
        }

        private static string GetPackageFolderPath()
        {
            string[] res = Directory.GetFiles(Application.dataPath, "SpriteUnlitGradientEditorButtons.cs", SearchOption.AllDirectories);
            if (res.Length == 0)
            {
                //Debug.LogError("Can not locate Sprite Gradient Package folder...");
                return null;
            }
            
            string path = res[0].Replace("Editor/SpriteUnlitGradientEditorButtons.cs", "").Replace(@"Editor\SpriteUnlitGradientEditorButtons.cs", "").Replace("\\", "/").Replace(@"\", "/");
            path = "Assets" + path.Replace(Application.dataPath, "");
            path = path.Replace("/", @"\");
            
            return path;
        }
        private static bool GetSelectedMaterial(out Material material)
        {
            material = AssetDatabase.LoadAssetAtPath<Material>(GetSelectedPath());
            if (material == null)
                return false;

            if (material.shader.name != "Sprite-Unlit-Gradient")
                return false;

            return true;
        }
        private static string GetSelectedPath()
        {
            string path = "Assets";
  
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
                path = AssetDatabase.GetAssetPath(obj);
            
            return path;
        }


        private static Transform FindOrCreateCanvas()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                canvas = new GameObject().AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                GameObject gameObjectCanvas;
                (gameObjectCanvas = canvas.gameObject).AddComponent<CanvasScaler>();
                gameObjectCanvas.name = "Canvas";
                
                Undo.RegisterCreatedObjectUndo (gameObjectCanvas, "Created go");
            }

            return canvas.transform;
        }
        
        private static Material CreateOrFindMaterial(ColorsStruct materialData, string assetName)
        {
            if (GetSelectedMaterial(out var selected))
            {
                if (CreateStructFromMaterial(out var selectedMaterialData, selected))
                {
                    materialData.Color0Hex = selectedMaterialData.Color0Hex;
                    materialData.Color1Hex = selectedMaterialData.Color1Hex;
                    materialData.Color2Hex = selectedMaterialData.Color2Hex;
                }
            }

            Material material;
            if (!FindExistingMaterial(out material, materialData))
            {
                material = CreateMaterialFromStruct(materialData);
                CreateMaterialAsset(material, assetName);
            }

            return material;
        }
        public static bool FindExistingMaterial(out Material material, ColorsStruct parameters)
        {
            var path = Application.dataPath + GetPackageFolderPath().Remove(0, 6) + UserMaterialsPath;
            //Debug.Log("Path: " + path);

            if (!Directory.Exists(path))
            {
                material = null;
                return false;
            }
            
            var assets = Directory.GetFiles(path, "*.mat");
            //Debug.Log("Assets Found: " + assets.Length);
            for (int i = 0; i < assets.Length; i++)
            {
                var assetPath = "Assets" + assets[i].Replace(Application.dataPath, "");
                //Debug.Log("Checking asset: " + assetPath);
                var materialAsset = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                //Debug.Log("Material asset: " + materialAsset);
                if (materialAsset is null)
                {
                    //Debug.Log("Material asset is null");
                    continue;
                }

                if (!CreateStructFromMaterial(out var colorsStruct, materialAsset))
                {
                 //Debug.Log("Can not create struct from material");
                    continue;
                }

                if (IsEqual(colorsStruct, parameters))
                {
                    material = materialAsset;
                    return true;
                }
                //Debug.Log("");
            }

            //Debug.Log("Assets Not Found");
            
            material = null;
            return false;
        }
        public static Material CreateMaterialAsset(Material material, string assetName)
        {
            var fullPathToUserMat = Application.dataPath + GetPackageFolderPath().Replace("Assets", "") + UserMaterialsPath;

            if (!Directory.Exists(fullPathToUserMat))
            {
                Directory.CreateDirectory(fullPathToUserMat);
            }
            
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(GetPackageFolderPath() + UserMaterialsPath + assetName +".mat");
            AssetDatabase.CreateAsset(material, uniqueFileName);
            AssetDatabase.Refresh();

            return material;
        }
        public static bool IsEqual(ColorsStruct struct1, ColorsStruct struct2)
        {
            //Debug.Log("Checking Is Equal");
            if (Math.Abs(struct1.Center - struct2.Center) > 0.001f)
            {
                //Debug.Log("Center Not Equal");
                return false;
            }

            if (Math.Abs(struct1.Rotate - struct2.Rotate) > 0.001f)
            {
                //Debug.Log("Rotate Not Equal");
                return false;
            }

            if (struct1.Radial != struct2.Radial)
            {
                //Debug.Log("Radial Not Equal");
                return false;
            }

            if (struct1.Color0Hex != struct2.Color0Hex)
            {
                //Debug.Log(struct1.Color0Hex);
                //Debug.Log(struct2.Color0Hex);
                //Debug.Log("Color 0 Hex Not Equal");
                return false;
            }

            if (struct1.Color1Hex != struct2.Color1Hex)
            {
                //Debug.Log("Color 1 Hex Not Equal");
                return false;
            }

            if (struct1.Color2Hex != struct2.Color2Hex)
            {
                //Debug.Log("Color 2 Hex Not Equal");
                return false;
            }

            if (Math.Abs(struct1.Alpha0 - struct2.Alpha0) > 0.001f)
            {
                //Debug.Log("Alpha 0 Not Equal");
                return false;
            }

            if (Math.Abs(struct1.Alpha1 - struct2.Alpha1) > 0.001f)
            {
                //Debug.Log("Alpha 1 Not Equal");
                return false;
            }

            if (Math.Abs(struct1.Alpha2 - struct2.Alpha2) > 0.001f)
            {
                //Debug.Log("Alpha 2 Not Equal");
                return false;
            }

            return true;
        }
        
        public static Material CreateMaterialFromStruct(ColorsStruct colorsStruct)
        {
            var material = new Material (Shader.Find("Sprite-Unlit-Gradient"));
            
            material.SetFloat("_center", colorsStruct.Center);
            material.SetFloat("_rotate", colorsStruct.Rotate);
            material.SetFloat("_radial", colorsStruct.Radial ? 1 : 0);

            material.SetColor("_Color0", colorsStruct.Color0);
            material.SetColor("_Color1", colorsStruct.Color1);
            material.SetColor("_Color2", colorsStruct.Color2);

            return material;
        }

        public static bool CreateStructFromMaterial(out ColorsStruct colorStruct, Material material)
        {
            if (material.shader.name != ShaderName)
            {
                colorStruct = new ColorsStruct();
                return false;
            }

            var center = material.GetFloat("_center");
            var rotate = material.GetFloat("_rotate");
            bool radial = material.GetFloat("_radial") != 0;

            var c0 = ColorUtility.ToHtmlStringRGB(material.GetColor("_Color0"));
            var c1 = ColorUtility.ToHtmlStringRGB(material.GetColor("_Color1"));
            var c2 = ColorUtility.ToHtmlStringRGB(material.GetColor("_Color2"));
            
            var a0 = material.GetColor("_Color0").a;
            var a1 = material.GetColor("_Color1").a;
            var a2 = material.GetColor("_Color2").a;
            
            colorStruct = new ColorsStruct(c0, a0, c1, a1, c2, a2, radial, rotate, center);
            return true;
        }

        private static ColorsStruct DefaultLinear =>
            new ColorsStruct("#DE6262", 1f, "#FFB88C", 1f, "#FFFFFF", 0f, false, 0.25f, 0.999f);
        //Добавить поле AlphaBlend
        private static ColorsStruct DefaultRadial => 
            new ColorsStruct("#BDC3C6", 1f, "#2C3E50", 1f, "#FFFFFF", 0f, true, 0f, 0.999f);
        
        private static ColorsStruct DefaultLinearTransparent => 
            new ColorsStruct("#BDC3C6", 1f, "#2C3E50", 0f, "#FFFFFF", 0f, false, 0.25f, 0.999f);
        private static ColorsStruct DefaultRadialTransparent => 
            new ColorsStruct("#BDC3C6", 1f, "#2C3E50", 0f, "#FFFFFF", 0f, true, 0.25f, 0.999f);
        
        public struct ColorsStruct
        {
            public string Color0Hex;
            public readonly float Alpha0;
            public string Color1Hex;
            public readonly float Alpha1;
            public string Color2Hex;
            public readonly float Alpha2;
            public readonly bool Radial;
            public readonly float Rotate;
            public readonly float Center;
            
            
            public Color Color0
            {
                get
                {
                    Color c = Color.white;
                    ColorUtility.TryParseHtmlString("#" + Color0Hex, out c);
                    c.a = Alpha0;
                    return c;
                }
            }
            
            public Color Color1
            {
                get
                {
                    Color c = Color.white;
                    ColorUtility.TryParseHtmlString("#" + Color1Hex, out c);
                    c.a = Alpha1;
                    return c;
                }
            }
            
            public Color Color2
            {
                get
                {
                    Color c = Color.white;
                    ColorUtility.TryParseHtmlString("#" + Color2Hex, out c);
                    c.a = Alpha2;
                    return c;
                }
            }

            public ColorsStruct(string color0, float alpha0, string color1, float alpha1, string color2, float alpha2, bool radial, float rotate, float center)
            {
                Color0Hex = color0.Replace("#", "");
                Color1Hex = color1.Replace("#", "");
                Color2Hex = color2.Replace("#", "");
                Radial = radial;
                Rotate = rotate;
                Center = center;
                Alpha0 = alpha0;
                Alpha1 = alpha1;
                Alpha2 = alpha2;
            }
        }
    }
}
#endif