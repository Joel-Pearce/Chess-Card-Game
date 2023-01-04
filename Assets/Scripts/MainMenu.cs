using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public static string colour;
    public static int difficulty;
    public void StartButton()
    {
        StartCoroutine(Wait());
        SceneManager.LoadScene("ModeScreenBranch");
    }

    public void QuitButton()
    {
        StartCoroutine(Wait());
        Application.Quit();
    }

    public void WhiteButton()
    {
        StartCoroutine(Wait());
        colour = "white";
        SceneManager.LoadScene("GameScreenAI");
    }

    public void BlackButton()
    {
        StartCoroutine(Wait());
        colour = "black";
        SceneManager.LoadScene("GameScreenAI");
    }

    public void SinglePlayerButton()
    {
        StartCoroutine(Wait());
        SceneManager.LoadScene("ModeScreenAIDifficulty");
    }

    public void Level1()
    {
        StartCoroutine(Wait());
        difficulty = 1;
        SceneManager.LoadScene("ModeScreenBlackOrWhite");
    }

    public void Level2()
    {
        StartCoroutine(Wait());
        difficulty = 2;
        SceneManager.LoadScene("ModeScreenBlackOrWhite");
    }

    public void Level3()
    {
        StartCoroutine(Wait());
        difficulty = 3;
        SceneManager.LoadScene("ModeScreenBlackOrWhite");
    }




    public void MultiplayerButton()
    {
        StartCoroutine(Wait());
        SceneManager.LoadScene("Loading");
    }

    public void BackToBranch()
    {
        StartCoroutine(Wait());
        SceneManager.LoadScene("ModeScreenBranch");
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }

    public void JoinRoom()
    {
        colour = "black";
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    //following two methods learnt from tutorial @ https://www.youtube.com/watch?v=fbk_SIhbjDc&ab_channel=InfoGamer
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions {IsVisible = true, IsOpen = true, MaxPlayers = 2};
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions {IsVisible = true, IsOpen = true, MaxPlayers = 2};
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);

    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScreenMultiplayer");
    }

    private IEnumerator Wait()
    {
        
        yield return new WaitForSeconds(3);

    }


}
