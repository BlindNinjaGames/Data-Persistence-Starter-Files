using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//You need this so the code so the Exit button will work.
#if UNITY_EDITOR
using UnityEditor;
#endif

//THIS SCRIPT SHOULD GO ON THE CANVAS ON THE TITLE SCREEN THAT HOLDS THE TITLE, SAVE / QUIT BUTTONS, AND OPENING MESSAGE TEXT

public class MenuHandler : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] TextMeshProUGUI openingMessage;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Change the opening message at the bottom of the screen depnding on whether there save data or not.
        //This was easiest to control using a boolean. 

        if (gameManager.isFirstTimePlaying == false)
        {
            openingMessage.text = gameManager.playerName + " has the high score of: " + gameManager.playerHighScore;

        }

        else
        {
            openingMessage.text = "You are the first to play!\nNow is the time to set the standard!";
        }
    }

    public void StartNew()
    {
        gameManager.deleteSaveDataButton.gameObject.SetActive(false);  //This just hides the delete button when playing the game.

        SceneManager.LoadScene("main");  //Make sure the name of the scene matches EXACTLY
    }

    public void Exit()
    {
        //Use this to be able to quit the game while in Unity.
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }



}
