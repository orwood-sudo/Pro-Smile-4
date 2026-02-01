using EmotionTracker;
using TMPro;
using UnityEngine;


public class Prova : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI emotionsText;
    [SerializeField] TextMeshProUGUI blinkText;
    [SerializeField] TextMeshProUGUI SP;
    public int point;
    int score = 0;
    //bool prova;
    private void Start()
    {
        point = 1000;
        SP.text = point.ToString();
    }
    // Update is called once per frame
    void Update()
    {

        if (ComputerVisionReceiver.IsBlinked) blinkText.text = "Blink: Blinked";
        else blinkText.text = "Blink: No blinking detected";
        if (ComputerVisionReceiver.CurrentEmotion != EmotionType.Neutral)
        {
            if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Fear) emotionsText.text = "Fear";
            else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Angry) emotionsText.text = "Angry";
            else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Happy) emotionsText.text = "Happy";
            else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Sad) emotionsText.text = "Sad";
        }
        else emotionsText.text = "neutral";


        /* if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Happy)
         {
             if (prova)
             {

                 pointGain(score);
             }
         }
         else if (ComputerVisionReceiver.CurrentEmotion !=EmotionType.Happy)
         {
             prova = true;
         }*/
    }

}
