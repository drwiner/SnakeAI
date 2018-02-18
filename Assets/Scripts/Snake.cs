using UnityEngine;
using System;

public class Snake : MonoBehaviour {

    [SerializeField]
    public Snake Next { get; set; }

    static public Action<String> hit;
    
    public void RemoveTail()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hit != null)
        {
            hit(other.tag);
        }
        if (other.tag == "Food")
        {
            Destroy(other.gameObject);
        }
    }

}
