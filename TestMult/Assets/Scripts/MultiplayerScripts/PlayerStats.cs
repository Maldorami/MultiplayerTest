using UnityEngine.UI;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField]
    Text killCount;
    [SerializeField]
    Text deathCount;

    private void Start()
    {
        if (UserAccountManager.isLoggedIn)
            UserAccountManager.instance.GetData(OnReceivedData);
    }

    void OnReceivedData(string data)
    {
        killCount.text = UserAccountDataTranslator.DataToKills(data).ToString() + " KILLS";
        deathCount.text = UserAccountDataTranslator.DataToDeath(data).ToString() + " DEATHS";
    }

    public void RecibeData()
    {
        killCount.text = "Loading...";
        deathCount.text = "Loading...";

        if (UserAccountManager.isLoggedIn)
            UserAccountManager.instance.GetData(OnReceivedData);
    }
}
