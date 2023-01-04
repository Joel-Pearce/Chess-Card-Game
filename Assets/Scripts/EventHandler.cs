using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class EventHandler : MonoBehaviourPunCallbacks
{
    public GameObject controller;
    public Game game;
    public int left = 0;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game>();
    }
    public void OnMouseUp()
    {
        if(GameObject.FindGameObjectsWithTag("MoveSquare").Length != 0)
        {
            DestroyMoveSquares();
        }
    }

    public void DestroyMoveSquares()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MoveSquare");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); 
            Game.coords = "";
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        left++;
        base.OnPlayerLeftRoom(otherPlayer);
        if(game.P1 && left == 1)
        {
            GameOver("WHITE");

        }
        else if (left == 1)
        {
            GameOver("BLACK");
        }

    }

    public void GameOver(string colour)
    {
      SceneManager.LoadScene("GameOver");
      Game.winner = colour;
    }


}
