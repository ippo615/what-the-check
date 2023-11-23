using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPad : MonoBehaviour
{
    public Transform warpTarget;
    public bool isActive = true;
    public bool isWarpable = true;
    public float secondsToWarp = 0.3f;
    private float contactTime;
    private bool isContacting = false;
    private GameObject contactPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isWarpable)
        {
            return;
        }
        // Warping only works reliably if we do not use the CharacterController's .move
        // method. So I created a .WarpTo method in the character controller which will
        // keep track of the intended warp target and then warp if appropriate or move
        // regularly otherwise.
        if(collider.gameObject.CompareTag("Player"))
        {
            isContacting = true;
            contactPlayer = collider.gameObject;
            //collider.gameObject.transform.SetPositionAndRotation(warpTarget.position,warpTarget.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isContacting = false;
            if (!isWarpable)
            {
                isWarpable = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isContacting && isWarpable)
        {
            contactTime += Time.fixedDeltaTime;
            if(contactTime > secondsToWarp)
            {
                contactPlayer.GetComponent<CharacterControllerMovement>().WarpTo(warpTarget, transform);
            }
        }
    }
}
