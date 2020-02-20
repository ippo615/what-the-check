using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject objectToEnable;
    public bool pressToActivate = true;
    public int collidersToActivate = 1;

    private bool isActive = false;
    private List<GameObject> collidingObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        collidingObjects.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        collidingObjects.Remove(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        isActive = (collidingObjects.Count > collidersToActivate);
        if( isActive )
        {
            objectToEnable.SetActive(pressToActivate);
        }
        else {
            objectToEnable.SetActive(!pressToActivate);
        }
    }
}
