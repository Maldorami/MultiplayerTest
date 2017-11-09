using UnityEngine.Networking;
using UnityEngine;

public class HealthPackManager : NetworkBehaviour
{
    public GameObject healthPack;
    public int healing;
    public float reSpawnTimer;

    float timer;
    bool healthPackSpawned = false;
    GameObject aux;

    private void Start()
    {
        aux = Instantiate(healthPack, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        aux.SetActive(false);
    }

    private void Update()
    {
        if (!healthPackSpawned)
        {
            timer += Time.deltaTime;

            if (timer > reSpawnTimer)
            {
                healthPackSpawned = true;
                aux.SetActive(true);
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if (health != null)
        {
            if (healthPackSpawned)
            {
                Debug.Log(other.name);
                health.TakeDamage(-healing, null);
                healthPackSpawned = false;
                aux.SetActive(false);
                timer = 0;
            }
        }
    }
}
