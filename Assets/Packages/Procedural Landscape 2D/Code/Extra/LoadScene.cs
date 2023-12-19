using UnityEngine;
using UnityEngine.SceneManagement;

namespace Packages.Procedural_Landscape_2D.Code.Extra
{
    public class LoadScene : MonoBehaviour
    {
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
