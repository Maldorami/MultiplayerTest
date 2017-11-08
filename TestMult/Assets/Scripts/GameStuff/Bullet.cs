using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
