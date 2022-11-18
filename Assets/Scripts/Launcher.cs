using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public Button btn;
    public TMP_Text feedbackText;

    private byte maxPlayersPerRoom = 2;

    bool isConnecting;
    string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        feedbackText.text = "";
        isConnecting = true;
        btn.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            LogFeedback("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    void LogFeedback(string message)
    {
        if (feedbackText == null)
        {
            return;
        }
        feedbackText.text += System.Environment.NewLine + message;
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogFeedback("OnJoinRandomFailed : next -> Create a new Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LogFeedback("OnDisconnected" + cause);
        isConnecting = false;
        btn.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        LogFeedback("OnJoinedRoom with " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("BattleScene");
        }
    }
}
