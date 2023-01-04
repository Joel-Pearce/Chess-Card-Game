using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class GameOver : MonoBehaviour
{
    public Text textField;
    public void SetText()
    {
      textField.text = "GAME OVER" + "\n" + Game.winner + " WINS!";
      PhotonNetwork.LeaveRoom();
    }

    void Start()
    {
        SetText();
    }
    
}
