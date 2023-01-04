using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CardTests : MonoBehaviour
{


    [Test]
    public void GameTestsSimplePasses()
    {

    }

    [UnityTest]
    public IEnumerator isValidSquare_test()
    {
        SceneManager.LoadScene("TitleScreen");

        yield return new WaitForSeconds(2);
        
        GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");


        MainMenu mm = menu.AddComponent<MainMenu>();

        mm.StartButton();
        mm.SinglePlayerButton();
        mm.Level1();
        mm.WhiteButton();

        yield return new WaitForSeconds(2);
        GameObject obj = GameObject.FindGameObjectWithTag("GameController");
        Game2 game = obj.AddComponent<Game2>();
        Debug.Log(game.isValidSquare("a2"));

        bool b = game.isValidSquare("a2");

        Assert.AreEqual(true, b);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SkipTurn_test()
    {
        SceneManager.LoadScene("TitleScreen");

        yield return new WaitForSeconds(2);
        
        GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");


        MainMenu mm = menu.AddComponent<MainMenu>();

        mm.StartButton();
        mm.SinglePlayerButton();
        mm.Level1();
        mm.WhiteButton();

        yield return new WaitForSeconds(2);
        GameObject c = GameObject.FindGameObjectWithTag("Card");
        Card2 card = c.AddComponent<Card2>();

        card.WhiteSkipTurn();

        Assert.AreEqual(false, Game2.whiteMove);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Castle_test()
    {
        SceneManager.LoadScene("TitleScreen");

        yield return new WaitForSeconds(2);
        
        GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");


        MainMenu mm = menu.AddComponent<MainMenu>();

        mm.StartButton();
        mm.SinglePlayerButton();
        mm.Level1();
        mm.WhiteButton();

        yield return new WaitForSeconds(2);
        GameObject c = GameObject.FindGameObjectWithTag("Card");
        Card2 card = c.AddComponent<Card2>();

        card.WhiteCastle();

        Assert.AreEqual(true, Game2.whiteCastle);

        yield return null;
    }

    [UnityTest]
    public IEnumerator EnPassantTrap_test()
    {
        SceneManager.LoadScene("TitleScreen");

        yield return new WaitForSeconds(2);
        
        GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");


        MainMenu mm = menu.AddComponent<MainMenu>();

        mm.StartButton();
        mm.SinglePlayerButton();
        mm.Level1();
        mm.WhiteButton();

        yield return new WaitForSeconds(2);
        GameObject c = GameObject.FindGameObjectWithTag("Card");
        Card2 card = c.AddComponent<Card2>();

        card.BlackEnPassantTrap();

        Assert.AreEqual(true, Game2.blackEnPassantX);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PromotionTrap_test()
    {
        SceneManager.LoadScene("TitleScreen");

        yield return new WaitForSeconds(2);
        
        GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");


        MainMenu mm = menu.AddComponent<MainMenu>();

        mm.StartButton();
        mm.SinglePlayerButton();
        mm.Level1();
        mm.WhiteButton();

        yield return new WaitForSeconds(2);
        GameObject c = GameObject.FindGameObjectWithTag("Card");
        Card2 card = c.AddComponent<Card2>();

        card.BlackPromotionTrap();

        Assert.AreEqual(true, Game2.blackPromotionX);

        yield return null;
    }

    [UnityTest]
    public IEnumerator QueenPromotion_test()
    {
        SceneManager.LoadScene("TitleScreen");

        yield return new WaitForSeconds(2);
        
        GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");


        MainMenu mm = menu.AddComponent<MainMenu>();

        mm.StartButton();
        mm.SinglePlayerButton();
        mm.Level1();
        mm.WhiteButton();

        yield return new WaitForSeconds(2);
        GameObject c = GameObject.FindGameObjectWithTag("Card");
        Card2 card = c.AddComponent<Card2>();

        GameObject obj = GameObject.FindGameObjectWithTag("GameController");
        Game2 game = obj.AddComponent<Game2>();

        card.WhiteQueenCard();

        Assert.AreEqual(true, game.whiteQ);

        yield return null;
    }



    


}
