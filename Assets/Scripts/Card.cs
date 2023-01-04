using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public SpriteRenderer render;
    public bool used = false;
    public GameObject controller;
    public Game game;
    public static string destroySquare = "";
    public static bool destroy = false;
    public GameObject vessel;
    public Connection con;

    public GameObject des;
    public GameObject cardText;
    public Text cText;


    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game>();
        vessel = GameObject.FindGameObjectWithTag("Connection");
        con = vessel.GetComponent<Connection>();
        des = GameObject.FindGameObjectWithTag("Description");
        des.GetComponent<Image>().enabled = false;
        cardText = GameObject.FindGameObjectWithTag("CardText");
        cText = cardText.GetComponent<Text>();
    }

    void Update()
    {
        

    }

    private void OnMouseEnter()
    {
        des.GetComponent<Image>().enabled = true;
        des.transform.position = new Vector3(this.transform.position.x + 100, this.transform.position.y + 150, -10);
        cText.text = SetCardText();
    }

    private void OnMouseExit()
    {
        des.GetComponent<Image>().enabled = false;
        cText.text = "";
    }


     private string SetCardText()
    {
        string text = "";
        switch(this.name)
        {
            case "black_skip_turn_card(Clone)": text = "Skip turn. +50 LP."; break;
            case "black_castle_card(Clone)": text = "Skip turn. Castling enabled."; break;
            case "white_take_back_card(Clone)": text = "Move is taken back x1."; break;
            case "white_take_back_x3_card(Clone)": text = "Move is taken back x3."; break;
            case "black_en_passant_trap_card(Clone)": text = "If opponent en passants they lose 200 LP x1. Non-stackable."; break;
            case "black_promotion_trap_card(Clone)": text = "If opponent promotes they lose 200 LP x1. Non-stackable."; break;
            case "white_queen_card(Clone)": text = "Skip turn. White pawns will promote to queens x1. Non-stackable."; break;

            case "white_skip_turn_card(Clone)": text = "Skip turn. +50 LP."; break;
            case "white_castle_card(Clone)": text = "Skip turn. Castling enabled."; break;
            case "black_take_back_card(Clone)": text = "Move is taken back x1."; break;
            case "black_take_back_x3_card(Clone)": text = "Move is taken back x3."; break;
            case "white_en_passant_trap_card(Clone)": text = "If opponent en passants they lose 200 LP x1. Non-stackable."; break;
            case "white_promotion_trap_card(Clone)": text = "If opponent promotes they lose 200 LP x1. Non-stackable."; break;
            case "black_queen_card(Clone)": text = "Skip turn. Black pawns will promote to queens x1. Non-stackable. "; break;
            
        }

        return text;
    }
    private void OnMouseUp()
    {
        if(this.used == false && Game.play)
        {
            if(Game.whiteMove == true)
            {
                WhiteEffect();
            }
            else
            {
                BlackEffect();
            }
        }
    }

    public void WhiteEffect()
    {
        string player = "";
        switch(this.name)
        {
            case "white_skip_turn_card(Clone)": WhiteSkipTurn(); Game.cardJustUsed = "white_skip"; break;
            case "white_castle_card(Clone)": WhiteCastle(); Game.cardJustUsed = "white_castle";  break;
            case "white_take_back_card(Clone)":   if (Game.moves > 0){
                TakeBack(); 
                Game.whiteMove = false;
            Game.blackMove = true;
            }
            break;
            case "white_take_back_x3_card(Clone)": if (Game.moves >2 )
            {
                TakeBackX3();
                Game.whiteMove = false;
            Game.blackMove = true;
            }
            break;
            case "white_en_passant_trap_card(Clone)": Game.cardJustUsed = "black_ep";  BlackEnPassantTrap(); break;
            case "white_promotion_trap_card(Clone)": Game.cardJustUsed = "black_promotion";  BlackPromotionTrap(); break;
            case "white_queen_card(Clone)": if(game.whiteQ == false){ 
            Game.cardJustUsed = "white_queen"; WhiteQueenCard();
            }
            break;
        }

        if(game.P1)
        {
            player = "p1";
        }
        else
        {
            player = "p2";
        }

        con.CardMirror(this.name + "," + player);

    }

    public void BlackEffect()
    {
        string player = "";
        switch(this.name)
        {
            case "black_skip_turn_card(Clone)": BlackSkipTurn(); Game.cardJustUsed = "black_skip";  break;
            case "black_castle_card(Clone)": BlackCastle(); Game.cardJustUsed = "black_castle";  break;
            case "black_take_back_card(Clone)": if (Game.moves > 0) {
                TakeBack(); 
            Game.whiteMove = true;
            Game.blackMove = false;
            }
            break;
            case "white_take_back_x3_card(Clone)": if (Game.moves > 2) {
                TakeBackX3(); 
            Game.whiteMove = true;
            Game.blackMove = false;
            }
            break;
            case "black_en_passant_trap_card(Clone)": WhiteEnPassantTrap(); Game.cardJustUsed = "white_ep";  break;
            case "black_promotion_trap_card(Clone)": WhitePromotionTrap(); Game.cardJustUsed = "white_promotion";  break;
            case "black_queen_card(Clone)": if(game.blackQ == false)
            {
                Game.cardJustUsed = "black_queen"; BlackQueenCard(); 
            }
            break;
        }

        if(game.P1)
        {
            player = "p1";
        }
        else
        {
            player = "p2";
        }

        con.CardMirror(this.name + "," + player);
    }

    public void CardMirrorEffect()
    {
        switch(this.name)
        {
            case "white_skip_turn_card(Clone)": WhiteSkipTurn(); break;
            case "white_castle_card(Clone)": WhiteCastle(); break;
            case "black_take_back_card(Clone)":   if (Game.moves > 0){
                TakeBack(); 
                Game.whiteMove = false;
            Game.blackMove = true;
            }
            break;
            case "black_take_back_x3_card(Clone)": if (Game.moves >2 )
            {
                TakeBackX3();
                Game.whiteMove = false;
            Game.blackMove = true;
            }
            break;
            case "white_en_passant_trap_card(Clone)": BlackEnPassantTrap(); break;
            case "white_promotion_trap_card(Clone)":  BlackPromotionTrap(); break;
            case "white_queen_card(Clone)": WhiteQueenCard(); break;
            case "black_skip_turn_card(Clone)": BlackSkipTurn();  break;
            case "black_castle_card(Clone)": BlackCastle(); break;
            case "white_take_back_card(Clone)": if (Game.moves > 0) {
                TakeBack(); 
            Game.whiteMove = true;
            Game.blackMove = false;
            }
            break;
            case "white_take_back_x3_card(Clone)": if (Game.moves > 2) {
                TakeBackX3(); 
            Game.whiteMove = true;
            Game.blackMove = false;
            }
            break;
            case "black_en_passant_trap_card(Clone)": WhiteEnPassantTrap(); break;
            case "black_promotion_trap_card(Clone)": WhitePromotionTrap(); break;
            case "black_queen_card(Clone)": BlackQueenCard(); break;
        }
        
    }

    public void WhiteSkipTurn()
    {
        Game.whiteMove = false;
        Game.blackMove = true;
        Game.whiteLifePoints += 50;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackSkipTurn()
    {
        Game.whiteMove = true;
        Game.blackMove = false;
        Game.blackLifePoints += 50;
        used = true;
        render.material.color = Color.red;
    }

    public void WhiteCastle()
    {
        Game.whiteCastle = true;
        Game.whiteMove = false;
        Game.blackMove = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackCastle()
    {
        Game.blackCastle = true;
        Game.whiteMove = true;
        Game.blackMove = false;
        used = true;
        render.material.color = Color.red;
    }

    public void TakeBack()
    {
        string lastMove = Game.previousMoves[Game.previousMoves.Count - 1];
        Game.previousMoves.RemoveAt(Game.previousMoves.Count -1);
        string[] positions = lastMove.Split(',');
        string reversed = positions[1] + "," + positions[0];

        game.TakeBackMove(reversed);

        if(Game.lastMoveDestroyed[Game.lastMoveDestroyed.Count - 1] == true)
        {
            game.Spawn(Game.destroyedPieces[Game.destroyedPieces.Count - 1], positions[1]);
            Game.destroyedPieces.RemoveAt(Game.destroyedPieces.Count -1);
        }
        Game.lastMoveDestroyed.RemoveAt(Game.lastMoveDestroyed.Count -1);
        Game.moves--;
        used = true;
        render.material.color = Color.red;
    }

    public void TakeBackX3()
    {
        if(Game.whiteMove)
        {
            Game.blackLifePoints -= 100;
        }
        else
        {
            Game.whiteLifePoints -= 100;
        }

        for(int i = 0; i < 3; i++)
        {
            TakeBack();
        }
        used = true;
        render.material.color = Color.red;
    
    }

    public bool isWhite()
    {
        switch(this.name)
        {
            case "white_skip_turn_card(Clone)": return true;
            case "white_castle_card(Clone)": return true;
            case "white_take_back_card(Clone)": return true;
            case "white_destroy_pawn_card(Clone)": return true;
        }

        return false;
    }

    public void WhiteEnPassantTrap()
    {
        Game.whiteEnPassantX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackEnPassantTrap()
    {
        Game.blackEnPassantX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void WhitePromotionTrap()
    {
        Game.whitePromotionX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackPromotionTrap()
    {
        Game.blackPromotionX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void WhiteQueenCard()
    {
        Game.whiteMove = false;
        Game.blackMove = true;
        game.whiteQ = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackQueenCard()
    {
        Game.whiteMove = true;
        Game.blackMove = false;
        game.blackQ = true;
        used = true;
        render.material.color = Color.red;
    }


}
