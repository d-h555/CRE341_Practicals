using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance of the GameManager

    public bool isGameOver = false;
    
    public GameObject mainMenu;
    public GameObject winScreen;
    public GameObject loseScreen;
   
   MazeBuilder mazeGen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        isGameOver = false; // Reset game over state each frame

        if (mainMenu != null) mainMenu.SetActive(true); // Show main menu

        if (winScreen !=null) winScreen.SetActive(false); // Hide win screen
        if (loseScreen !=null) loseScreen.SetActive(false); // Hide lose screen
    }

  public void StartGame()
    {   
        Time.timeScale = 1f;
        Debug.Log("Start Game button clicked!");
        isGameOver = false; // Reset game over state
        if (mainMenu != null) mainMenu.SetActive(false); // Hide main menu
        Debug.Log("Game Started!"); // Log game start message
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
        Debug.Log("Game Quit!"); // Log game quit message
    }
    public void WinGame()
    {
        isGameOver = true; // Set game over state to true
        if (winScreen !=null) winScreen.SetActive(true); // Show win screen
        Debug.Log("You Win!"); // Log win message
    }

    public void LoseGame()
    {
        isGameOver = true; // Set game over state to true
        if (loseScreen !=null) loseScreen.SetActive(true); // Show lose screen
        Debug.Log("You Lose!"); // Log lose message
    }

      public void ReturnToMainMenu()
    {
        isGameOver = false; // Reset game over state
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        if (mainMenu != null) mainMenu.SetActive(true); // Show the main menu
        if (winScreen != null) winScreen.SetActive(false); // Hide the win screen
        if (loseScreen != null) loseScreen.SetActive(false); // Hide the lose screen
        Debug.Log("Returning to Main Menu...");
    }


    public void CollectCheese()
    {
        mazeGen.collectedCheeseCount++; // Increment collected cheese count
        Debug.Log($"Collected Cheese:  {mazeGen.collectedCheeseCount}/{mazeGen.totalCheeseCount}"); // Log the collected cheese count

        if (mazeGen.collectedCheeseCount >= mazeGen.totalCheeseCount) // Check if all cheese is collected
        {
            WinGame(); // Call win game method if all cheese is collected
        }
        {
            WinGame(); // Call win game method if all cheese is collected
        }
    }
  
  public void OnClick()
{
    Debug.Log("Button clicked!");
}

}
