using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Game : MonoBehaviour
{
  public bool whiteQ = false, blackQ = false;
    public GameObject waiting;
    public static bool play = false;
    public Text textField, whiteLP, blackLP;
    public GameObject white_king, white_queen, white_rook, white_bishop, white_knight, white_pawn;
    public GameObject black_king, black_queen, black_rook, black_bishop, black_knight, black_pawn;
    public GameObject white_skip_turn_card, black_skip_turn_card, white_castle_card, black_castle_card,
    white_take_back_card, black_take_back_card,
    white_en_passant_trap_card, black_en_passant_trap_card, white_promotion_trap_card, black_promotion_trap_card, 
    
    white_take_back_x3_card, black_take_back_x3_card, white_queen_card, black_queen_card,
    card_back;
    private GameObject[] black = new GameObject[16];
    private GameObject[] white = new GameObject[16];
    public GameObject[,] positions = new GameObject[8, 8];
    public static bool whiteMove;
    public static bool blackMove;
    public static string winner;
    public static bool blackCastle;
    public static bool whiteCastle;
    public static List<bool> lastMoveDestroyed;
    public AudioSource btnClick;
    public static List<string> previousMoves;
    public static List<GameObject> destroyedPieces;
    public static string coords;
    public static bool destroyPawn;

    public static int moves;

    public static int cardsUsed;
    public static string cardJustUsed;
    public List<GameObject> enemyCards = new List<GameObject>();

    public static bool whiteEnPassantX, blackEnPassantX, whitePromotionX, blackPromotionX;
    public bool P1 = false, P2 = false;
    public string[] squareNames = {"a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8",
    "b1", "b2", "b3", "b4", "b5", "b6", "b7", "b8",
    "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8",
    "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8",
    "e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8",
    "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8",
    "g1", "g2", "g3", "g4", "g5", "g6", "g7", "g8",
    "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8"};

    public static Dictionary<string, Vector2> squares;
    public static Dictionary<Vector2, string> square_references;
    public static Dictionary<string, Vector2> cards;
    public static Dictionary<Vector2, string> card_references;
    private float x = -3.94f;
    private float y = -3.92f;
    public static int whiteLifePoints;
    public static int blackLifePoints;

    public void Awake()
    {
      MainMenu.colour = "white";
      blackCastle = false;
      whiteCastle = false;
      lastMoveDestroyed = new List<bool>();
      previousMoves = new List<string>();
      destroyedPieces = new List<GameObject>();
      coords = "";
      destroyPawn = false;
      whiteEnPassantX = false; blackEnPassantX = false; whitePromotionX = false; blackPromotionX = false;
      whiteLifePoints = 1000;
      blackLifePoints = 1000;
      card_references = new Dictionary<Vector2, string>();
      cards = new Dictionary<string, Vector2>();
      square_references = new Dictionary<Vector2, string>();
      squares = new Dictionary<string, Vector2>();
    }
    public void SetText(string text)
    {
      textField.text = text;
    }

    public void SetWhiteLP(int lp)
    {
      whiteLP.text = "LP: " + lp.ToString();
    }

    public void SetBlackLP(int lp)
    {
      blackLP.text = "LP: " + lp.ToString();
    }


    public void Start()
    {
      waiting = GameObject.FindGameObjectWithTag("Waiting");
      var rand = new System.Random();
      int Index1 = rand.Next(5);
      int Index2 = rand.Next(5);
      int Index3 = rand.Next(5);
      int Index4 = rand.Next(5);
      int Index5 = rand.Next(5);
      int Index6 = rand.Next(5);

      for(int i = 0; i < 8; i++)
      {
        for(int j = 0; j < 8; j++)
        {
          positions[i, j] = null;
        }
      }

      if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
      {
        MainMenu.colour = "black";
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
          CreateWhiteCard(-8.1f, -1.8f, Index1);
          CreateWhiteCard(-6.8f, -1.8f, Index2);
          CreateWhiteCard(-5.5f, -1.8f, Index3);
          CreateCardBack(-8.1f, 1.8f);
          CreateCardBack(-6.8f, 1.8f);
          CreateCardBack(-5.5f, 1.8f);
          if(PhotonNetwork.IsConnected)
          {
            this.P2 = false;
          }
        }
        else
        {
          CreateCardBack(-8.1f, 1.8f);
          CreateCardBack(-6.8f, 1.8f);
          CreateCardBack(-5.5f, 1.8f);
          CreateBlackCard(-8.1f, -1.8f, Index4);
          CreateBlackCard(-6.8f, -1.8f, Index5);
          CreateBlackCard(-5.5f, -1.8f, Index6);
          Vector3 temp = whiteLP.transform.position;
          whiteLP.transform.position = blackLP.transform.position;
          blackLP.transform.position = temp;
          this.P1 = false;
        }
       
    }

    void Update()
    {
      if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
      {
        waiting.SetActive(false);
        play = true;
      }
      SetWhiteLP(whiteLifePoints);
      SetBlackLP(blackLifePoints);

      if(whiteLifePoints <= 0)
      {
        GameOver("BLACK");
      }

      if(blackLifePoints <= 0)
      {
        GameOver("WHITE");
      }
    }

    public GameObject Spawn(GameObject name, string square)
      {
        GameObject obj = Instantiate(name, makeVector(squares[square]), Quaternion.identity);
        Piece piece = obj.GetComponent<Piece>(); 
        Add2DArray(square, obj);
        return obj;
      }

    public static Vector3 makeVector(Vector2 v)
    {
      return new Vector3(v.x, v.y, -2);
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
            Piece piece = obj.GetComponent<Piece>();
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
      Debug.Log("Destroyed");
      int x = (int) square[0] - 97;
      int y = (int) Char.GetNumericValue(square[1]) - 1;

      GameObject obj = positions[x, y];
      destroyedPieces.Add(obj);
      string name = obj.name;
      Piece p = obj.GetComponent<Piece>();
      p.DestroyGameObject();
      CalculateLP(obj);
      Empty2DArray(square);
      if(name.Equals("white_king(Clone)"))
      {
        GameOver("BLACK");
      }
      else if(name.Equals("black_king(Clone)"))
      {
        GameOver("WHITE");
      }
  

    }

    public void SafeDestroyPiece(string square)
    {
      Debug.Log("Destroyed");
      int x = (int) square[0] - 97;
      int y = (int) Char.GetNumericValue(square[1]) - 1;

      GameObject obj = positions[x, y];
      destroyedPieces.Add(obj);
      string name = obj.name;
      Piece p = obj.GetComponent<Piece>();
      p.DestroyGameObject();
      Empty2DArray(square);
      if(name.Equals("white_king(Clone)"))
      {
        GameOver("BLACK");
      }
      else if(name.Equals("black_king(Clone)"))
      {
        GameOver("WHITE");
      }
  

    }

    public void Move(string coords2)
    {
      Debug.Log(lastMoveDestroyed.Count);
      string[] positions = coords2.Split(',');
      string from = positions[0];
      string to = positions[1];
      GameObject obj = Convert(from);
      Piece piece = obj.GetComponent<Piece>();
      if((piece.getColour().Equals("white") && whiteMove) || ((piece.getColour().Equals("black") && !whiteMove)))
      {
        if(piece.ValidMove(to) == true && IsEnPassant(from, to) == true && isPawn(from))
        {
          EnPassant(from, to);
          lastMoveDestroyed.Add(true);
          piece.Move(to, coords2);
          SetText("");
          if(piece.getColour().Equals("white"))
            {
                Game.whiteMove = false;
                Game.blackMove = true;
            }
            else
            {
                Game.whiteMove = true;
                Game.blackMove = false;
            }
          previousMoves.Add(coords2);
          moves++;
        }
        else if(piece.ValidMove(to) == true && isOccupied(to))
        {
          Debug.Log("Moved To Occupied Square");
          DestroyPiece(to);
          lastMoveDestroyed.Add(true);
          piece.Move(to, coords2);
          SetText("");
          if(piece.getColour().Equals("white"))
            {
                Game.whiteMove = false;
                Game.blackMove = true;
            }
            else
            {
                Game.whiteMove = true;
                Game.blackMove = false;
            }
          previousMoves.Add(coords2);
          moves++;
        }
        else if(piece.ValidMove(to) == true)
        {
          Debug.Log("Moved To Empty Square");
          lastMoveDestroyed.Add(false);
          piece.Move(to, coords2);
          SetText("");
          if(piece.getColour().Equals("white"))
            {
                Game.whiteMove = false;
                Game.blackMove = true;
            }
            else
            {
                Game.whiteMove = true;
                Game.blackMove = false;
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

    public void TakeBackMove(string coords)
    {
      string[] positions = coords.Split(',');
      string from = positions[0];
      string to = positions[1];
      GameObject obj = Convert(from);
      Piece piece = obj.GetComponent<Piece>();
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

    public void GameOver(string colour)
    {
      SceneManager.LoadScene("GameOver");
      winner = colour;
    }

    private IEnumerator Wait()
    {
        
        yield return new WaitForSeconds(3);
    }

    public void BackButton()
    {
        btnClick.Play();
        StartCoroutine(Wait());
        Reset();
        SceneManager.LoadScene("ModeScreenBlackOrWhite");
    }

    public void Reset()
    {
      squares.Clear();
      square_references.Clear();
      previousMoves.Clear();
      destroyedPieces.Clear();
    }

    public bool CanCastle(string from, string to)
    {
      GameObject obj1 = Convert(from);
      GameObject obj2 = Convert(to);
      Piece piece1 = obj1.GetComponent<Piece>();
      Piece piece2 = obj2.GetComponent<Piece>();

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
      Piece piece1 = obj1.GetComponent<Piece>();
      Piece piece2 = obj2.GetComponent<Piece>();

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

    public GameObject CreateWhiteCard(float x, float y, int i)
    {
      GameObject[] cards = {white_castle_card, white_skip_turn_card,
      white_en_passant_trap_card, white_promotion_trap_card, white_queen_card};

      GameObject obj = Instantiate(cards[i], new Vector3 (x, y, 0), Quaternion.identity);

      return obj;
    }

    public GameObject CreateBlackCard(float x, float y, int i)
    {
      GameObject[] cards = {black_castle_card, black_skip_turn_card,
      black_en_passant_trap_card, black_promotion_trap_card, black_queen_card};

      GameObject obj = Instantiate(cards[i], new Vector3 (x, y, 0), Quaternion.identity);

      return obj;
    }

    public GameObject CreateCardBack(float x, float y)
    {
      GameObject obj = Instantiate(card_back, new Vector3 (x, y, 0), Quaternion.identity);

      return obj;
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
            Piece piece = obj.GetComponent<Piece>();
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
      if(whiteMove)
      {
        whiteEnPassantX = false;
      }
      else
      {
        blackEnPassantX = false;
      }

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
      int i = rand2.Next(2);
      if(i == 0)
      {
        Spawn(white_knight, square);
      }
      else if(i == 1)
      {
        Spawn(white_bishop, square);
      }
      else
      {
        Spawn(white_rook, square);
      }

    }

    public void CreateWhiteQueen(string square)
    {
      Spawn(white_queen, square);
    }

    public void CreateRandomBlackPiece(string square)
    {
      var rand2 = new System.Random();
      int i = rand2.Next(2);
      if(i == 0)
      {
        Spawn(black_knight, square);
      }
      else if(i == 1)
      {
        Spawn(black_bishop, square);
      }
      else
      {
        Spawn(black_rook, square);
      }

    }

    public void CreateBlackQueen(string square)
    {
      Spawn(black_queen, square);
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
        case "black_queen 1(Clone)": blackLifePoints -= 500; return;
      }

      if(whiteLifePoints <= 0)
      {
        GameOver("BLACK");
      }

      if(blackLifePoints <= 0)
      {
        GameOver("WHITE");
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

    public void MirrorEffect(string s)
    {
      GameObject gameObject1 = card_back;

      switch(s)
      {
        case "white_skip_turn_card(Clone)": gameObject1 = white_skip_turn_card; break;
        case "white_castle_card(Clone)": gameObject1 = white_castle_card; break;
        case "black_take_back_card(Clone)": gameObject1 = black_take_back_card; break;
        case "white_en_passant_trap_card(Clone)": gameObject1 = white_en_passant_trap_card; break;
        case "white_promotion_trap_card(Clone)": gameObject1 = white_promotion_trap_card; break;
        case "black_skip_turn_card(Clone)": gameObject1 = black_skip_turn_card; break;
        case "black_castle_card(Clone)": gameObject1 = black_castle_card; break;
        case "white_take_back_card(Clone)": gameObject1 = white_take_back_card; break;
        case "black_en_passant_trap_card(Clone)": gameObject1 = black_en_passant_trap_card; break;
        case "black_promotion_trap_card(Clone)": gameObject1 = black_promotion_trap_card; break;
        case "white_take_back_x3_card(Clone)": gameObject1 = white_take_back_x3_card; break;
        case "black_take_back_x3_card(Clone)": gameObject1 = black_take_back_x3_card; break;
        case "white_queen_card(Clone)": gameObject1 = white_queen_card; break;
        case "black_queen_card(Clone)": gameObject1 = black_queen_card; break;

      }

      if(cardsUsed == 0)
      {
        GameObject obj = MakeMirrorCard(gameObject1, -8.1f, 1.8f);
        Card card = obj.GetComponent<Card>();
        card.CardMirrorEffect();

      }
      else if(cardsUsed == 1)
      {
        GameObject obj = MakeMirrorCard(gameObject1, -6.8f, 1.8f);
        Card card = obj.GetComponent<Card>();
        card.CardMirrorEffect();
      }
      else
      {
        GameObject obj = MakeMirrorCard(gameObject1, -5.5f, 1.8f);
        Card card = obj.GetComponent<Card>();
        card.CardMirrorEffect();
      }

      cardsUsed++;
    }

    public GameObject MakeMirrorCard(GameObject name, float x, float y)
      {
        GameObject obj = Instantiate(name, new Vector3(x, y, -3), Quaternion.identity);
        Card card = obj.GetComponent<Card>(); 
        enemyCards.Add(obj);
        return obj;
      }

    public bool IsEmpty(string coords)
    {
      int x = (int) coords[0] - 97;
      int y = (int) Char.GetNumericValue(coords[1]) - 1;

      if(positions[x, y] == null)
      {
        return true;
      }

      return false;

    }

    

      

    


}






