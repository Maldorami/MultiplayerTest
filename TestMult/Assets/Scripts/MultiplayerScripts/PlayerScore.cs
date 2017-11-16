using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    PlayerHealth player;

    private void OnEnable()
    {
        player = GetComponent<PlayerHealth>();
        //SyncScore();
    }

    private void OnDestroy()
    {
        if(player != null)
        SyncNow();
    }

    IEnumerator SyncScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SyncNow();
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
