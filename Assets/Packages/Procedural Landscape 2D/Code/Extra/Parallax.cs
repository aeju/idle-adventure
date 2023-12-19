using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.Parallax
{
    public class Parallax : MonoBehaviour
    {
        public Camera cam;
        public BgLayer[] bgLayers;
        public bool useVertical = true;

        private void Start()
        {
            var cameraInstance = GetComponent<Camera>();
            if (cameraInstance != null)
                cam = cameraInstance;
        }

        private void Update()
        {
            if(cam == null)
                return;
            
            var velocity = cam.velocity;
            float camSpeedHorizontal = velocity.x;
            float camSpeedVertical = velocity.y;
            
            if (camSpeedHorizontal != 0 || camSpeedVertical != 0)
                foreach (BgLayer layer in bgLayers)
                {
                    float speedHorizontal = camSpeedHorizontal * layer.speedOverCam;
                    float speedVertical = camSpeedVertical * layer.speedOverCam;

                
                    foreach (GameObject bg in layer.bgs)
                    {
                        if(bg == null)
                            continue;

                        var position = bg.transform.position;
                        float z = position.z;
                        float y = position.y;
                        float x = position.x;
                        x += speedHorizontal * Time.deltaTime;
                        if (useVertical)
                            y += speedVertical * Time.deltaTime;
                        Vector3 newPosition = new Vector3(x, y, z);

                        bg.transform.position = newPosition;
                    }
                }
        }

        [System.Serializable]
        public struct BgLayer
        {
            public GameObject[] bgs;
            [Range(-1,1)]
            public float speedOverCam;
        }
    }
}
