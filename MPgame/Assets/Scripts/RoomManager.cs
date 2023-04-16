using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text ChatText;
    [SerializeField] InputField InputText;
    void Log(string message)
    {
        Debug.Log(message);
        ChatText.text += "\n";
        ChatText.text += message;
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    //События комнаты
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Log(newPlayer.NickName + " зашел в комнату");
        playersList.Add(newPlayer.NickName);
        RefreshPlayers();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Log(otherPlayer.NickName + " вышел из комнаты");
        playersList.Remove(otherPlayer.NickName);
        RefreshPlayers();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    //RPC методы
    [PunRPC]
    public void ShowMessage(string message, PhotonMessageInfo info)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }
    public void Send()
    {
        if (string.IsNullOrWhiteSpace(InputText.text)) { return; }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            InputText.text = string.Empty;
        }
    }



    [SerializeField] List<string> playersList;
    [SerializeField] Text PlayersText;
    private void Start()
    {
        playersList.Add(PhotonNetwork.NickName);
        RefreshPlayers();
    }

    void RefreshPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ShowPlayers", RpcTarget.All, "Игроки" + "\n" + string.Join("\n", playersList));
        }
    }

    [PunRPC]
    public void ShowPlayers(string players)
    {
        PlayersText.text = players;
    }


}
