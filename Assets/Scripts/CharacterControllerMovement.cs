using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float jumpBufferEarlySeconds = 0.05f;
    public float jumpBufferLateSeconds = 0.05f;

    private Vector3 playerMove = Vector3.zero;
    private float jumpBufferPressedTime = 0.0f;
    private bool isJumping = false;

    private Transform warpTarget = null;
    private Transform warpOrigin = null;
    private Transform oldWarpOrigin = null;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup")) {
            other.gameObject.SetActive(false);
        }
    }
    public void WarpTo( Transform newWarpTarget, Transform newWarpOrigin )
    {
        warpTarget = newWarpTarget;
        oldWarpOrigin = warpOrigin;
        warpOrigin = newWarpOrigin;
    }

    void Update()
    {
        if( warpTarget )
        {
            if(oldWarpOrigin == warpTarget)
            {
                warpTarget = null;
                return;
            }
            transform.SetPositionAndRotation(warpTarget.position, warpTarget.rotation);
            warpTarget = null;
            return;
        }
        // This is odd:
        // When pressing left+up "Jump" will not fire.
        // When pressing right+down "Jump" will note fire.
        // Other combos (right+up), (left+down) seem to work.
        if( Input.GetButtonDown("Jump") )
        {
            jumpBufferPressedTime = Time.time;
        }
        float jumpBufferDelta = Time.time - jumpBufferPressedTime;

        float gravityState = playerMove.y;

        // We convert the camera coordinates to world coordinates that are
        // "aligned" to the xz-plane. For this we assume that the camera
        // does not rotate on it's forward axis (ie you do not "tilt" your
        // head like an owl). This allows for any up/down, or side-to-side
        // viewing to work the same. We use the camera's up and forward
        // vectors for the vertical axis and the right vector for the horizontal
        // axis.
        Vector3 vCamera = Camera.main.transform.forward + Camera.main.transform.up;
        Vector3 hCamera = Camera.main.transform.right;

        // Normalize to make sure one direction is not faster than another.
        // Note: we probably dont need to normalize hCamera because it uses 1 unit vector.
        vCamera.Normalize();

        // Given player input -- compute the direction the player wants to move
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        playerMove = vCamera * vInput + hCamera * hInput;

        // The y-axis is always used for jumping so it should not be
        // affected by directional input. It should only be affected by
        // jumping and gravity
        playerMove.y = 0.0f;

        // Make sure that moving diagonally is the same speed as moving
        // horizontally or vertically. Then apply the speed.
        playerMove.Normalize();
        playerMove *= speed;

        if (characterController.isGrounded)
        {
            isJumping = false;
        }
        else
        {
            // Re-apply the saved gravity state
            playerMove.y = gravityState;

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            playerMove.y -= gravity * Time.deltaTime;
        }

        if (!isJumping)
        {
            // Compute jumping
            if (jumpBufferDelta < jumpBufferEarlySeconds || jumpBufferDelta < jumpBufferLateSeconds)
            {
                playerMove.y = jumpSpeed;
                isJumping = true;
            }
        }

        // Move the controller
        characterController.Move(playerMove * Time.deltaTime);

    }

}
