using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class Piece : MonoBehaviour
{
    public GameObject moveSquare;
    public GameObject controller;
    private bool isDragging;
    Vector2 startPosition;
    public Game game;
    string square;
    public bool moved = false;
    public bool justMovedTwo = false;
    public int moves = 0;

    void Start()
    {
        startPosition = transform.position;
        this.square = getSquare();
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game>();
    }

   void Update()
    {
        if(PawnAtBackRank())
        {
            game.PromotionX();
            if(Game.whiteMove)
            {
                Game.whitePromotionX = false;
            }
            else
            {
                Game.blackPromotionX = false;
            }
            if(getColour() == "white")
            {
                game.SafeDestroyPiece(getSquare());
                if(game.whiteQ == false)
                {
                    game.CreateRandomWhitePiece(getSquare());
                }
                else
                {
                    game.CreateWhiteQueen(getSquare());
                    game.whiteQ = false;
                }
            }
            else
            {
                game.SafeDestroyPiece(getSquare());
                if(game.blackQ == false)
                {
                    game.CreateRandomBlackPiece(getSquare());
                }
                else
                {
                    game.CreateBlackQueen(getSquare());
                    game.blackQ = false;
                }
            }
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
        Debug.Log("Destroyed");
    }
    private void OnMouseDown()
    {
        if((Game.play) && (getColour().Equals("white") && Game.whiteMove) && game.P1 == true || ((getColour().Equals("black") && !Game.whiteMove && game.P2 == true)))
        {
            if(Game.coords.Length == 0)
            {
                Game.coords = getSquare() + ",";
            }
            else
            {
                Game.coords += getSquare();
                game.Move(Game.coords);
                Game.coords = "";
                DestroyMoveSquares();
            }
        }

        if(game.isPawn(getSquare()) && Game.destroyPawn == true)
        {
            game.DestroyPiece(getSquare());
            Game.destroyPawn = false;
        }
    }

    private void OnMouseUp()
    {
        if((Game.play) && (getColour().Equals("white") && Game.whiteMove) && game.P1 == true || ((getColour().Equals("black") && !Game.whiteMove && game.P2 == true)))
        {
            DestroyMoveSquares();
            LegalMoves(MoveSquarePositions());
        }

        
    }

    public string getColour()
    {
        
        switch (this.name)
        {
            case "black_queen 1(Clone)": return "black";
            case "black_knight(Clone)": return "black"; 
            case "black_bishop(Clone)": return "black"; 
            case "black_king(Clone)": return "black"; 
            case "black_rook(Clone)": return "black"; 
            case "black_pawn(Clone)": return "black"; 
        }

        return "white";

    }

    public string getSquare()
    {
        string myKey = Game.square_references[transform.position];
        return myKey;           
    }
    //This method and some others were loosely inspired by https://github.com/etredal/Chess_App
    public void DestroyMoveSquares()
    {

        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MoveSquare");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); 
        }
    }

    //This method and some others were loosely inspired by https://github.com/etredal/Chess_App
    public void CreateMoveSquare(string square)
    {
        GameObject ms = Instantiate(moveSquare, Game.makeVector(Game.squares[square]), Quaternion.identity);
    }

    public void LegalMoves(List<string> lst)
    {
        foreach(string s in lst)
        {
            CreateMoveSquare(s);
        }
    }

    public void CreateAttackSquare(string square)
    {
        
    }
    //This method and some others were loosely inspired by https://github.com/etredal/Chess_App
    public List<string> MoveSquarePositions()
    {
        
        switch (this.name)
        {
            case "black_pawn(Clone)": return BlackPawnMove(); 
            case "white_pawn(Clone)": return WhitePawnMove();  
            case "black_knight(Clone)": return KnightMove(); 
            case "white_knight(Clone)": return KnightMove(); 
            case "black_bishop(Clone)": return BishopMove(); 
            case "white_bishop(Clone)": return BishopMove(); 
            case "black_rook(Clone)": return RookMove(); 
            case "white_rook(Clone)": return RookMove(); 
            case "black_queen 1(Clone)": return QueenMove(); 
            case "white_queen(Clone)": return QueenMove(); 
            case "black_king(Clone)": return KingMove(); 
            case "white_king(Clone)": return KingMove(); 
        }

        return new List<string>();

        
    }

    public List<string> WhitePawnMove()
    {
        Game sc = controller.GetComponent<Game>();
        List<string> moveTo = new List<string>();
        if(this.moved)
        {
            if(sc.isOccupied(findSquare(getSquare(), 0, 1)) == false)
            {
                moveTo.Add(findSquare(getSquare(), 0, 1));
            }
        }
        else if(sc.isOccupied(findSquare(getSquare(), 0, 1)) == false)
        {
            moveTo.Add(findSquare(getSquare(), 0, 1));
            if(sc.isOccupied(findSquare(getSquare(), 0, 2)) == false)
            {
                moveTo.Add(findSquare(getSquare(), 0, 2));
            }
        }

        if(sc.isOccupied(findSquare(getSquare(), 1, 1)) && isEnemy(findSquare(getSquare(), 1, 1)))
        {
            moveTo.Add(findSquare(getSquare(), 1, 1));
        }
        if(sc.isOccupied(findSquare(getSquare(), -1, 1)) && isEnemy(findSquare(getSquare(), -1, 1)))
        {
            moveTo.Add(findSquare(getSquare(), -1, 1));
        }

        string westSquare = findSquare(getSquare(), -1, 0);
        string eastSquare = findSquare(getSquare(), 1, 0);

        if(sc.isOccupied(westSquare))
        {
            Debug.Log("Occupied");
            GameObject obj1 = game.Convert(westSquare);
            Piece p1 = obj1.GetComponent<Piece>();
            if(p1.justMovedTwo == true && isEnemy(westSquare))
            {
                Debug.Log("justmovedtwo");
                moveTo.Add(findSquare(getSquare(), -1, 1));
            }
        }

        if(sc.isOccupied(eastSquare) && isEnemy(eastSquare))
        {
            Debug.Log("Occupied");
            GameObject obj1 = game.Convert(eastSquare);
            Piece p1 = obj1.GetComponent<Piece>();
            if(p1.justMovedTwo == true)
            {
                Debug.Log("justmovedtwo");
                moveTo.Add(findSquare(getSquare(), 1, 1));
            }
        }

        return moveTo;

    }

    public List<string> BlackPawnMove()
    {
        Game sc = controller.GetComponent<Game>();
        List<string> moveTo = new List<string>();
        if(this.moved)
        {
            if(sc.isOccupied(findSquare(getSquare(), 0, -1)) == false)
            {
                moveTo.Add(findSquare(getSquare(), 0, -1));
            }
        }
        else if(sc.isOccupied(findSquare(getSquare(), 0, -1)) == false)
        {
            moveTo.Add(findSquare(getSquare(), 0, -1));
            if(sc.isOccupied(findSquare(getSquare(), 0, -2)) == false)
            {
                moveTo.Add(findSquare(getSquare(), 0, -2));
            }
        }
        if(sc.isOccupied(findSquare(getSquare(), -1, -1)) && isEnemy(findSquare(getSquare(), -1, -1)))
        {
            moveTo.Add(findSquare(getSquare(), -1, -1));
        }
        if(sc.isOccupied(findSquare(getSquare(), 1, -1)) && isEnemy(findSquare(getSquare(), 1, -1)))
        {
            moveTo.Add(findSquare(getSquare(), 1, -1));
        }

        string westSquare = findSquare(getSquare(), -1, 0);
        Debug.Log("westSquare");
        string eastSquare = findSquare(getSquare(), 1, 0);

        if(sc.isOccupied(westSquare))
        {
            Debug.Log("Occupied");
            GameObject obj1 = game.Convert(westSquare);
            Piece p1 = obj1.GetComponent<Piece>();
            if(p1.justMovedTwo == true)
            {
                moveTo.Add(findSquare(getSquare(), -1, -1));
            }
        }

        if(sc.isOccupied(eastSquare))
        {
            Debug.Log("Occupied");
            GameObject obj1 = game.Convert(eastSquare);
            Piece p1 = obj1.GetComponent<Piece>();
            if(p1.justMovedTwo == true)
            {
                moveTo.Add(findSquare(getSquare(), 1, -1));
            }
        }

        return moveTo;
        
    }

    

    public List<string> KnightMove()
    {
        List<string> l1 = PointMoveSquare(1, 2);
        List<string> l2 = PointMoveSquare(2, 1);
        List<string> l3 = PointMoveSquare(-1, -2);
        List<string> l4 = PointMoveSquare(-2 , -1);
        List<string> l5 = PointMoveSquare(-1, 2);
        List<string> l6 = PointMoveSquare(1, -2);
        List<string> l7 = PointMoveSquare(-2, 1);
        List<string> l8 = PointMoveSquare(2, -1);

        return l1.Concat(l2).Concat(l3).Concat(l4).Concat(l5).Concat(l6).Concat(l7).Concat(l8).ToList();


    }

    public List<string> BishopMove()
    {
        List<string> l1 = LineMoveSquare(1, 1);
        List<string> l2 = LineMoveSquare(1, -1);
        List<string> l3 = LineMoveSquare(-1, 1);
        List<string> l4 = LineMoveSquare(-1, -1);

        List<string> check = l1.Concat(l2).Concat(l3).Concat(l4).ToList();

        foreach(string s in check)
        {
            Debug.Log(s);
        }

        return l1.Concat(l2).Concat(l3).Concat(l4).ToList();
    }

    public List<string> RookMove()
    {
        List<string> l1 = LineMoveSquare(1, 0);
        List<string> l2 = LineMoveSquare(0, -1);
        List<string> l3 = LineMoveSquare(0, 1);
        List<string> l4 = LineMoveSquare(-1, 0);

        return l1.Concat(l2).Concat(l3).Concat(l4).ToList();

    }

    public List<string> KingMove()
    {
        List<string> l1 = PointMoveSquare(0, 1);
        List<string> l2 = PointMoveSquare(1, 0);
        List<string> l3 = PointMoveSquare(-1, 0);
        List<string> l4 = PointMoveSquare(0, -1);
        List<string> l5 = PointMoveSquare(1, 1);
        List<string> l6 = PointMoveSquare(-1, -1);
        List<string> l7 = PointMoveSquare(1, -1);
        List<string> l8 = PointMoveSquare(-1, 1);
        List<string> l9 = CastleSquares();

        return l1.Concat(l2).Concat(l3).Concat(l4).Concat(l5).Concat(l6).Concat(l7).Concat(l8).Concat(l9).ToList();

    }

    public List<string> QueenMove()
    {
        List<string> l1 = BishopMove();
        List<string> l2 = RookMove();

        return l1.Concat(l2).ToList();
    }

    public string findSquare(string square, int a, int b)
    {
        int letter = (int) square[0] + a;
        int number = (int)Char.GetNumericValue(square[1]) + b;

        char temp = (char) letter;

        return temp.ToString() + number.ToString();
    }
    //This method and some others were loosely inspired by https://github.com/etredal/Chess_App
    public List<string> LineMoveSquare(int x, int y)
    {
        List<string> moveTo = new List<string>();
        Game sc = controller.GetComponent<Game>();
        
        int xIncrement = x;
        int yIncrement = y;


        while (sc.isValidSquare(findSquare(getSquare(), x, y)) && sc.isOccupied(findSquare(getSquare(), x, y)) == false)
        {
            moveTo.Add(findSquare(getSquare(), x, y));
            x += xIncrement;
            y += yIncrement;
            
        }
        if (sc.isOccupied(findSquare(getSquare(), x, y)) && isEnemy(findSquare(getSquare(), x, y)))
        {
            moveTo.Add(findSquare(getSquare(), x, y));

        }

        return moveTo;

    }
    //This method and some others were loosely inspired by https://github.com/etredal/Chess_App
    public List<string> PointMoveSquare(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        List<string> moveTo = new List<string>();

        if(sc.isValidSquare(findSquare(getSquare(), x, y)))
        {
            if(game.isOccupied(findSquare(getSquare(), x, y)) == false)
            {
                moveTo.Add(findSquare(getSquare(), x, y));
            }
            else if(isEnemy(findSquare(getSquare(), x, y)) == true)
            {
                moveTo.Add(findSquare(getSquare(), x, y));
            }
        }

        return moveTo;


    }

    public List<string> CastleSquares()
    {

        List<string> moveTo = new List<string>();

        if(this.moved == false && getColour() == "white" && Game.whiteCastle)
        {
            if(game.positions[0,0] != null && game.IsEmpty("b1") && game.IsEmpty("c1") && game.IsEmpty("d1"))
            {
                GameObject obj = game.Convert("a1");
                Piece p = obj.GetComponent<Piece>();
                if(p.moved == false)
                {
                    moveTo.Add("a1");
                }

            }
            
            if(game.positions[7,0] != null && game.IsEmpty("f1") && game.IsEmpty("g1"))
            {
                GameObject obj = game.Convert("h1");
                Piece p = obj.GetComponent<Piece>();
                if(p.moved == false)
                {
                    moveTo.Add("h1");
                }
            }
        }

        if(this.moved == false && getColour() == "black" && Game.blackCastle)
        {
            if(game.positions[0,7] != null && game.IsEmpty("b8") && game.IsEmpty("c8") && game.IsEmpty("d8"))
            {
                GameObject obj = game.Convert("a8");
                Piece p = obj.GetComponent<Piece>();
                if(p.moved == false)
                {
                    moveTo.Add("a8");
                }

            }
            
            if(game.positions[7,7] != null && game.IsEmpty("f8") && game.IsEmpty("g8"))
            {
                GameObject obj = game.Convert("h1");
                Piece p = obj.GetComponent<Piece>();
                if(p.moved == false)
                {
                    moveTo.Add("h8");
                }
            }
        }

        return moveTo;

    }

    public void Move(string s, string coords)
    {
      game.MovedTwoIsFalse();
      Debug.Log("Moving");
      this.moved = true;
      Game sc = controller.GetComponent<Game>();
      sc.Empty2DArray(this.square);
      this.transform.position = Game.squares[s];
      this.square = Game.square_references[this.transform.position];
      sc.Add2DArray(this.square, gameObject);
      CheckIfMovedTwo(coords);
      moves++;
    }

    public bool isEnemy(string square)
    {
      Game sc = controller.GetComponent<Game>();
      int x = (int) square[0] - 97;
      int y = (int) Char.GetNumericValue(square[1]) - 1;
      GameObject obj = sc.positions[x, y];
      Piece p2 = obj.GetComponent<Piece>();
      if(getColour().Equals(p2.getColour()))
      {
          return false;
      }
      return true;
    }

    public bool ValidMove(string s)
    {
        List<string> valid = MoveSquarePositions();
        if(valid.Contains(s))
        {
            return true;
        }
        return false;
    }

    public void CheckIfMovedTwo(string coords)
    {
        switch(this.name)
        {
            case "black_pawn(Clone)": 
            case "white_pawn(Clone)":
            if(MinusTwoSquares(coords) == 2)
            {
                justMovedTwo = true;
                return;
            }
            else
            {
                justMovedTwo = false;
                return;
            }
        }
    }

    public double MinusTwoSquares(string squares)
    {
        char c1 = squares[1];
        char c2 = squares[4];

        double d1 = Char.GetNumericValue(c1);
        double d2 = Char.GetNumericValue(c2);

        return Math.Abs(d2 - d1);
    }

    public bool PawnAtBackRank()
    {
        switch(this.name)
        {
            case "black_pawn(Clone)": 
            case "white_pawn(Clone)":
            if(AtBackRank(getSquare()))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public bool AtBackRank(string square)
    {
        char num = square[1];
        if(num.Equals('1')||num.Equals('8'))
        {
            return true;
        }
        return false;
    }




}
