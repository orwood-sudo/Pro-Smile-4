using EmotionTracker;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimationscripLogic : MonoBehaviour
{
    private Prova feeling;
    [Header("Impostazioni Sprite")]
        public Sprite[] frames; 

        private Image _uiImage;

        void Awake()
        {
            _uiImage = GetComponent<Image>();
        }
        void Start()
        {
            if (frames != null && frames.Length > 0)
            {
                _uiImage.sprite= frames[0];
            }
        }
    void Update()
    {
        SetFrame(0);
    }
    public void SetFrame(int index)
        {
            if (frames == null || frames.Length == 0) return;

            int safeIndex = Mathf.Clamp(index, 0, frames.Length - 1);
            _uiImage.sprite = frames[safeIndex];

            if(ComputerVisionReceiver.CurrentEmotion == EmotionType.Happy)
            {
                _uiImage.sprite = frames[1];
            }
            else if(ComputerVisionReceiver.CurrentEmotion == EmotionType.Sad)
            {
                _uiImage.sprite = frames[2];
            }
            else if(ComputerVisionReceiver.CurrentEmotion == EmotionType.Angry)
            {
                _uiImage.sprite = frames[4];
            }
        else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Fear)
        {
            _uiImage.sprite = frames[3];
        }
        else if (ComputerVisionReceiver.CurrentEmotion == EmotionType.Neutral)
        {
            _uiImage.sprite = frames[0];
        }
    }
    
}