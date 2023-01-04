using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOver2 : MonoBehaviour
{
    public Text textField;
    public void SetText()
    {
      textField.text = "GAME OVER" + "\n" + Game2.winner + " WINS!";
    }

    void Start()
    {
        SetText();
    }
    
}
