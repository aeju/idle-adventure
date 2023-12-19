using System.Linq;
using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.Extra.LandscapeSettingsGameView
{
    [RequireComponent(typeof(ProceduralLandscape2D))]
    public class LandscapeSettingsView : MonoBehaviour
    {
        private ProceduralLandscape2D landscape2D;
        private DynamicLoader.DynamicLoader dynamicLoader;
        
        private void Awake()
        {
            landscape2D = GetComponent<ProceduralLandscape2D>();
            dynamicLoader = GetComponent<DynamicLoader.DynamicLoader>();
        }
        
        public void ApplyValue(SliderView.SliderType sliderType, float newValue, string layerName)
        {
            var layer = landscape2D.GenerationLayers.First(x => x.title == layerName);
            
            switch (sliderType)
            {
                case SliderView.SliderType.Scale:
                    layer.generationScale = newValue;
                    break;
                case SliderView.SliderType.Power:
                    layer.verticalPower = newValue;
                    break;
            }
            
            Regenerate();
        }

        private async void Regenerate()
        {
            
            if (dynamicLoader == null)
            {
                await landscape2D.Generate();        
            }
            else
            {
                dynamicLoader.ClearAll();
            }
        }

        
    }
}
