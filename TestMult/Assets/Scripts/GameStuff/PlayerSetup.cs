using UnityEngine.Networking;
using UnityEngine;


public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    GameObject playerUIPrefab;
    GameObject playerUIInstace;

    private void Start()
    {
        if (isLocalPlayer)
        {
            //playerUIInstace = Instantiate(playerUIPrefab);
            //playerUIInstace.name = playerUIPrefab.name;
        }
    }


    private void OnDisable()
    {
        Destroy(playerUIInstace);
    }

}
