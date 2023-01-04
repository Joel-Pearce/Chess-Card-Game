using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class Reset : MonoBehaviour
{
    public void Restart()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameController");
        Destroy(obj);
        SceneManager.LoadScene("ModeScreenBlackOrWhite");
    }

    public void BackToLobby()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameController");
        Destroy(obj);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("ModeScreenBranch");
    }

}
