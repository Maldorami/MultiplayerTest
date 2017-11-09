using System;
using UnityEngine;

public class UserAccountDataTranslator : MonoBehaviour {

    private static string KILLS_TAG = "[KILLS]";
    private static string DEATH_TAG = "[DEATHS]";

    public static string DataToSend(int kills, int deaths)
    {
        return KILLS_TAG + kills.ToString() + "/" + DEATH_TAG + deaths.ToString();
    }

    public static int DataToKills(string data)
    {
        return int.Parse(DataToValue(data, KILLS_TAG));
    }

    public static int DataToDeath(string data)
    {
        return int.Parse(DataToValue(data, DEATH_TAG));
    }

    public static string DataToValue(string data, string TAG)
    {
        string[] pieces = data.Split('/');
        foreach (string piece in pieces)
        {
            if (piece.StartsWith(TAG))
            {
                return piece.Substring(TAG.Length);                
            }
        }

        Debug.LogError("Error: DATATOVALUE");
        return "";
    }
}
