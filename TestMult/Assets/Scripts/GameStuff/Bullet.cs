using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject owner;
    
    public void setOwner(GameObject _source)
    {
        owner = _source;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.RpcTakeDamage(10, owner);
        }
        Destroy(gameObject);
    }
}
