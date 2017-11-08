using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour {

    private NetworkManager networkmanager;

    List<GameObject> roomList = new List<GameObject>();
    [SerializeField]
    Text status;
    [SerializeField]
    GameObject roomListItemPrefab;
    [SerializeField]
    Transform roomListParent;

    private void Start()
    {
        networkmanager = NetworkManager.singleton;
        if (networkmanager.matchMaker == null)
            networkmanager.StartMatchMaker();

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkmanager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";

        if(!success || matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }

        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if(_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }
            roomList.Add(_roomListItemGO);
        }

        if(roomList.Count == 0)
        {
            status.text = "No rooms at the moment.";
        }

    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkmanager.matchMaker.JoinMatch(_match.networkId, "","", "", 0, 0, networkmanager.OnMatchJoined);
        ClearRoomList();
        status.text = "JOINING...";
    }
}
