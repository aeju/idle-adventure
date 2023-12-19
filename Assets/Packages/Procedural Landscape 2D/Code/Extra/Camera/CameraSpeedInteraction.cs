using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.Extra.Camera
{
    public class CameraSpeedInteraction : MonoBehaviour
    {
        [SerializeField] private CameraMovement cameraMovement;

        public void SpeedUp()
        {
            cameraMovement.moveSpeed *= 1.1f;
        }
        
        public void SpeedDown()
        {
            cameraMovement.moveSpeed /= 1.1f;
        }
    }
}
