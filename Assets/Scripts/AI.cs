using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading;

public class AI : MonoBehaviour
{
    public bool cardProbability = false;
    GameObject ghost;
    GhostBoard ghostBoard;
    GameObject controller;
    Game2 game;
    private string colour;
    private string oppositeColour;
    public int cardsUsed = 0;
    System.Random rnd = new System.Random();
    System.Random rndCard = new System.Random();

    public GameObject white_skip_turn_card, black_skip_turn_card, white_castle_card, black_castle_card,

    white_en_passant_trap_card, black_en_passant_trap_card, white_promotion_trap_card, black_promotion_trap_card;

    public void Awake()
    {
        GameObject ghost = GameObject.FindGameObjectWithTag("Ghost");
        ghostBoard = ghost.GetComponent<GhostBoard>();
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game2>();
        if(MainMenu.colour.Equals("white"))
        {
            colour = "black";
            oppositeColour = "white";
        }
        else
        {
            colour = "white";
            oppositeColour = "black";
        }

    }

    public string GenerateRandomMove()
    {

        List<string> moves = PossibleMoves();

        int index = rnd.Next(0, moves.Count - 1);

        return moves[index];
    }

    public string GenerateRandomHumanMove()
    {

        List<string> moves = PossibleMovesHuman();

        int index = rnd.Next(0, moves.Count - 1);

        return moves[index];
    }

    public void GenerateRandomCardMove()
    {
        GameObject card;
        GameObject obj;
        Card2 newCard;

        GameObject[] whiteCards = {white_skip_turn_card, white_castle_card, white_en_passant_trap_card, white_promotion_trap_card};

        GameObject[] blackCards = {black_skip_turn_card, black_castle_card, black_en_passant_trap_card, black_promotion_trap_card};

        if(game.P1)
        {
            card = blackCards[rndCard.Next(0, 3)];
        }
        else
        {
            card = whiteCards[rndCard.Next(0, 3)];
        }

        if(cardsUsed == 0)
        {
            obj = Instantiate(card, new Vector3(-8.1f, 1.8f, -3), Quaternion.identity);
            newCard = obj.GetComponent<Card2>(); 
            newCard.CardMirrorEffect();
            Debug.Log(newCard.name);
            
        }
        else if(cardsUsed == 1)
        {
            obj = Instantiate(card, new Vector3(-6.8f, 1.8f, -3), Quaternion.identity);
            newCard = obj.GetComponent<Card2>(); 
            newCard.CardMirrorEffect();
            Debug.Log(newCard.name);
        }
        else if(cardsUsed == 2)
        {
            obj = Instantiate(card, new Vector3(-5.5f, 1.8f, -3), Quaternion.identity);
            newCard = obj.GetComponent<Card2>(); 
            newCard.CardMirrorEffect();
            Debug.Log(newCard.name);
        }

        cardsUsed++;

    }

    public List<string> PossibleMoves()
    {
        List<string> moves = new List<string>();
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject obj in pieces)
        {
            Piece2 p = obj.GetComponent<Piece2>();
            if(p.getColour().Equals(this.colour))
            {
                string from = p.getSquare();
                List<string> tos = p.MoveSquarePositions();
                foreach (string s in tos)
                {
                    moves.Add(from + "," + s);
                }

            }
        }

        return moves;
    }

    public List<string> PossibleMovesHuman()
    {
        List<string> moves = new List<string>();
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject obj in pieces)
        {
            Piece2 p = obj.GetComponent<Piece2>();
            if(p.getColour().Equals(this.colour) == false)
            {
                string from = p.getSquare();
                List<string> tos = p.MoveSquarePositions();
                foreach (string s in tos)
                {
                    moves.Add(from + "," + s);
                }

            }
        }

        return moves;
    }

    public string BestMoveDepth1()
    {
        int max = 0;
        string bestMove = GenerateRandomMove();
        List<string> moves = PossibleMoves();
        foreach (string s in moves)
        {
            int temp = game.PseudoMove(s);
            if(temp > max)
            {
                max = temp;
                bestMove = s;
            }
        }

       return bestMove;

    }

    public string BestMoveHuman()
    {
        int max = 0;
        string bestMove = GenerateRandomHumanMove();
        List<string> moves = PossibleMovesHuman();
        foreach (string s in moves)
        {
            int temp = game.PseudoMove(s);
            if(temp > max)
            {
                max = temp;
                bestMove = s;
            }
        }

       return bestMove;

    }

    public string BestMoveDepth2()
    {
        int whiteLP = Game2.whiteLifePoints;
        int blackLP = Game2.blackLifePoints;
        int balance;
        if(colour.Equals("white"))
        {
            balance = whiteLP - blackLP;
        }
        else
        {
            balance = blackLP - whiteLP;
        }

        int max = -1000;
        string bestMove = GenerateRandomMove();
        List<string> moves = PossibleMoves();

        foreach (string s in moves)
        {
            game.PseudoMove(s);
            string s2 = BestMoveHuman();
            game.PseudoMove(s);

            if(colour.Equals("white"))
            {
                if(max < (Game2.whiteLifePoints - Game2.blackLifePoints))
                {
                    max = Game2.whiteLifePoints - Game2.blackLifePoints;
                    bestMove = s;
                }
            }
            else
            {
                if(max < (Game2.blackLifePoints - Game2.whiteLifePoints))
                {
                    max = Game2.blackLifePoints - Game2.whiteLifePoints;
                    bestMove = s;
                }
            }

            game.PseudoTakeBack();

            Game2.whiteLifePoints = whiteLP;
            Game2.blackLifePoints = blackLP;

        }

        if(colour.Equals("white"))
        {
            if(balance < (Game2.whiteLifePoints - Game2.blackLifePoints))
            {
                return bestMove;
            }
        }
        else
        {
            if(max < (Game2.blackLifePoints - Game2.whiteLifePoints))
            {
                return bestMove;
            }
        }

        return GenerateRandomMove();

    }

    //this is AI based off of using the GhostBoard class rather than moving objects in the extant game

    public string GenerateRandomGhostMove()
    {
        //generates every legal move of colour
        List<string> moves = ghostBoard.PossibleMoves(colour);

        int index = rnd.Next(0, moves.Count - 1);

        return moves[index];
    }

    public string GenerateRandomHumanGhostMove()
    {
        List<string> moves = ghostBoard.PossibleMoves(oppositeColour);

        int index = rnd.Next(0, moves.Count - 1);

        return moves[index];
    }

    public string GenerateDepth1GhostMove()
    {
        //need to use tempBoard variable or else ghostBoard.board will keep resetting to Game2.positions
        ghostBoard.InitialiseTempBoard();
        int max = 0;
        string bestMove = GenerateRandomGhostMove();
        List<string> moves = ghostBoard.PossibleMoves(colour);
        foreach (string s in moves)
        {
            int temp = HeuristicFunction(s);
            temp += ghostBoard.Move(s, colour);
            if(temp > max)
            {
                max = temp;
                bestMove = s;
            }
            ghostBoard.TempBoard();
        }

       return bestMove;

    }

    public string GenerateDepth1HumanMove()
    {
        ghostBoard.InitialiseTempBoard();
        int max = 0;
        string bestMove = GenerateRandomHumanGhostMove();
        List<string> moves = ghostBoard.PossibleMoves(oppositeColour);
        foreach (string s in moves)
        {
            int temp = HeuristicFunction(s);
            temp += ghostBoard.Move(s, oppositeColour);
            if(temp > max)
            {
                max = temp;
                bestMove = s;
            }
            ghostBoard.TempBoard();
        }

       return bestMove;

    }

    public string GenerateDepth2GhostMove()
    {
        ghostBoard.InitialiseBoard();
        int max = -10000;
        string bestMove = GenerateRandomGhostMove();
        List<string> moves = ghostBoard.PossibleMoves(colour);
        //shuffle moves or else AI will always select the first move in the array if all else equal
        moves.Shuffle();
        foreach (string s in moves)
        {
            int temp = HeuristicFunction(s);
            temp += ghostBoard.Move(s, colour);
            string oMove = GenerateDepth1HumanMove();
            temp += ghostBoard.Move(oMove, colour);
            if(temp > max)
            {
                max = temp;
                bestMove = s;
            }
            ghostBoard.TempBoard();
        }

        //if the situation is neither very bad nor very good there is a possibility of the AI using a card
        if(max < 50 && max > -50)
        {
            cardProbability = true;
        }

        return bestMove;

    }

    public string GenerateDepth2HumanMove()
    {
        ghostBoard.InitialiseTempBoard();
        int max = -10000;
        string bestMove = GenerateRandomHumanMove();
        List<string> moves = ghostBoard.PossibleMoves(oppositeColour);
        moves.Shuffle();
        foreach (string s in moves)
        {
            int temp = HeuristicFunction(s);
            temp += ghostBoard.Move(s, oppositeColour);
            string oMove = GenerateDepth1GhostMove();
            temp = temp + ghostBoard.Move(oMove, oppositeColour);
            if(temp > max)
            {
                max = temp;
                bestMove = s;
            }
            ghostBoard.InitialiseBoard();
        }

       return bestMove;

    }

    public string GenerateDepth3GhostMove()
    {
        ghostBoard.InitialiseBoard();
        int max = -10000;
        string bestMove = GenerateRandomGhostMove();
        List<string> moves = ghostBoard.PossibleMoves(colour);
        moves.Shuffle();
        foreach (string s in moves)
        {
            int temp = HeuristicFunction(s);
            temp += ghostBoard.Move(s, colour);
            string oMove = GenerateDepth1HumanMove();
            temp = temp + ghostBoard.Move(oMove, colour);
            string move = GenerateDepth1GhostMove();
            temp = temp + ghostBoard.Move(move, colour);
            if(temp > max)
            {
                max = temp;
                bestMove = s;
            }
            ghostBoard.InitialiseBoard();
        }

       return bestMove;

    }

    public int Evaluate(string colour)
    {
        int value = 0;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (ghostBoard.board[i,j] != null)
                {
                    GameObject obj = ghostBoard.board[i,j];
                    switch(obj.name)
                    {
                        case "white_pawn(Clone)": value += -50; break;
                        case "black_pawn(Clone)": value += 50; break;
                        case "white_knight(Clone)": value += -150; break;
                        case "black_knight(Clone)": value += 150; break;
                        case "white_bishop(Clone)": value += -150; break;
                        case "black_bishop(Clone)": value += 150; break;
                        case "white_rook(Clone)": value += -250; break;
                        case "black_rook(Clone)": value += 250; break;
                        case "white_queen(Clone)": value += -500; break;
                        case "black_queen(Clone)": value += 500; break;
                        case "white_king(Clone)": value += -1000; break;
                        case "black_king(Clone)": value += 1000; break;

                    }  

                }
            }
        }

        return value;
    }

    //Minimax algorithm adapted from https://github.com/byanofsky/chess-ai-2/blob/master/public/js/movecalc.js
    public ArrayList Minimax(int depth, GameObject[,] bboard, string pColour, bool maximisingPlayer)
    {
        var value = 0;
        if(depth == 0)
        {
            value = Evaluate(pColour);
            var arlist = new ArrayList(); 
            arlist.Add(value);
            arlist.Add(null);
            return arlist;
        }

        string bestMove = null;
        List<string> moves = ghostBoard.PossibleMoves(pColour);
        moves.Shuffle();
        int bestMoveValue;

        if(maximisingPlayer)
        {
            bestMoveValue = -100000;
        }
        else
        {
            bestMoveValue = 100000;
        }

        foreach(string s in moves)
        {
            string possMove = s;
            GameObject[,] newBoard = ghostBoard.Move(possMove, bboard);
            GameObject[,] saved = ghostBoard.SaveBoard(newBoard);
            value = (int) Minimax(depth-1, newBoard, pColour, !maximisingPlayer)[0];

             if (maximisingPlayer) {
                if ((int) value > bestMoveValue) 
                {
                    bestMoveValue = (int) value;
                    bestMove = possMove;
                }
                } 
                else {
                    if ((int) value < bestMoveValue) 
                    {
                        bestMoveValue = (int) value;
                        bestMove = possMove;
                    }
                }

                newBoard = ghostBoard.SaveBoard(saved);
                
        }


         var arlist2 = new ArrayList(); 
            arlist2.Add(value);
            arlist2.Add(bestMove);
            return arlist2;


    }

    


    public int HeuristicFunction(string s)
    {
      string[] positions = s.Split(',');
      string from = positions[0];
      string to = positions[1];

      //bad idea to move king if can move other piece
      if(IsKing(s))
      {
          return -1;
      }
      //moving king pawn is considered to be the most solid opening in theory
      if(IsPawn(from) && ((from.Equals("e2") && (to.Equals("e4")) || (from.Equals("e7") && to.Equals("e5")))))
      {
          return 2;
      }
      //"develop" your pieces is basic chess advice
      if((IsKnight(from) || IsBishop(from)) && AtBackRank(from) && Game2.moves < 10)
      {
          return 1;
      }

      if(PawnWillPromote(from))
      {
          return 150;
      }

      return 0;

    }

    public bool IsKing(string s)
    {
        if(ghostBoard.Access2DArray(s) == null)
        {
            return false;
        }
        GameObject obj = ghostBoard.Access2DArray(s);

        switch(obj.name)
        {
            case "black_king(Clone)": return true;
            case "white_king(Clone)": return true;
        }

        return false;
    }

    public bool IsPawn(string s)
    {
        if(ghostBoard.Access2DArray(s) == null)
        {
            return false;
        }
        GameObject obj = ghostBoard.Access2DArray(s);

        switch(obj.name)
        {
            case "black_pawn(Clone)": return true;
            case "white_pawn(Clone)": return true;
        }

        return false;
    }

    public bool IsKnight(string s)
    {
        if(ghostBoard.Access2DArray(s) == null)
        {
            return false;
        }
        GameObject obj = ghostBoard.Access2DArray(s);

        switch(obj.name)
        {
            case "black_knight(Clone)": return true;
            case "white_knight(Clone)": return true;
        }

        return false;
    }

    public bool IsBishop(string s)
    {
        if(ghostBoard.Access2DArray(s) == null)
        {
            return false;
        }
        GameObject obj = ghostBoard.Access2DArray(s);

        switch(obj.name)
        {
            case "black_bishop(Clone)": return true;
            case "white_bishop(Clone)": return true;
        }

        return false;
    }

    public bool AtBackRank(string s)
    {
        GameObject obj = ghostBoard.Access2DArray(s);
        string c = ghostBoard.getColour(obj);
        char num = s[1];
        if((c.Equals("white") && num.Equals('1')) ||(c.Equals("black") &&  num.Equals('8')))
        {
            return true;
        }
        return false;
    }

    public bool PawnWillPromote(string s)
    {
        if(IsPawn(s))
        {
            GameObject obj = ghostBoard.Access2DArray(s);
            string c = ghostBoard.getColour(obj);
            char num = s[1];
            if((c.Equals("white") && num.Equals('7')) ||(c.Equals("black") &&  num.Equals('2')))
            {
                return true;
            }
        }


        return false;
    }



    public void ResetBoard()
    {
        ghostBoard.InitialiseBoard();
    }

    public void DebugBoard()
    {
        string s = "";
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(ghostBoard.board[i,j] == null)
                {
                    s += "*";
                }
                else
                {
                    s += "P";
                }
            }
            Debug.Log(s + "\n");
            s = "";
        }
    }


}

//Fischer-Yates shuffle learnt from https://www.delftstack.com/howto/csharp/shuffle-a-list-in-csharp/
static class ExtensionsClass
    {
        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }


    
