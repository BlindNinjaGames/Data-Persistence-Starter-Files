using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainManager : MonoBehaviour
{
    //ADD THIS SCRIPT TO AN EMPTY GAME OBJECT CALLED "MainManager" ON THE GAMEPLAY SCREEN.

    public Brick brickPrefab;
    public int lineCount = 6;
    public Rigidbody ball;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI firstTimeText;

    GameManager gameManager;


    bool isGameStarted = false;
    int point;

    bool isGameOver = false;



    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        HighScoreText();  //This will update the high score text whenever the scene loads.
    }

    private void Update()
    {
        if (!isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                isGameStarted = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameManager.congratulationsText.gameObject.SetActive(false);  //Turn off the player name input field.
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        this.point += point;
        scoreText.text = $"Score : {this.point}";
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);

        //When the game ends, check to see which number is higher. If the score is higher than the currently saved high score
        //then activate the player name input field. Also, change the high score to the point score.

        if (point > gameManager.playerHighScore)
        {
            gameManager.congratulationsText.gameObject.SetActive(true);
            gameManager.playerHighScore = point;
        }
    }

    //This changes the high score text at the top of the screen based on whether a save file exists or not which is
    //controlled by the boolean "isFirstTimePlaying". I used separate text boxes so the messages would line up correctly.
    public void HighScoreText()
    {
        if (gameManager.isFirstTimePlaying == false)
        {
            highScoreText.gameObject.SetActive(true);
            firstTimeText.gameObject.SetActive(false);
        }

        else
        {
            highScoreText.gameObject.SetActive(false);
            firstTimeText.gameObject.SetActive(true);
        }

        if (gameManager.isFirstTimePlaying == false)
        {
            highScoreText.text = gameManager.playerName + " has the high score of: " + gameManager.playerHighScore;
        }

        else
        {
            firstTimeText.text = "Show them how it's done!";
        }

    }

    //Add this to a main menu button. 
    public void MainMenu()
    {
        gameManager.congratulationsText.gameObject.SetActive(false);  //turns off the player input field.
        gameManager.deleteSaveDataButton.gameObject.SetActive(true);  //brings the Delete Save button back.

        SceneManager.LoadScene("TitleScreen");
    }


}
