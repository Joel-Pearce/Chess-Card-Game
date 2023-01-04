using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card2 : MonoBehaviour
{
    public SpriteRenderer render;
    public bool used = false;
    public GameObject controller;
    public Game2 game;
    public GameObject vessel;
    public GameObject des;
    public GameObject cardText;
    public Text cText;
    
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        controller = GameObject.FindGameObjectWithTag("GameController");
        game = controller.GetComponent<Game2>();
        des = GameObject.FindGameObjectWithTag("Description");
        des.GetComponent<Image>().enabled = false;
        cardText = GameObject.FindGameObjectWithTag("CardText");
        cText = cardText.GetComponent<Text>();

    }

    void Update()
    {

        
    }

    private string SetCardText()
    {
        string text = "";
        switch(this.name)
        {
            case "black_skip_turn_card(Clone)": text = "Skip turn. +50 LP."; break;
            case "black_castle_card(Clone)": text = "Skip turn. Castling enabled."; break;
            case "white_take_back_card(Clone)": text = "Move is taken back x2."; break;
            case "white_take_back_x3_card(Clone)": text = "Move is taken back x3."; break;
            case "black_en_passant_trap_card(Clone)": text = "If opponent en passants they lose 200 LP x1. Non-stackable."; break;
            case "black_promotion_trap_card(Clone)": text = "If opponent promotes they lose 200 LP x1. Non-stackable."; break;
            case "white_queen_card(Clone)": text = "Skip turn. White pawns will promote to queens x1. Non-stackable."; break;

            case "white_skip_turn_card(Clone)": text = "Skip turn. +50 LP."; break;
            case "white_castle_card(Clone)": text = "Skip turn. Castling enabled."; break;
            case "black_take_back_card(Clone)": text = "Move is taken back x2."; break;
            case "black_take_back_x3_card(Clone)": text = "Move is taken back x3."; break;
            case "white_en_passant_trap_card(Clone)": text = "If opponent en passants they lose 200 LP x1. Non-stackable."; break;
            case "white_promotion_trap_card(Clone)": text = "If opponent promotes they lose 200 LP x1. Non-stackable."; break;
            case "black_queen_card(Clone)": text = "Skip turn. Black pawns will promote to queens x1. Non-stackable."; break;
            
        }

        return text;
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


    private void OnMouseUp()
    {
        if(this.used == false)
        {
            if(Game2.whiteMove == true)
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
        switch(this.name)
        {
            case "white_skip_turn_card(Clone)": WhiteSkipTurn(); Game2.cardJustUsed = "white_skip"; break;
            case "white_castle_card(Clone)": WhiteCastle(); Game2.cardJustUsed = "white_castle";  break;
            case "white_take_back_card(Clone)":   if (Game2.moves > 1){
                TakeBackx2(); 
                Game2.whiteMove = true;
            Game2.blackMove = false;
            }
            break;
            case "black_take_back_x3_card(Clone)": if (Game2.moves >2 )
            {
                TakeBackX3();
                Game2.whiteMove = false;
            Game2.blackMove = true;
            }
            break;
            case "white_en_passant_trap_card(Clone)": Game2.cardJustUsed = "black_ep";  BlackEnPassantTrap(); break;
            case "white_promotion_trap_card(Clone)": Game2.cardJustUsed = "black_promotion";  BlackPromotionTrap(); break;
            case "white_queen_card(Clone)": Game2.cardJustUsed = "white_queen"; WhiteQueenCard(); break;
        }


    }

    public void BlackEffect()
    {
        switch(this.name)
        {
            case "black_skip_turn_card(Clone)": BlackSkipTurn(); Game2.cardJustUsed = "black_skip";  break;
            case "black_castle_card(Clone)": BlackCastle(); Game2.cardJustUsed = "black_castle";  break;
            case "black_take_back_card(Clone)": if (Game2.moves > 1) {
                TakeBackx2(); 
            Game2.whiteMove = false;
            Game2.blackMove = true;
            }
            break;
            case "white_take_back_x3_card(Clone)": if (Game2.moves > 2) {
                TakeBackX3(); 
            Game2.whiteMove = true;
            Game2.blackMove = false;
            }
            break;
            case "black_en_passant_trap_card(Clone)": WhiteEnPassantTrap(); Game2.cardJustUsed = "white_ep";  break;
            case "black_promotion_trap_card(Clone)": WhitePromotionTrap(); Game2.cardJustUsed = "white_promotion";  break;
            case "black_queen_card(Clone)": Game2.cardJustUsed = "black_queen"; BlackQueenCard(); break;
        }
    }

    public void CardMirrorEffect()
    {
        switch(this.name)
        {
            case "white_skip_turn_card(Clone)": WhiteSkipTurn(); break;
            case "white_castle_card(Clone)": WhiteCastle(); break;
            case "black_take_back_card(Clone)":   if (Game2.moves > 0){
                TakeBack(); 
                Game2.whiteMove = false;
            Game2.blackMove = true;
            }
            break;
            case "black_take_back_x3_card(Clone)": if (Game2.moves >2 )
            {
                TakeBackX3();
                Game2.whiteMove = false;
            Game2.blackMove = true;
            }
            break;
            case "white_en_passant_trap_card(Clone)": BlackEnPassantTrap(); break;
            case "white_promotion_trap_card(Clone)":  BlackPromotionTrap(); break;
            case "white_queen_card(Clone)": WhiteQueenCard(); break;
            case "black_skip_turn_card(Clone)": BlackSkipTurn();  break;
            case "black_castle_card(Clone)": BlackCastle(); break;
            case "white_take_back_card(Clone)": if (Game2.moves > 0) {
                TakeBack(); 
            Game2.whiteMove = true;
            Game2.blackMove = false;
            }
            break;
            case "white_take_back_x3_card(Clone)": if (Game2.moves > 2) {
                TakeBackX3(); 
            Game2.whiteMove = true;
            Game2.blackMove = false;
            }
            break;
            case "black_en_passant_trap_card(Clone)": WhiteEnPassantTrap(); break;
            case "black_promotion_trap_card(Clone)": WhitePromotionTrap(); break;
            case "black_queen_card(Clone)": BlackQueenCard(); break;
        }
        
    }

    public void WhiteSkipTurn()
    {
        Game2.whiteMove = false;
        Game2.blackMove = true;
        Game2.whiteLifePoints += 50;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackSkipTurn()
    {
        Game2.whiteMove = true;
        Game2.blackMove = false;
        Game2.blackLifePoints += 50;
        used = true;
        render.material.color = Color.red;
    }

    public void WhiteCastle()
    {
        Game2.whiteCastle = true;
        Game2.whiteMove = false;
        Game2.blackMove = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackCastle()
    {
        Game2.blackCastle = true;
        Game2.whiteMove = true;
        Game2.blackMove = false;
        used = true;
        render.material.color = Color.red;
    }

    public void TakeBack()
    {
        string lastMove = Game2.previousMoves[Game2.previousMoves.Count - 1];
        Game2.previousMoves.RemoveAt(Game2.previousMoves.Count -1);
        string[] positions = lastMove.Split(',');
        string reversed = positions[1] + "," + positions[0];

        game.TakeBackMove(reversed);

        if(Game2.lastMoveDestroyed[Game2.lastMoveDestroyed.Count - 1] == true)
        {
            game.Spawn(Game2.destroyedPieces[Game2.destroyedPieces.Count - 1], positions[1]);
            Game2.destroyedPieces.RemoveAt(Game2.destroyedPieces.Count -1);
        }
        Game2.lastMoveDestroyed.RemoveAt(Game2.lastMoveDestroyed.Count -1);
        Game2.moves--;
        used = true;
        render.material.color = Color.red;
    }

    public void TakeBackx2()
    {
        TakeBack();
        TakeBack();
    }

    public void TakeBackX3()
    {
        if(Game2.whiteMove)
        {
            Game2.blackLifePoints -= 100;
        }
        else
        {
            Game2.whiteLifePoints -= 100;
        }

        for(int i = 0; i < 3; i++)
        {
            TakeBack();
        }

        
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
        Game2.whiteMove = true;
        Game2.blackMove = false;
        Game2.whiteEnPassantX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackEnPassantTrap()
    {
        Game2.whiteMove = false;
        Game2.blackMove = true;
        Game2.blackEnPassantX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void WhitePromotionTrap()
    {
        Game2.whiteMove = true;
        Game2.blackMove = false;
        Game2.whitePromotionX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackPromotionTrap()
    {
        Game2.whiteMove = false;
        Game2.blackMove = true;
        Game2.blackPromotionX = true;
        used = true;
        render.material.color = Color.red;
    }

    public void WhiteQueenCard()
    {
        Game2.whiteMove = false;
        Game2.blackMove = true;
        game.whiteQ = true;
        used = true;
        render.material.color = Color.red;
    }

    public void BlackQueenCard()
    {
        Game2.whiteMove = true;
        Game2.blackMove = false;
        game.blackQ = true;
        used = true;
        render.material.color = Color.red;
    }








}

