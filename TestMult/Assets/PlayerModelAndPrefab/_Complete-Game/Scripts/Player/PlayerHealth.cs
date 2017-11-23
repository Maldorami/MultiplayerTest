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
    
    public void CmdTakeDamage(int amount, GameObject _source)
    {
        //RpcTakeDamage(amount, _source);
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (DestroyOnDeath)
                Destroy(gameObject);
            else
            {
                if (_source != null) actScore(_source);
                Invoke("CmdPlayerDeath", 0);
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
        if (_sourceHealth == null) return;

        _sourceHealth.kills++;
        deaths++;
    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcSpawn()
    {
        currentHealth = maxHealth;
        Vector3 spawnPoint = Vector3.zero;

        if(SpawnPoints != null && SpawnPoints.Length > 0)
        {
            spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }

        transform.position = spawnPoint;
    }

    [Command]
    void CmdPlayerDeath()
    {
        RpcPlayerDeath();
    }

    [ClientRpc]
    void RpcPlayerDeath(){
        if (isLocalPlayer)
        {
            deaths++;
        }else
        {
            kills++;
        }
    }

}