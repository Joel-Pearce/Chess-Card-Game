using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class MoveSquare : MonoBehaviour
{
    public GameObject controller;
    public Game game;

    public GameObject vessel;
    public Connection connection;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game>();

        vessel = GameObject.FindGameObjectWithTag("Connection");
        connection = vessel.GetComponent<Connection>();
    }

    public Vector2 getPosition()
    {
        return transform.position;
    }

    public string getSquare()
    {
        return Game.square_references[transform.position];
    }

    void OnMouseDown()
    {
       
       
    }
    void OnMouseUp()
    {
        if(Game.coords.Length == 3)
        {
            Game.coords = Game.coords + getSquare();
            if(PhotonNetwork.IsConnected)
            {
                connection.SendMove(Game.coords);
            }
            else
            {
                game.Move(Game.coords);
            }
            Game.coords = "";
            DestroyMoveSquares();
        }
    }
    public void DestroyMoveSquares()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MoveSquare");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); 
        }
    }



}
