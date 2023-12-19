using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.Extra.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        public float moveSpeed = 10;
        private float _currentSpeed;
        
        private void Update()
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, moveSpeed, Time.deltaTime * 10);
            transform.position += Vector3.right * _currentSpeed * Time.deltaTime; 
        }
    }
}
