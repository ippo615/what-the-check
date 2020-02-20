using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{

    public List<GameObject> targets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject gob in targets){
                gob.SetActive(!gob.activeInHierarchy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
