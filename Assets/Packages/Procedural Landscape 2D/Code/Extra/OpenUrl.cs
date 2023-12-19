using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.Extra
{
    public class OpenUrl : MonoBehaviour
    {
        public void Open(string url)
        {
            Application.OpenURL(url);
        }
    }
}
