using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Connection : MonoBehaviour, IOnEventCallback
{

    public GameObject controller;
    public Game game;
    public const byte SendCoords = 1;
    public const byte CardEffect = 2;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game>();
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if(eventCode == SendCoords)
        {
            string s = (string)photonEvent.CustomData;
            game.Move(s);
        }
        else if(eventCode == CardEffect)
        {
            string s = (string)photonEvent.CustomData;
            UseCard(s);

        }


    }

    public void SendMove(string s)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SendCoords, s, raiseEventOptions, SendOptions.SendReliable);
    }

    public void CardMirror(string s)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CardEffect, s, raiseEventOptions, SendOptions.SendReliable);

    }

    public void UseCard(string s)
    {
        string[] arr = s.Split(',');
        string player = arr[1];
        if(player.Equals("p1"))
        {
            if(game.P2)
            {
                game.MirrorEffect(arr[0]);
            }
        }
        else if(player.Equals("p2"))
        {
            if(game.P1)
            {
                game.MirrorEffect(arr[0]);
            }
        }
        
    }



    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }



    
}
