using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.Extra.Camera
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        public Transform target; // Assign player object in the inspector
        public bool followVertical;
        
        private void FixedUpdate()
        {
            // Get the current position of the target and the camera
            Vector3 targetPosition = target.position;
            Vector3 cameraPosition = transform.position;
            
            cameraPosition.x = targetPosition.x;
            if (followVertical)
                cameraPosition.y = targetPosition.y;

            // Apply the updated position to the camera
            transform.position = cameraPosition;
        }
    }
}
