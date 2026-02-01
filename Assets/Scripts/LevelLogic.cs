using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro; // Add this for TextMeshPro

public class LevelLogic : MonoBehaviour
{
    public AudioSource phoneVibration; 
    public GameObject polpo;
     public GameObject Camaleonte;
    public AudioSource currentMusic;
    public GameObject testoTelefono; 
    public GameObject boxDialogue;

    public TextMeshProUGUI readyToBlinkText;

    // Typewriter settings
    public float typingSpeed = 0.05f;
    private TextMeshProUGUI textComponent; 
    private string fullText;

    void Awake()
    {
        
        // Cache the text component and the full message
        textComponent = testoTelefono.GetComponent<TextMeshProUGUI>();
        fullText = textComponent.text;
    }

    public void StartSound()
    {
        phoneVibration.Play();
    }

    public void StopCurrentMusic()
    {
        currentMusic.Pause();
    }

    public void ShowDialogueBox()
    {
        testoTelefono.SetActive(true);
        boxDialogue.SetActive(true);
        // Start the typewriter effect every time the box is shown
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = ""; // Clear existing text
        foreach (char c in fullText.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        StartCoroutine(FinalScene(0.5f)); 
    }

    public void HideDialogueBox()
    {
        testoTelefono.SetActive(false);
    }

    private IEnumerator FinalScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // 1. Ensure the object is active first
        if (!boxDialogue.activeSelf) 
        {
            boxDialogue.SetActive(true);
        }

        // 2. Safely grab the animator and play
        Animator anim = boxDialogue.GetComponent<Animator>();
        if (anim != null) 
        {
            anim.Play("Animation_Box_Dialogue");
        }
        else 
        {
            Debug.LogWarning("No Animator found on boxDialogue!");
        }
        StartCoroutine(ReadyToBlinkDelay(1f));
    }
    public void DoFinalScene(int index)
    {
        switch(index)
        {
            case 1:
            boxDialogue.SetActive(true);
            StopCurrentMusic();
            StartSound();
            ShowDialogueBox();
            break;

            case 2:
            //logica studio
            boxDialogue.SetActive(true);
            StopCurrentMusic();
            polpo.SetActive(true );
            ShowDialogueBox();
            break;
            case 3:
            //logica gala
            boxDialogue.SetActive(true);
            StopCurrentMusic();
            Camaleonte.SetActive(true );
            ShowDialogueBox();
            break;
            case 4:
                //logica finale
             SceneManager.LoadScene(4);
             break;
            
            default:
                break;
        }
    }



    private IEnumerator ReadyToBlinkDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        BlinkToNextScene manager = FindFirstObjectByType<BlinkToNextScene>();
        if (manager != null) manager.ReadyToBlink();

        readyToBlinkText.text = "Chiudi gli occhi per continuare";
    }

    
}