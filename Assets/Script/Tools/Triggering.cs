using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Triggering : MonoBehaviour
{

    List<GameObject> triggeredObjects = new List<GameObject>();
    bool triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggeredObjects = GetComponentsInChildren<Transform>(true).ToList().Where(t => t.gameObject != this.gameObject).Select(t => t.gameObject).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TriggeringOpen()
    {
               foreach (var obj in triggeredObjects)
        {
            obj.SetActive(true);
            if(obj.GetComponent<Enemy>()!=null)
            obj.GetComponent<Enemy>()?.TriggerActivation();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(triggered) return;
        if (collision.CompareTag("Player"))
        {
            TriggeringOpen();
            triggered = true;
        }
    }
}
