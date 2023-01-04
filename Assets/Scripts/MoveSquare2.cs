using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveSquare2 : MonoBehaviour
{
    public GameObject AI;
    public AI computer;
    public GameObject controller;
    public Game2 game;


    void Start()
    {
        AI = GameObject.FindGameObjectWithTag("AI");
        computer = AI.GetComponent<AI>();
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game2>();

    }

    public Vector2 getPosition()
    {
        return transform.position;
    }

    public string getSquare()
    {
        return Game2.square_references[transform.position];
    }

    void OnMouseDown()
    {
       
       
    }
    void OnMouseUp()
    {
        if(Game2.coords.Length == 3)
        {
            Game2.coords = Game2.coords + getSquare();
            game.Move(Game2.coords);
            Game2.coords = "";
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
