using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SimulationTests
{
    public IEnumerator simulation_test1()
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
        
        game.Move("a2,a3");
        game.Move("a7,a6");
        game.Move("a3,a4");

        bool b = game.isOccupied("a2");

        Assert.AreEqual(false, b);

        yield return null;
    }

    public IEnumerator simulation_test2()
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
        
        game.Move("a2,a3");
        game.Move("a7,a6");
        game.Move("a3,a4");

        Assert.AreEqual(3, Game.moves);

        yield return null;
    }

    public IEnumerator simulation_test3()
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
        
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");

        Assert.AreEqual(0, Game.moves);

        yield return null;
    }

    public IEnumerator simulation_test4()
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
        
        game.DestroyPiece("a1");

        Assert.AreEqual(false, game.isOccupied("a1"));

        yield return null;
    }

    public IEnumerator simulation_test5()
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
        
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("f7,a2");
        game.Move("a2,a3");

        Assert.AreEqual(true, Game.blackMove);

        yield return null;
    }
}
