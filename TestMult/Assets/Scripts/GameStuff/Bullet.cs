using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    public GameObject owner;
    float timeLife = 2f;
    float timer = 0;
    
    private void Update()
    {
        timer += Time.deltaTime;

        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.CmdTakeDamage(10, owner);
        }

        Destroy(this.gameObject);
    }
}
