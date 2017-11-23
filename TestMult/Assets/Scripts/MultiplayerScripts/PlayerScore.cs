using System.Collections;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    PlayerHealth player;
    float timeBetweenSync = 1;
    float timer = 0;

    private void OnEnable()
    {
        player = GetComponent<PlayerHealth>();
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenSync)
        {
            SyncNow();
            timer = 0;
        }
    }

    void SyncNow()
    {
        if (UserAccountManager.isLoggedIn)
            UserAccountManager.instance.GetData(OnReceivedData);
    }

    void OnReceivedData(string data)
    {
        if (player.kills == 0 && player.deaths == 0)
            return;

        int kills = UserAccountDataTranslator.DataToKills(data);
        int deaths = UserAccountDataTranslator.DataToDeath(data);

        kills += player.kills;
        deaths += player.deaths;

        player.kills = 0;
        player.deaths = 0;

        string newData = UserAccountDataTranslator.DataToSend(kills, deaths);
        UserAccountManager.instance.SendData(newData);
    }

}
