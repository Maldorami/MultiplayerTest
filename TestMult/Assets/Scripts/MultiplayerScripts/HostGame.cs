using UnityEngine.Networking;
using UnityEngine;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 2;    
    private string roomName;

    private void Start()
    {
        if(NetworkManager.singleton.matchMaker == null)
        {
            NetworkManager.singleton.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + "players");
            //Create room
            NetworkManager.singleton.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, NetworkManager.singleton.OnMatchCreate);
        }
    }

}
