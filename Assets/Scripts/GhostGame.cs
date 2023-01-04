using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GhostGame : MonoBehaviour
{
    System.Random rndAI = new System.Random();
    public GameObject ai;
    public AI computer;
    public GameObject white_king, white_queen, white_rook, white_bishop, white_knight, white_pawn;
    public GameObject black_king, black_queen, black_rook, black_bishop, black_knight, black_pawn;
    private GameObject[] black = new GameObject[16];
    private GameObject[] white = new GameObject[16];
    public GameObject[,] positions = new GameObject[8, 8];
    public static bool whiteMove;
    public static bool blackMove;
    public static string winner;
    public static bool blackCastle = false;
    public static bool whiteCastle = false;
    public static List<bool> lastMoveDestroyed = new List<bool>();
    public static List<string> previousMoves = new List<string>();
    public static List<GameObject> destroyedPieces = new List<GameObject>();
    public static string coords = "";
    public static bool destroyPawn = false;

    public static int moves;
    public static int cardsUsed;
    public static string cardJustUsed;
    public List<GameObject> enemyCards = new List<GameObject>();

    public static bool whiteEnPassantX = false, blackEnPassantX = false, whitePromotionX = false, blackPromotionX = false;
    public bool P1 = false, P2 = false;
    public static string[] squareNames = {"a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8",
    "b1", "b2", "b3", "b4", "b5", "b6", "b7", "b8",
    "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8",
    "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8",
    "e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8",
    "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8",
    "g1", "g2", "g3", "g4", "g5", "g6", "g7", "g8",
    "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8"};

    public static Dictionary<string, Vector2> squares = new Dictionary<string, Vector2>();
    public static Dictionary<Vector2, string> square_references = new Dictionary<Vector2, string>();
    public static Dictionary<string, Vector2> cards = new Dictionary<string, Vector2>();
    public static Dictionary<Vector2, string> card_references = new Dictionary<Vector2, string>();
    private float x = -3.94f;
    private float y = -3.92f;
    public static int whiteLifePoints = 1000;
    public static int blackLifePoints = 1000;
    public void Start()
    {
      ai = GameObject.FindGameObjectWithTag("AI");
      computer = ai.GetComponent<AI>();

      var rand = new System.Random();
      int Index1 = rand.Next(6);
      int Index2 = rand.Next(6);
      int Index3 = rand.Next(6);
      int Index4 = rand.Next(6);
      int Index5 = rand.Next(6);
      int Index6 = rand.Next(6);

      for(int i = 0; i < 8; i++)
      {
        for(int j = 0; j < 8; j++)
        {
          positions[i, j] = null;
        }
      }

      if(MainMenu.colour.Equals("black"))
      {
        Array.Reverse(squareNames);
      }

      whiteMove = true;
      blackMove = false;
        for (int i = 0; i < 64; i++)
        {
        if (i % 8 == 0 && i > 0)
        {
            y = -3.94f;
            x += 1.1f;
        }
        squares.Add(squareNames[i], new Vector2(x, y));
        square_references.Add(new Vector2(x, y), squareNames[i]);
        y += 1.1f;


        }

        white = new GameObject[] { Spawn(white_king, "e1"), Spawn(white_queen, "d1"), Spawn(white_rook, "a1"),
        Spawn(white_rook, "h1"), Spawn(white_bishop, "c1"), Spawn(white_bishop, "f1"), Spawn(white_knight, "b1"),
        Spawn(white_knight, "g1"), Spawn(white_pawn, "a2"), Spawn(white_pawn, "b2"), Spawn(white_pawn, "c2"),
        Spawn(white_pawn, "d2"), Spawn(white_pawn, "e2"), Spawn(white_pawn, "f2"), Spawn(white_pawn, "g2"),
        Spawn(white_pawn, "h2") };
        black = new GameObject[] { Spawn(black_king, "e8"), Spawn(black_queen, "d8"), Spawn(black_rook, "a8"),
        Spawn(black_rook, "h8"), Spawn(black_bishop, "c8"), Spawn(black_bishop, "f8"), Spawn(black_knight, "b8"),
        Spawn(black_knight, "g8"), Spawn(black_pawn, "a7"), Spawn(black_pawn, "b7"), Spawn(black_pawn, "c7"),
        Spawn(black_pawn, "d7"), Spawn(black_pawn, "e7"), Spawn(black_pawn, "f7"), Spawn(black_pawn, "g7"),
        Spawn(black_pawn, "h7") };

        if(MainMenu.colour.Equals("white"))
        {
          this.P1 = true;
        }
        else
        {
          this.P2 = true;
        }
       
    }

    public GameObject Spawn(GameObject name, string square)
      {
        GameObject obj = Instantiate(name, makeVector(squares[square]), Quaternion.identity);
        Piece2 piece = obj.GetComponent<Piece2>(); 
        Add2DArray(square, obj);
        return obj;
      }

    public static Vector3 makeVector(Vector2 v)
    {
      return new Vector3(v.x, v.y, -10);
    }

    public bool isValidSquare(string square)
    {

      if(squareNames.Contains(square))
      {
        return true;
      }
      return false;
    }

    public bool isOccupied(string square)
    {
        var objects = GameObject.FindGameObjectsWithTag("Piece");
        foreach(var obj in objects)
        {
            Piece2 piece = obj.GetComponent<Piece2>();
            if(square.Equals(piece.getSquare()))
            {
              return true;

            }
            
        }

        return false;
    }

    public void Add2DArray(string square, GameObject obj)
    {
      int x = (int) square[0] - 97;
      int y = (int) Char.GetNumericValue(square[1]) - 1;
      positions[x, y] = obj;
    }

    public void Empty2DArray(string square)
    {
      int x = (int) square[0] - 97;
      int y = (int) Char.GetNumericValue(square[1]) - 1;
      positions[x, y] = null;

    }

    public void DestroyPiece(string square)
    {
      int x = (int) square[0] - 97;
      int y = (int) Char.GetNumericValue(square[1]) - 1;

      GameObject obj = positions[x, y];
      destroyedPieces.Add(obj);
      string name = obj.name;
      Piece2 p = obj.GetComponent<Piece2>();
      p.DestroyGameObject();
      CalculateLP(obj);
      Empty2DArray(square);
    }

    public void Move(string coords2)
    {
      try
      {
        string[] positions = coords2.Split(',');
      string from = positions[0];
      string to = positions[1];
      GameObject obj = Convert(from);
      Piece2 piece = obj.GetComponent<Piece2>();
      if((piece.getColour().Equals("white") && whiteMove) || ((piece.getColour().Equals("black") && !whiteMove)))
      {
        if(piece.ValidMove(to) == true && IsEnPassant(from, to) == true && isPawn(from))
        {
          EnPassant(from, to);
          lastMoveDestroyed.Add(true);
          piece.Move(to, coords2);
          if(piece.getColour().Equals("white"))
            {
                whiteMove = false;
                blackMove = true;
            }
            else
            {
                whiteMove = true;
                blackMove = false;
            }
          previousMoves.Add(coords2);
          moves++;
        }
        else if(piece.ValidMove(to) == true && isOccupied(to))
        {
          DestroyPiece(to);
          lastMoveDestroyed.Add(true);
          piece.Move(to, coords2);
          if(piece.getColour().Equals("white"))
            {
                whiteMove = false;
                blackMove = true;
            }
            else
            {
                whiteMove = true;
                blackMove = false;
            }
          previousMoves.Add(coords2);
          moves++;
        }
        else if(piece.ValidMove(to) == true)
        {
          lastMoveDestroyed.Add(false);
          piece.Move(to, coords2);
          if(piece.getColour().Equals("white"))
            {
                whiteMove = false;
                blackMove = true;
            }
            else
            {
                whiteMove = true;
                blackMove = false;
            }
          previousMoves.Add(coords2);
          moves++;
        }
        else if(CanCastle(from, to))
        {
          Castle(from, to);
          lastMoveDestroyed.Add(false);
          previousMoves.Add(coords2);
          moves++;
        }
        else
        {

        }

      }
      else
      {

      }

      }
      catch(Exception e)
      {
        Debug.Log(e);
      }
      
      
    }

    public void TakeBackMove(string coords)
    {
      string[] positions = coords.Split(',');
      string from = positions[0];
      string to = positions[1];
      GameObject obj = Convert(from);
      Piece2 piece = obj.GetComponent<Piece2>();
      piece.Move(to, coords);
      if(piece.moves == 2)
      {
        piece.moved = false;
      }
    }

    public GameObject Convert(string coords)
    {
      int x = (int) coords[0] - 97;
      int y = (int) Char.GetNumericValue(coords[1]) - 1;

      return positions[x, y];

    }

    private IEnumerator Wait()
    {
        
        yield return new WaitForSeconds(3);
    }


    public bool CanCastle(string from, string to)
    {
      GameObject obj1 = Convert(from);
      GameObject obj2 = Convert(to);
      Piece2 piece1 = obj1.GetComponent<Piece2>();
      Piece2 piece2 = obj2.GetComponent<Piece2>();

      if(obj1.name.Equals("white_king(Clone)") && obj2.name.Equals("white_rook(Clone)") && piece1.moved == false
      && piece2.moved == false && whiteCastle == true)
      {
        if(to.Equals("a1"))
        {
          if(isOccupied("b1") == false && isOccupied("c1") == false && isOccupied("d1") == false)
          {
            return true;
          }
        }
        if(to.Equals("h1"))
        {
          if(isOccupied("g1") == false && isOccupied("f1") == false)
          {
            return true;
          }
        }
      }
      else if(obj1.name.Equals("black_king(Clone)") && obj2.name.Equals("black_rook(Clone)") && piece1.moved == false
      && piece2.moved == false && blackCastle == true)
      {
        if(to.Equals("h8"))
        {
          if(isOccupied("g8") == false && isOccupied("f8") == false)
          {
            return true;
          }
        }
        if(to.Equals("a8"))
        {
          if(isOccupied("b1") == false && isOccupied("c8") == false && isOccupied("d8") == false)
          {
            return true;
          }
        }
      }
      return false;
    }

    public void Castle(string from, string to)
    {
      GameObject obj1 = Convert(from);
      GameObject obj2 = Convert(to);
      Piece2 piece1 = obj1.GetComponent<Piece2>();
      Piece2 piece2 = obj2.GetComponent<Piece2>();

      if(to.Equals("a1"))
      {
        piece1.Move("d1", "empty");
        piece2.Move("c1", "empty");
      }
      else if(to.Equals("h1"))
      {
        piece1.Move("g1", "empty");
        piece2.Move("f1", "empty");
      }
      else if(to.Equals("a8"))
      {
        piece1.Move("d8", "empty");
        piece2.Move("c8", "empty");
      }
      else if(to.Equals("h8"))
      {
        piece1.Move("g8", "empty");
        piece2.Move("f8", "empty");
      }
    }

    public bool isPawn(string square)
    {
      GameObject obj = Convert(square);

      if(obj.name.Equals("white_pawn(Clone)") || obj.name.Equals("black_pawn(Clone)"))
      {
        return true;
      }
      return false;
    }

    public bool PawnExists()
    {
      foreach(GameObject obj in positions)
      {
        if(obj.name.Equals("white_pawn(Clone)") || obj.name.Equals("black_pawn(Clone)"))
        {
          return true;
        }
      }
      return false;
    }

    public void MovedTwoIsFalse()
    {
        var objects = GameObject.FindGameObjectsWithTag("Piece");
        foreach(var obj in objects)
        {
            Piece2 piece = obj.GetComponent<Piece2>();
            piece.justMovedTwo = false;

        }
    }

    public bool IsEnPassant(string from, string to)
    {
      if(to.Equals(findSquare(from, -1, 1)) && isOccupied(to) == false)
      {
        return true;
      }
      if(to.Equals(findSquare(from, -1, -1)) && isOccupied(to) == false)
      {
        return true;
      }
      if(to.Equals(findSquare(from, 1, 1)) && isOccupied(to) == false)
      {
        return true;
      }
      if(to.Equals(findSquare(from, 1, -1)) && isOccupied(to) == false)
      {
        return true;
      }

      return false;
    }

    public void EnPassant(string from, string to)
    {
      if(to.Equals(findSquare(from, -1, 1)) && isOccupied(to) == false)
      {
        DestroyPiece(findSquare(from, -1, 0));
      }
      if(to.Equals(findSquare(from, -1, -1)) && isOccupied(to) == false)
      {
        DestroyPiece(findSquare(from, -1, 0));
      }
      if(to.Equals(findSquare(from, 1, 1)) && isOccupied(to) == false)
      {
        DestroyPiece(findSquare(from, 1, 0));
      }
      if(to.Equals(findSquare(from, 1, -1)) && isOccupied(to) == false)
      {
        DestroyPiece(findSquare(from, 1, 0));
      }

      EnPassantX();

    }

    public string findSquare(string square, int a, int b)
    {
        int letter = (int) square[0] + a;
        int number = (int)Char.GetNumericValue(square[1]) + b;

        char temp = (char) letter;

        return temp.ToString() + number.ToString();
    }

    public void CreateRandomWhitePiece(string square)
    {
      var rand2 = new System.Random();
      int i = rand2.Next(3);
      if(i == 0)
      {
        Spawn(white_knight, square);
      }
      else if(i == 1)
      {
        Spawn(white_bishop, square);
      }
      else if(i == 2)
      {
        Spawn(white_rook, square);
      }
      else
      {
        Spawn(white_queen, square);
      }

    }

    public void CreateRandomBlackPiece(string square)
    {
      var rand2 = new System.Random();
      int i = rand2.Next(3);
      if(i == 0)
      {
        Spawn(black_knight, square);
      }
      else if(i == 1)
      {
        Spawn(black_bishop, square);
      }
      else if(i == 2)
      {
        Spawn(black_rook, square);
      }
      else
      {
        Spawn(black_queen, square);
      }

    }

    public void CalculateLP(GameObject obj)
    {
      switch(obj.name)
      {
        case "white_pawn(Clone)": whiteLifePoints -= 50; return;
        case "black_pawn(Clone)": blackLifePoints -= 50; return;
        case "white_knight(Clone)": whiteLifePoints -= 150; return;
        case "black_knight(Clone)": blackLifePoints -= 150; return;
        case "white_bishop(Clone)": whiteLifePoints -= 150; return;
        case "black_bishop(Clone)": blackLifePoints -= 150; return;
        case "white_rook(Clone)": whiteLifePoints -= 250; return;
        case "black_rook(Clone)": blackLifePoints -= 250; return;
        case "white_queen(Clone)": whiteLifePoints -= 500; return;
        case "black_queen(Clone)": blackLifePoints -= 500; return;
      }

    }

    public void EnPassantX()
    {

      if(whiteEnPassantX && blackMove)
      {
        whiteLifePoints -= 200;
        whiteEnPassantX = false;
      }
      if(blackEnPassantX && whiteMove)
      {
        blackLifePoints -= 200;
        blackEnPassantX = false;
      }

    }

    public void PromotionX()
    {

      if(whitePromotionX && blackMove)
      {
        whiteLifePoints -= 200;
        whitePromotionX = false;
      }
      if(blackPromotionX && whiteMove)
      {
        blackLifePoints -= 200;
        blackPromotionX = false;
      }
      
    }

    public void GetRandomAIMove()
    {
      int probs = rndAI.Next(0, 1000);

      if(probs < 2 && computer.cardsUsed < 3)
      {
        if((P1 && blackMove) || (P2 && whiteMove))
        {
          computer.GenerateRandomCardMove();
          StartCoroutine(Wait());
        }
      }

      if(P1)
      {
        if(blackMove)
        {
          String s = computer.GenerateRandomMove();
          Move(s);
        }
      }
      else
      {
        if(whiteMove)
        {
          String s = computer.GenerateRandomMove();
          Move(s);
        }
      }

    }

    public void GetDepth1AIMove()
    {
      if((P1 && blackMove) || (P2 && whiteMove))
      {
        string s = computer.BestMoveDepth1();
        Move(s);
      }

    }

    public int PseudoMove(string coords2)
    {
      string[] positions = coords2.Split(',');
      string from = positions[0];
      string to = positions[1];
      GameObject obj = Convert(from);
      Piece2 piece = obj.GetComponent<Piece2>();
      if(piece.ValidMove(to) == true && IsEnPassant(from, to) == true && isPawn(from))
      {
        return 50;
      }
      else if(piece.ValidMove(to) == true && isOccupied(to))
      {
        GameObject obj2 = Convert(to);
        return PseudoCalculateLP(obj2);
      }
      else 
      {
        return 0;
      }
          
    }

    public int PseudoCalculateLP(GameObject obj)
    {
      switch(obj.name)
      {
        case "white_pawn(Clone)": return 50;
        case "black_pawn(Clone)": return 50;
        case "white_knight(Clone)": return 150;
        case "black_knight(Clone)": return 150;
        case "white_bishop(Clone)": return 150;
        case "black_bishop(Clone)": return 150;
        case "white_rook(Clone)": return 250;
        case "black_rook(Clone)": return 250;
        case "white_queen(Clone)": return 500;
        case "black_queen(Clone)": return 500;
        case "white_king(Clone)": return 1000;
        case "black_king(Clone)": return 1000;

      }

      return 0;
    }




    

      

    


}






