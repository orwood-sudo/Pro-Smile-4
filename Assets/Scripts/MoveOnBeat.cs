using EmotionTracker;
using System.Drawing;
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
    public Punti boh;
    private SpriteRenderer sr;
    private bool isNormal = true;
    private bool isStatic = false;
    [Header("Settings")]
    public string correctExpression;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Fondamentale per i Prefab: resetta lo stato ogni volta che l'NPC appare
    void OnEnable()
    {
        // Reset delle variabili di stato
        isStatic = false;
        isNormal = true;

        // Reset dello sprite iniziale
        if (normalImage != null)
            sr.sprite = normalImage;

        // Iscrizione all'evento del Beat
        BeatManager.OnBeat += HandleBeat;
    }

    void OnDisable()
    {
        // Disiscrizione per evitare errori di memoria quando il prefab sparisce
        BeatManager.OnBeat -= HandleBeat;
    }

    void HandleBeat(int beatIndex)
    {
        // Se l'NPC ha già reagito (male), non si muove più
        if (!isStatic)
        {
            ChangeSprite();
        }
    }

    void ChangeSprite()
    {
        sr.sprite = isNormal ? normalImage_S : normalImage;
        isNormal = !isNormal;
    }

    public void ReactionNpc()
    {
        // Se è già statico, ignoriamo ulteriori input per questo specifico prefab
        if (isStatic) return;
        Punti pointManager=FindFirstObjectByType<Punti>();
        string detectedEmotion = ComputerVisionReceiver.CurrentEmotion.ToString();

        if (detectedEmotion == correctExpression)
        {
            sr.sprite = CorrectReaction;
            pointManager.UpdatePunteggio(true, 500);
            Debug.Log(gameObject.name + ": Correct!");
            // Opzionale: isStatic = true; se vuoi che si fermi anche quando indovina
        }
        else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Angry)
        {
            isStatic = true;
            pointManager.UpdatePunteggio(false, 300);
            sr.sprite = AngryReaction;
            Debug.Log(gameObject.name + ": Angry Reaction");
        }
        else
        {
            isStatic = true;
            pointManager.UpdatePunteggio(false, 100);
            sr.sprite = WrongReaction;
            Debug.Log(gameObject.name + ": Wrong Reaction");
        }
    }
    public string GetCorrectExpression()//modificare in string
    {
        return correctExpression;
    }


    
}