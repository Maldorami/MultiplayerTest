using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject owner;
    
    public void setOwner(GameObject _source)
    {
        owner = _source;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.TakeDamage(10, owner);
        }
        Destroy(gameObject);
    }
}
