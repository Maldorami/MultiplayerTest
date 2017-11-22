using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour {

    public GameObject owner;
    float timeLife = 2f;
    float timer = 0;
    
    public void setOwner(GameObject _source)
    {
        owner = _source;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        transform.Translate(Vector3.forward * 10f * Time.deltaTime);

        if (timer > timeLife) NetworkServer.Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.CmdTakeDamage(10, owner);
        }
        Destroy(gameObject);
    }
}
