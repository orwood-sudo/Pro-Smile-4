using UnityEngine;
using EmotionTracker;
using UnityEngine.SceneManagement;

public class BlinkToNextScene : MonoBehaviour
{
    public  bool readyToBlink = false;

    private int currentSceneIndex = 0; 

    public float timeToWait = 2.0f;
    private float timer = 0f;

    private static bool introCutScenePlayed = false;  //A tempo solo se è la cutscene iniziale. 
    

    void Start()
    {
        readyToBlink = false;
        timer = 0f;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (!readyToBlink && !introCutScenePlayed)
        {
            
            timer += Time.deltaTime;
            if (timer >= timeToWait)
            {
                readyToBlink = true;
                Debug.Log("Ready to blink!!");
            }

        }

        if (readyToBlink && ComputerVisionReceiver.IsBlinked)
        {
            currentSceneIndex++;
            readyToBlink = false;

            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(currentSceneIndex);
                Debug.Log("Scene Changed");
                timer = 0;
                introCutScenePlayed = true; 
            }
            else
            {
                // SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings -1 );
                // Qui logica per vedere Quale finale fare
                Debug.Log("Scegli finale, vai in BlinkToNextScene");
            }
        }
    }
    public void ReadyToBlink()
    {
        readyToBlink = true; 
        Debug.Log("Ready to Blink TUREEE");
    }
}
