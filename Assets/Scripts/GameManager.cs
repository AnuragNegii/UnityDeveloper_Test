using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager: MonoBehaviour{
    public static GameManager Instance{get; private set;}

    [SerializeField] private int noOfBoxes = 5;
    [SerializeField] private float totalTime = 1;  
    [SerializeField] private TextMeshProUGUI tmpText;
    [SerializeField] private TextMeshProUGUI boxRemaining;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    private void OnEnable(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start(){
        PlayerScript.OnBoxDestroyed += Player_OnBoxDestroyed;
        PlayerScript.PlayerDied += Player_Dead;
        tmpText.text = "Time:"+(int)totalTime;
        boxRemaining.text = "Box: "+ noOfBoxes;
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f; // ⏵ Resume time
    }

    private void Update(){
        totalTime -= Time.deltaTime;
        if (totalTime <= 0 && noOfBoxes > 0){
            totalTime = 0;
            Player_Dead();
        }
        
//ui element call
        tmpText.text = "Time: "+(int)totalTime;
        boxRemaining.text = "Box: "+ noOfBoxes;
    }

    private void Player_OnBoxDestroyed(){
        noOfBoxes -= 1;
    }

    private void Player_Dead(){
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ⏸ Pause the game

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable(){
        PlayerScript.OnBoxDestroyed -= Player_OnBoxDestroyed;
        PlayerScript.PlayerDied -= Player_Dead;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
