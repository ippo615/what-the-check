using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPad : MonoBehaviour
{
    public Transform warpTarget;
    public bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Warping only works reliably if we do not use the CharacterController's .move
        // method. So I created a .WarpTo method in the character controller which will
        // keep track of the intended warp target and then warp if appropriate or move
        // regularly otherwise.
        if( collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<CharacterControllerMovement>().WarpTo(warpTarget,transform);
            //collider.gameObject.transform.SetPositionAndRotation(warpTarget.position,warpTarget.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
