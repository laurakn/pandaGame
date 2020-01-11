using UnityEngine;

public class Pause : MonoBehaviour  {
    [SerializeField] 
    private GameObject pausePanel;
    void Start()
    {
        Debug.Log("Pause start");
        pausePanel.SetActive(false);
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("p");
            if (!pausePanel.activeSelf) {
                PauseGame();
            }
            else ContinueGame();   
        } 
     }
    private void PauseGame() {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        print("active");
        //Disable scripts that still work while timescale is set to 0
    } 
    private void ContinueGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        print("inactive");
        //enable the scripts again
    }
}