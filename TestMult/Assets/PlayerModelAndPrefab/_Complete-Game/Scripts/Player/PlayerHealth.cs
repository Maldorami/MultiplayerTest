using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;

    public bool DestroyOnDeath;
    public NetworkStartPosition[] SpawnPoints;

    public int kills = 0;
    public int deaths = 0;

    private void Start()
    {
        SpawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount, GameObject _source)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (DestroyOnDeath)
                Destroy(gameObject);
            else
            {
                actScore(_source);
                currentHealth = maxHealth;
                RpcSpawn();
            }
        }
        else
        {
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }

    }

    void actScore(GameObject _source)
    {
        PlayerHealth _sourceHealth = _source.GetComponent<PlayerHealth>();
        if (_sourceHealth != null) _sourceHealth.kills++;
        deaths++;
    }


    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcSpawn()
    {
        Vector3 spawnPoint = Vector3.zero;

        if(SpawnPoints != null && SpawnPoints.Length > 0)
        {
            spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }

        transform.position = spawnPoint;
    }

}