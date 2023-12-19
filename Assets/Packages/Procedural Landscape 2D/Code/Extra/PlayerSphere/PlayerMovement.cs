using UnityEngine;

namespace Packages.Procedural_Landscape_2D.Code.Extra.PlayerSphere
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float speed = 10.0f; // maximum movement speed
        public float force = 10.0f; // force to apply on arrow key press
        public float jumpForce = 10.0f; // force to apply on jump
        public bool onGround = true; // check if the player is on the ground

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                // Apply force to move the sphere to the right
                rb.AddForce(new Vector2(force, 0));
                // Limit the speed
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                // Apply force to move the sphere to the left
                rb.AddForce(new Vector2(-force, 0));
                // Limit the speed
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y);
            }

            if (Input.GetKeyDown(KeyCode.Space) && onGround)
            {
                // Apply force to move the sphere upwards
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                onGround = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            onGround = true;
        }
    }
}
