using UnityEngine;

public class SpineAnimatorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component from the GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Example: Check for user input to control Spine animations
        if (Input.GetKey(KeyCode.W))
        {
            // Trigger the "Walk" animation transition
            animator.SetTrigger("Walk");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Trigger the "Idle" animation transition
            animator.SetTrigger("Idle");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // Trigger the "Jump" animation transition
            animator.SetTrigger("Dead");
        }
    }
}