using System.Drawing;
using EmotionTracker;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MoveOnBeat : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalImage;
    public Sprite normalImage_S;
    public Sprite CorrectReaction;
    public Sprite CorrectReaction_S;
    public Sprite WrongReaction;
    public Sprite WrongReaction_S;
    public Sprite AngryReaction;
    public Sprite AngryReaction_S;

    private SpriteRenderer sr;
    private bool isNormal = true;
    private bool isStatic = false;

    private string currentReation=""; 

    [Header("Settings")]
    public string correctExpression;

    //SoundEffects
    public AudioSource correctSDX;
    public AudioSource angrySDX;
    public AudioSource sadSDX;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        currentReation = "normal";
    }

    // Fondamentale per i Prefab: resetta lo stato ogni volta che l'NPC appare
    void OnEnable()
    {
        isStatic = false;
        isNormal = true;

        if (normalImage != null)
            sr.sprite = normalImage;

        // Iscrizione all'evento del Beat
        BeatManager.OnBeat += HandleBeat;
    }

    void OnDisable()
    {
        // Disiscrizione per evitare errori di memoria
        BeatManager.OnBeat -= HandleBeat;
    }

    void HandleBeat(int beatIndex)
    {
        // Se l'NPC non è bloccato (reazione errata), continua a muoversi a tempo
        if (!isStatic)
        {
            if(currentReation == "correctReaction")
            {
                ChangeSprite(CorrectReaction, CorrectReaction_S);
                Debug.Log("Correct Reactiooon");

            }
            else if(currentReation == "angryReaction")
            {
                ChangeSprite(AngryReaction, AngryReaction_S);
                Debug.Log("Angry Reactiooon");

            }
            else if(currentReation == "sadReaction")
            {
                ChangeSprite(WrongReaction, WrongReaction_S);
                Debug.Log("Sad Reactiooon");

            } else if(currentReation == "normal")
            {
                ChangeSprite(normalImage, normalImage_S);
                Debug.Log("Normal Reactiooon");

            }
            else
            {
                Debug.Log("Orco Duro");
            }
            


        }
    }

    void ChangeSprite(Sprite spriteNormal, Sprite sprite_S)
    {
        sr.sprite = isNormal ? sprite_S : spriteNormal;
        isNormal = !isNormal;
    }

    public void ReactionNpc()
    {

        Punti pointManager = FindFirstObjectByType<Punti>();
        string detectedEmotion = ComputerVisionReceiver.CurrentEmotion.ToString();

        if (pointManager == null)
        {
            Debug.LogError("Punti manager not found in scene!");
            return;
        }

        if (detectedEmotion == correctExpression)
        {
            pointManager.UpdatePunteggio(true, 500);
            Debug.Log(gameObject.name + ": Correct! Detected: " + detectedEmotion);

            currentReation = "correctReaction";
            correctSDX.Play();
        }
        else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Angry)
        {
            pointManager.UpdatePunteggio(false, 300);
            Debug.Log(gameObject.name + ": Angry Reaction");
            currentReation = "angryReaction";
            angrySDX.Play();
        }
        else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Sad)
        {
            pointManager.UpdatePunteggio(false, 100);
            Debug.Log(gameObject.name + ": Sad Reaction");
            currentReation = "sadReaction";
            sadSDX.Play();
        }
        else
        {
            pointManager.UpdatePunteggio(false, 100);
            Debug.Log(gameObject.name + ": Epsressione Sbagliata. Aspettato: " + correctExpression + " Però hai fatto: " + detectedEmotion);
            currentReation = "sadReaction";
            sadSDX.Play();
        }
    }

    public string GetCorrectExpression()
    {
        return correctExpression;
    }
}
