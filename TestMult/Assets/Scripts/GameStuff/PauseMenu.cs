using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class PauseMenu : MonoBehaviour {

    [SerializeField]
    Toggle pauseMenuToggle;
    NetworkManager networkManager;
    public static bool isOn = false;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        pauseMenuToggle.isOn = !pauseMenuToggle.isOn;
        isOn = pauseMenuToggle.isOn;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }

}
