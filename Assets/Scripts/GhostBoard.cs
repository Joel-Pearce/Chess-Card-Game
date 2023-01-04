using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GhostBoard : MonoBehaviour
{
    public bool whiteTurn;
    GameObject controller;
    Game2 game;

    string movedTwo;

    public GameObject[,] board = new GameObject[8,8];
    public GameObject[,] tempBoard = new GameObject[8,8];

    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game2>();
    }


    public void InitialiseBoard()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                this.board[i,j] = game.positions[i,j];
            }         
        }

    }

    public GameObject[,] SaveBoard(GameObject[,] bboard)
    {
        GameObject[,] ret = new GameObject[8,8];

        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                ret[i,j] = bboard[i,j];
            }         
        }

        return ret;

    }

    public void TempBoard()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                this.board[i,j] =  this.tempBoard[i,j];
            }         
        }

    }

    public void InitialiseTempBoard()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                this.tempBoard[i,j] = this.board[i,j];
            }         
        }

    }

    


    public GameObject Access2DArray(string coords)
    {
        int x = (int) coords[0] - 97;
        int y = (int) Char.GetNumericValue(coords[1]) - 1;

        return this.board[x,y];
    }

    public void EmptyBoard(string coords)
    {
        int x = (int) coords[0] - 97;
        int y = (int) Char.GetNumericValue(coords[1]) - 1;

        this.board[x,y] = null;
    }

    public void EmptyBoard(string coords, GameObject[,] bboard)
    {
        int x = (int) coords[0] - 97;
        int y = (int) Char.GetNumericValue(coords[1]) - 1;

        bboard[x,y] = null;
    }

    public void AddBoard(string coords, GameObject obj)
    {
        int x = (int) coords[0] - 97;
        int y = (int) Char.GetNumericValue(coords[1]) - 1;

        this.board[x,y] = obj;
    }

    public void AddBoard(string coords, GameObject obj, GameObject[,] bboard)
    {
        int x = (int) coords[0] - 97;
        int y = (int) Char.GetNumericValue(coords[1]) - 1;

        bboard[x,y] = obj;
    }

    public string getSquare(int x, int y)
    {
        Dictionary<int, string> dic = new Dictionary<int, string>();

        dic.Add(0, "a");
        dic.Add(1, "b");
        dic.Add(2, "c");
        dic.Add(3, "d");
        dic.Add(4, "e");
        dic.Add(5, "f");
        dic.Add(6, "g");
        dic.Add(7, "h");

        return dic[x] + (y + 1).ToString();

    }

    public string findSquare(string square, int a, int b)
    {
        int letter = (int) square[0] + a;
        int number = (int)Char.GetNumericValue(square[1]) + b;

        char temp = (char) letter;

        return temp.ToString() + number.ToString();
    }


    public int Move(string coords, string c)
    {
        string[] arr = coords.Split(',');
        string from = arr[0];
        string to = arr[1];
        int LP = 0;

        GameObject obj = Access2DArray(from);

        if(CheckIfMovedTwo(coords))
        {
            movedTwo = to;
        }
        else
        {
            movedTwo = "";
        }

        if(isEnemy(from, to))
        {
            LP = CalculateLP(c, to);
        }

        AddBoard(to,obj);

        EmptyBoard(from);

        ChangeTurns();


        return LP;

    }

    public GameObject[,] Move(string coords, GameObject[,] bboard)
    {
        GameObject[,] newBoard = new GameObject[8,8];
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                newBoard[i,j] =  bboard[i,j];
            }         
        }
        string[] arr = coords.Split(',');
        string from = arr[0];
        string to = arr[1];

        GameObject obj = Access2DArray(from);

        AddBoard(to,obj,newBoard);

        EmptyBoard(from,newBoard);

        ChangeTurns();

        return newBoard;

    }

    public bool isValidSquare(string square)
    {

      if(game.squareNames.Contains(square))
      {
        return true;
      }
      return false;
    }

    public bool isOccupied(string square)
    {
        if(isValidSquare(square) == false)
        {
            return false;
        }
        if(Access2DArray(square) == null)
        {
            return false;
        }
        return true;
    }

    public bool isEnemy(string from, string to)
    {
        if(isValidSquare(from) == false || isValidSquare(to) == false)
        {
            return false;
        }
        if(Access2DArray(from) != null && Access2DArray(to) != null)
        {
            if(!getColour(Access2DArray(from)).Equals(getColour(Access2DArray(to))))
            {
                return true;
            }
        }
        return false;

    }

    public string getColour(GameObject obj)
    {
        
        switch (obj.name)
        {
            case "black_queen(Clone)": return "black";
            case "black_knight(Clone)": return "black"; 
            case "black_bishop(Clone)": return "black"; 
            case "black_king(Clone)": return "black"; 
            case "black_rook(Clone)": return "black"; 
            case "black_pawn(Clone)": return "black"; 
        }

        return "white";

    }

    public bool PawnHasMoved(int y, GameObject obj)
    {
        if(getColour(obj).Equals("white") && y !=1 )
        {
            return true;
        }
        
        if(getColour(obj).Equals("black") && y !=6 )
        {
            return true;
        }

        return false;
        
    }

    public List<string> LineMoveSquare(int a, int b, int x, int y)
    {
        List<string> moveTo = new List<string>();
        
        int xIncrement = x;
        int yIncrement = y;


        while (isValidSquare(findSquare(getSquare(a, b), x, y)) && isOccupied(findSquare(getSquare(a, b), x, y)) == false)
        {
            moveTo.Add(findSquare(getSquare(a, b), x, y));
            x += xIncrement;
            y += yIncrement;
            
        }
        if (isValidSquare(findSquare(getSquare(a, b), x, y)) && isOccupied(findSquare(getSquare(a, b), x, y)) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), x, y)))
        {
            moveTo.Add(findSquare(getSquare(a, b), x, y));

        }

        moveTo = FilterList(moveTo);

        return moveTo;

    }

    public List<string> PointMoveSquare(int a, int b, int x, int y)
    {
        List<string> moveTo = new List<string>();

        if(isValidSquare(findSquare(getSquare(a, b), x, y)))
        {
            if(isOccupied(findSquare(getSquare(a, b), x, y)) == false)
            {
                moveTo.Add(findSquare(getSquare(a, b), x, y));
            }
            else if(isOccupied(findSquare(getSquare(a, b), x, y)) == true)
            {
                if(isEnemy(getSquare(a, b), findSquare(getSquare(a, b), x, y)) == true)
                {
                moveTo.Add(findSquare(getSquare(a, b), x, y));
                }
            }
            
        }

        moveTo = FilterList(moveTo);

        return moveTo;

    }

    public List<string> CastleMoveSquare(int a, int b)
    {

        List<string> moveTo = new List<string>();

        GameObject obj = board[a,b];

        //problem is that refers to CanCastle based off actual game and not theoretical moves
        if (game.CanCastle("e1", "a1") && getColour(obj).Equals("white") && Game2.whiteCastle)
        {
            moveTo.Add("a1");
        }

        if (game.CanCastle("e1", "h1") && getColour(obj).Equals("white") && Game2.whiteCastle)
        {
            moveTo.Add("h1");
        }

        if (game.CanCastle("e8", "a8") && getColour(obj).Equals("black") && Game2.blackCastle)
        {
            moveTo.Add("a8");
        }

        if (game.CanCastle("e8", "h8") && getColour(obj).Equals("black") && Game2.blackCastle)
        {
            moveTo.Add("h8");
        }

        return moveTo;

    }

    public void Castle(string s)
    {
        string[] arr = s.Split(',');
        string from = arr[0];
        string to = arr[1];

        GameObject obj = Access2DArray(from);
        GameObject obj2 = Access2DArray(to);

        if(s.Equals("e1,a1"))
        {
            AddBoard("b1", obj);
            AddBoard("c1", obj2);
            EmptyBoard(from);
            EmptyBoard(to);
        }

        if(s.Equals("e1,h1"))
        {
            AddBoard("f1", obj);
            AddBoard("e1", obj2);
            EmptyBoard(to);
        }

        if(s.Equals("e8,a8"))
        {
            AddBoard("b8", obj);
            AddBoard("c8", obj2);
            EmptyBoard(from);
            EmptyBoard(to);
        }

        if(s.Equals("e8,h8"))
        {
            AddBoard("f8", obj);
            AddBoard("e8", obj2);
            EmptyBoard(to);
        }

    }

    public List<string> WhitePawnMove(int a, int b, GameObject obj)
    {
        List<string> moveTo = new List<string>();
        List<string> ret = new List<string>();
        
        
        if(PawnHasMoved(b, obj))
        {
            if(isValidSquare(findSquare(getSquare(a, b), 0, 1)) && isOccupied(findSquare(getSquare(a, b), 0, 1)) && isOccupied(findSquare(getSquare(a, b), 0, 1)) == false)
            {
                moveTo.Add(findSquare(getSquare(a, b), 0, 1));
            }
        }
        else if(isValidSquare(findSquare(getSquare(a, b), 0, 1)) && isOccupied(findSquare(getSquare(a, b), 0, 1)) == false)
        {
            moveTo.Add(findSquare(getSquare(a, b), 0, 1));
            if(isValidSquare(findSquare(getSquare(a, b), 0, 2)) && isOccupied(findSquare(getSquare(a, b), 0, 2)) == false)
            {
                moveTo.Add(findSquare(getSquare(a, b), 0, 2));
            }
        }

        if(isValidSquare(findSquare(getSquare(a, b), 1, 1)) && isOccupied(findSquare(getSquare(a, b), 1, 1)) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), 1, 1)))
        {
            moveTo.Add(findSquare(getSquare(a, b), 1, 1));
        }
        if(isValidSquare(findSquare(getSquare(a, b), -1, 1)) && isOccupied(findSquare(getSquare(a, b), -1, 1)) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), -1, 1)))
        {
            moveTo.Add(findSquare(getSquare(a, b), -1, 1));
        }

        string westSquare = findSquare(getSquare(a, b), -1, 0);
        string eastSquare = findSquare(getSquare(a, b), 1, 0);

       if(isOccupied(westSquare) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), -1, 0)))
        {
            if(movedTwo.Equals(eastSquare))
            {
                moveTo.Add(findSquare(getSquare(a,b), -1, 1));
            }
        }

        if(isOccupied(eastSquare) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), 1, 0)))
        {
            if(movedTwo.Equals(eastSquare))
            {
                moveTo.Add(findSquare(getSquare(a,b), 1, 1));
            }
        }
        moveTo = FilterList(moveTo);


        return moveTo;

    }

    public List<string> BlackPawnMove(int a, int b, GameObject obj)
    {
        List<string> moveTo = new List<string>();
        if(PawnHasMoved(b, obj))
        {
            if(isValidSquare(findSquare(getSquare(a, b), 0, -1)) && isOccupied(findSquare(getSquare(a, b), 0, -1)) == false)
            {
                moveTo.Add(findSquare(getSquare(a, b), 0, -1));
            }
        }
        else if(isValidSquare(findSquare(getSquare(a, b), 0, -1)) && isOccupied(findSquare(getSquare(a, b), 0, -1)) == false)
        {
            moveTo.Add(findSquare(getSquare(a, b), 0, -1));
            if(isValidSquare(findSquare(getSquare(a, b), 0, -2)) && isOccupied(findSquare(getSquare(a, b), 0, -2)) == false)
            {
                moveTo.Add(findSquare(getSquare(a, b), 0, -2));
            }
        }
        if(isValidSquare(findSquare(getSquare(a, b), -1, -1)) && isOccupied(findSquare(getSquare(a, b), -1, -1)) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), -1, -1)))
        {
            moveTo.Add(findSquare(getSquare(a, b), -1, -1));
        }
        if(isValidSquare(findSquare(getSquare(a, b), 1, -1)) && isOccupied(findSquare(getSquare(a, b), 1, -1)) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), 1, -1)))
        {
            moveTo.Add(findSquare(getSquare(a, b), 1, -1));
        }

        string westSquare = findSquare(getSquare(a, b), -1, 0);
        string eastSquare = findSquare(getSquare(a, b), 1, 0);

       if(isOccupied(westSquare) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), -1, 0)))
        {
            if(movedTwo.Equals(eastSquare))
            {
                moveTo.Add(findSquare(getSquare(a,b), -1, -1));
            }
        }

        if(isOccupied(eastSquare) && isEnemy(getSquare(a, b), findSquare(getSquare(a, b), 1, 0)))
        {
            if(movedTwo.Equals(eastSquare))
            {
                moveTo.Add(findSquare(getSquare(a,b), 1, -1));
            }
        }


        

        

        moveTo = FilterList(moveTo);

        return moveTo;
        
    }

     public List<string> KnightMove(int x, int y)
    {
        List<string> l1 = PointMoveSquare(x, y, 1, 2);
        List<string> l2 = PointMoveSquare(x, y, 2, 1);
        List<string> l3 = PointMoveSquare(x, y, -1, -2);
        List<string> l4 = PointMoveSquare(x, y, -2 , -1);
        List<string> l5 = PointMoveSquare(x, y, -1, 2);
        List<string> l6 = PointMoveSquare(x, y, 1, -2);
        List<string> l7 = PointMoveSquare(x, y, -2, 1);
        List<string> l8 = PointMoveSquare(x, y, 2, -1);

        return l1.Concat(l2).Concat(l3).Concat(l4).Concat(l5).Concat(l6).Concat(l7).Concat(l8).ToList();


    }

    public List<string> BishopMove(int x, int y)
    {
        List<string> l1 = LineMoveSquare(x, y, 1, 1);
        List<string> l2 = LineMoveSquare(x, y, 1, -1);
        List<string> l3 = LineMoveSquare(x, y, -1, 1);
        List<string> l4 = LineMoveSquare(x, y, -1, -1);

        List<string> check = l1.Concat(l2).Concat(l3).Concat(l4).ToList();

        return l1.Concat(l2).Concat(l3).Concat(l4).ToList();
    }

    public List<string> RookMove(int x, int y)
    {
        List<string> l1 = LineMoveSquare(x, y, 1, 0);
        List<string> l2 = LineMoveSquare(x, y, 0, -1);
        List<string> l3 = LineMoveSquare(x, y, 0, 1);
        List<string> l4 = LineMoveSquare(x, y, -1, 0);

        return l1.Concat(l2).Concat(l3).Concat(l4).ToList();

    }

    public List<string> KingMove(int x, int y)
    {
        List<string> l1 = PointMoveSquare(x, y, 0, 1);
        List<string> l2 = PointMoveSquare(x, y, 1, 0);
        List<string> l3 = PointMoveSquare(x, y, -1, 0);
        List<string> l4 = PointMoveSquare(x, y, 0, -1);
        List<string> l5 = PointMoveSquare(x, y, 1, 1);
        List<string> l6 = PointMoveSquare(x, y, -1, -1);
        List<string> l7 = PointMoveSquare(x, y, 1, -1);
        List<string> l8 = PointMoveSquare(x, y, -1, 1);
        List<string> l9 = CastleMoveSquare(x, y);

        return l1.Concat(l2).Concat(l3).Concat(l4).Concat(l5).Concat(l6).Concat(l7).Concat(l8).Concat(l9).ToList();

    }

    public List<string> QueenMove(int x, int y)
    {
        List<string> l1 = BishopMove(x, y);
        List<string> l2 = RookMove(x, y);

        return l1.Concat(l2).ToList();
    }

    public List<string> MoveSquarePositions(int x, int y)
    {
        GameObject obj = board[x, y];
        
        switch (obj.name)
        {
            case "black_pawn(Clone)": return BlackPawnMove(x, y, obj); 
            case "white_pawn(Clone)": return WhitePawnMove(x, y, obj);  
            case "black_knight(Clone)": return KnightMove(x, y); 
            case "white_knight(Clone)": return KnightMove(x, y); 
            case "black_bishop(Clone)": return BishopMove(x, y); 
            case "white_bishop(Clone)": return BishopMove(x, y); 
            case "black_rook(Clone)": return RookMove(x, y); 
            case "white_rook(Clone)": return RookMove(x, y); 
            case "black_queen(Clone)": return QueenMove(x, y); 
            case "white_queen(Clone)": return QueenMove(x, y); 
            case "black_king(Clone)": return KingMove(x, y); 
            case "white_king(Clone)": return KingMove(x, y); 
        }

        return new List<string>();

    }

    public List<string> FilterList(List<string> lst)
    {
        List<string> ret = new List<string>();
        foreach(string s in lst)
        {
            if(isValidSquare(s))
            {
                ret.Add(s);
            }

        }

        return ret;
    }

    public List<string> PossibleMoves(string c)
    {
        List<string> moves = new List<string>();
        if(c.Equals("white"))
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(board[i, j] != null)
                    {
                        if(getColour(board[i, j]).Equals("white"))
                        {
                            string from = getSquare(i, j);
                            List<string> tos = MoveSquarePositions(i, j);
                            foreach(string s in tos)
                            {
                                moves.Add(from + "," + s);
                            }
                        }
                    }
                }
            }
        }

        if(c.Equals("black"))
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(board[i, j] != null)
                    {
                        if(getColour(board[i, j]).Equals("black"))
                        {
                            string from = getSquare(i, j);
                            List<string> tos = MoveSquarePositions(i, j);
                            foreach(string s in tos)
                            {
                                moves.Add(from + "," + s);
                            }
                        }
                    }
                }
            }
        }

        return moves;
    }

    public void ChangeTurns()
    {
        if(whiteTurn)
        {
            whiteTurn = false;
        }
        else
        {
            whiteTurn = true;
        }

    }

    public int CalculateLP(string colour, string coords)
    {
        int x = (int) coords[0] - 97;
        int y = (int) Char.GetNumericValue(coords[1]) - 1;
        GameObject obj = board[x, y];
        if(colour.Equals("white"))
        {
            switch(obj.name)
            {
                case "white_pawn(Clone)": return -50;
                case "black_pawn(Clone)": return 50;
                case "white_knight(Clone)": return -150;
                case "black_knight(Clone)": return 150;
                case "white_bishop(Clone)": return -150;
                case "black_bishop(Clone)": return 150;
                case "white_rook(Clone)": return -250;
                case "black_rook(Clone)": return 250;
                case "white_queen(Clone)": return -500;
                case "black_queen(Clone)": return 500;
                case "white_king(Clone)": return -1000;
                case "black_king(Clone)": return 1000;

            }
        
        
    }

    else
        {
            switch(obj.name)
            {
                case "white_pawn(Clone)": return 50;
                case "black_pawn(Clone)": return -50;
                case "white_knight(Clone)": return 150;
                case "black_knight(Clone)": return -150;
                case "white_bishop(Clone)": return 150;
                case "black_bishop(Clone)": return -150;
                case "white_rook(Clone)": return 250;
                case "black_rook(Clone)": return -250;
                case "white_queen(Clone)": return 500;
                case "black_queen(Clone)": return -500;
                case "white_king(Clone)": return 1000;
                case "black_king(Clone)": return -1000;

                }
        }

        return 0;

    }

    public bool CheckIfMovedTwo(string coords)
    {
        string[] arr = coords.Split(',');
        string from = arr[0];

        GameObject obj = Access2DArray(from);

        if(MinusTwoSquares(coords) == 2 && IsPawn(from))
        {
            return true;
        }

        return false;
        
    }

    public double MinusTwoSquares(string squares)
    {
        char c1 = squares[1];
        char c2 = squares[4];

        double d1 = Char.GetNumericValue(c1);
        double d2 = Char.GetNumericValue(c2);

        return Math.Abs(d2 - d1);
    }

    public bool IsPawn(string s)
    {
        if(Access2DArray(s) == null)
        {
            return false;
        }
        GameObject obj = Access2DArray(s);

        switch(obj.name)
        {
            case "black_pawn(Clone)": return true;
            case "white_pawn(Clone)": return true;
        }

        return false;
    }

    public bool IsCastling(string s)
    {
        switch(s)
        {
            case "e1,a1": return true;
            case "e1,h1": return true;
            case "e8,a8": return true;
            case "e8,h8": return true;
        }
        return false;
    }
    public bool IsEnPassant(string s)
    {
        string[] arr = s.Split(',');
        string from = arr[1];
        
        if(Access2DArray(from) == null)
        {
            return false;
        }

        if(IsPawn(from))
        {
            if(s[1].Equals(s[4]))
            {
                return true;
            }
        }
        return false;
    }

}
