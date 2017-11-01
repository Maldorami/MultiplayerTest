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

    private void Start()
    {
        SpawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (DestroyOnDeath)
                Destroy(gameObject);
            else
            {
                currentHealth = maxHealth;
                RpcSpawn();
            }
        }
        else
        {
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }

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