using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //ADD THIS SCRIPT TO AN EMPTY GAME OBJECT ON THE TITLE SCREEN CALLED "GameManager".

    //Look at the bottom of the code for a VERY long explanation

    public static GameManager Instance;

    [SerializeField] public TextMeshProUGUI congratulationsText;
    [SerializeField] public bool isFirstTimePlaying = true;
    [SerializeField] public Button deleteSaveDataButton;

    public TMP_InputField inputField;
    [HideInInspector] public string playerName;
    public int playerHighScore = 0;



    private void Awake()
    {
        LoadPlayerStats();  //This loads the saved data if any exists.

        congratulationsText.gameObject.SetActive(false);  //This keeps the player name input field hidden until we need it.


        if (Instance != null)
        {
            Destroy(gameObject);
            return;  //We want to exit out of this Awake function by using "return" so we don't create a second Instance = this.
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    //Attach this to the Submit button for inputting the player's name and saving the game.
    public void SetPlayerName()
    {
        playerName = inputField.text;
        isFirstTimePlaying = false;
        SavePlayerStats();  //Saves the game. MAKE SURE THIS IS LAST!
    }



    [System.Serializable]

    class SaveData
    {
        public bool isFirstTimePlaying;
        public string playerName;
        public int playerHighScore;

    }


    public void SavePlayerStats()
    {
        SaveData data = new SaveData();
        data.isFirstTimePlaying = isFirstTimePlaying;
        data.playerName = playerName;
        data.playerHighScore = playerHighScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPlayerStats()
    {
        string path = Application.persistentDataPath + "/savefile.json";


        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            isFirstTimePlaying = data.isFirstTimePlaying;
            playerName = data.playerName;
            playerHighScore = data.playerHighScore;
        }
    }


    public void DeleteSaveData()
    {
        File.Delete(Application.persistentDataPath + "/savefile.json");
    }

    /*
    Since this is a game object we don't want to get destroyed when changing scenes, we need the following code:

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;  //We want to exit out of this Awake function by using "return" so we don't create a second Instance = this.
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    Now for how I changed the player's name and triggered the save function:

    public void SetPlayerName()
    {
        playerName = inputField.text;
        isFirstTimePlaying = false;
        SavePlayerStats();
    }

    playerName is a string variable I'm using to save the string input in the input field.
    I'm also setting isFirstTimePlaying = false so the opening text on the title screen will update when loaded again. 
    Then, I'm calling the SavePlayerStats() function to save everything I want in that function.


    Remember, JSON can only store certain values:
       - a number
       - a string
       - a Boolean (true/false)
       - an array of values
       - Another JSON object

    Now for how I actually saved / loaded / and deleted the data:

    You need "using System.IO;" at the very top. 
    You need "[System.Serializable]" for the whole thing to work.

    This is one of those "don't worry about how or why it works. Just accept it does." kind of things. Input this code:

    class SaveData
    {
    }

    Inside the {} is what we want to save. The variables need to be the EXACT same type and name as the ones you are 
    using in your code.

    For this project, I need to save a boolean, a string, and an interger.
    
    I need to keep track of the isFirstTimePlaying boolean because that affects the text on the title screen.
    I need to keep track of the string playerName so we can remember what the player's name is between sessions.
    I need to keep track of the int playerHighScore so we can remember between sessions who is the bestest ever.

    Adding that information looks like this:

     class SaveData
    {
        public bool isFirstTimePlaying;
        public string playerName;
        public int playerHighScore;

    }




    Now we create a function that actually saves the data. Again, don't worry about why or how. Just follow these steps and
    it will work just fine.

    Create whatever function name you want. I chose "SavePlayerStats". Then add the line:
    SaveData data = new SaveData();

    Now, to save the data you just need to type "data.VARIABLENAME = VARIABLENAME;" I believe this links the variables in
    the SaveData class with the variables at the top of the script which in this case is the GameManager class.

    Once you've declared all your variables, you need to add:

     string json = JsonUtility.ToJson(data);

     File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    So the whole thing looks like this:

    public void SavePlayerStats()
    {
        SaveData data = new SaveData();
        data.isFirstTimePlaying = isFirstTimePlaying;
        data.playerName = playerName;
        data.playerHighScore = playerHighScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }




    Loading is pretty similar, just in reverse. Make a function name and put in this code:

    public void LoadPlayerStats()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

        // VARIABLENAME = data.VARIABLENAME;
        }
    }

    We're filling in the variable values in the GameManager class with the variable data from the SaveData class.
    So that looks like this:

    public void LoadPlayerStats()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            isFirstTimePlaying = data.isFirstTimePlaying;
            playerName = data.playerName;
            playerHighScore = data.playerHighScore;
        }
    }




    Deleting saved data is as easy as adding this function to an OnClick() event on a button.

    public void DeleteSaveData()
    {
        File.Delete(Application.persistentDataPath + "/savefile.json");
    }

     */
}