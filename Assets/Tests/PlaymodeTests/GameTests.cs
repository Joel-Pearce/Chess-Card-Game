using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameTests : MonoBehaviour
{


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
    public IEnumerator makeVector_test()
    {

        Vector3 v1 = new Vector3(-2,-2,-2);

        Vector2 vector2 = new Vector2(-2,-2);

        Vector3 v2 = Game2.makeVector(vector2);

        Assert.AreEqual(v1, v2);

        yield return null;
    }

    [UnityTest]
    public IEnumerator isOccupied_test()
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

        Assert.AreEqual(false, game.isOccupied("a3"));

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanCastle_test()
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

        Assert.AreEqual(false, game.CanCastle("a1", "e1"));

        yield return null;
    }

    [UnityTest]
    public IEnumerator IsEnPassant_test()
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

        Assert.AreEqual(false, game.IsEnPassant("a1", "e1"));

        yield return null;
    }

    [UnityTest]
    public IEnumerator findSquare_test()
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

        Assert.AreEqual(false, game.IsEnPassant("b2", game.findSquare("a1", 1, 1)));

        yield return null;
    }



}
