using UnityEngine.Networking;
using UnityEngine;

public class HealthPackManager : NetworkBehaviour
{
    public GameObject healthPack;
    public int healing;
    public float reSpawnTimer;

    float timer;
    bool healthPackSpawned = false;

    private void Update()
    {
        if (!healthPackSpawned)
        {
            timer += Time.deltaTime;

            if (timer > reSpawnTimer)
            {
                GameObject aux = (GameObject)Instantiate(healthPack, transform.position, transform.rotation);
                healthPackSpawned = true;
                NetworkServer.Spawn(aux);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
        //Debug.Log(collision.gameObject.name);
        //PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        //health.TakeDamage(-healing);
        //healthPackSpawned = false;
        //Destroy(healthPack);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(10);
        }
    }

}
