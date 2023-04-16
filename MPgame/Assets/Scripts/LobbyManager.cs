using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text LogText;
    [SerializeField] InputField nickInputField;
    [SerializeField] InputField roomNameInputField;
    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1, 9999); //Никнейм
        Log("Имя игрока: " + PhotonNetwork.NickName);
        PhotonNetwork.AutomaticallySyncScene = true; //Автопереключение сцены
        PhotonNetwork.GameVersion = "1"; //Версия игры
        PhotonNetwork.ConnectUsingSettings(); //Подключается к серверу Photon
    }
    void Log(string message)
    {
        Debug.Log(message);
        LogText.text += "\n";
        LogText.text += message;
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomNameInputField.text, new Photon.Realtime.RoomOptions { MaxPlayers = 15 });
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameInputField.text);
    }
    public override void OnConnectedToMaster()
    {
        Log("Подключились к серверу Photon");
    }
    public override void OnJoinedRoom()
    {
        Log("Вошли в комнату");
        PhotonNetwork.LoadLevel("Room");
    }
    public void ChangeName()
    {
        PhotonNetwork.NickName = nickInputField.text;
        Log("Новое имя игрока: " + PhotonNetwork.NickName);
    }
}
