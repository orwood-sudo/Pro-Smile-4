using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

namespace EmotionTracker // Ho lasciato un namespace generico, cambialo pure
{
    public class ComputerVisionReceiver : MonoBehaviour
    {
        public static bool IsBlinked { get; private set; }
        public static EmotionType CurrentEmotion { get; private set; } = EmotionType.Neutral;
        public static float RawBlinkValue { get; private set; }

        [Header("Configurazione Processo")]
        [SerializeField] private int _cameraId = 0;

        [Header("Calibrazione Blink")]
        [SerializeField] private float _blinkingThreshold = 0.5f;
        [SerializeField] private float _eyeOffset = 0f;
        [SerializeField] private float _sensitivity = 1.0f;

        [Header("Refresh Emozioni (Debounce)")]
        [Tooltip("Quanti messaggi uguali servono per cambiare emozione (es. 5-10)")]
        [SerializeField] private float _framesToConfirm = 8;

        private EmotionType _lastDetectedEmotion = EmotionType.Neutral;
        private int _confirmationCounter = 0;

        private Process _pythonProcess;
        private Thread _readThread;
        private bool _isRunning;

        private readonly Dictionary<string, EmotionType> _emotions = new Dictionary<string, EmotionType>
        {
            { "happy", EmotionType.Happy },
            { "sad", EmotionType.Sad },
            { "angry", EmotionType.Angry },
            { "neutral", EmotionType.Neutral },
            { "surprise", EmotionType.Fear },
            { "fear", EmotionType.Fear }
        };

        private void OnEnable() => StartPython();
        private void OnDisable() => EndPython();

        public void CalibrateNeutralFace()
        {
            _eyeOffset = -RawBlinkValue;
            UnityEngine.Debug.Log($"Calibrazione completata. Offset: {_eyeOffset}");
        }

        private void StartPython()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "cv.exe");

            if (!File.Exists(path))
            {
                UnityEngine.Debug.LogError($"cv.exe non trovato!");
                return;
            }

            _pythonProcess = new Process();
            _pythonProcess.StartInfo.WorkingDirectory = Application.streamingAssetsPath;
            _pythonProcess.StartInfo.FileName = path;
            _pythonProcess.StartInfo.Arguments = _cameraId.ToString();
            _pythonProcess.StartInfo.UseShellExecute = false;
            _pythonProcess.StartInfo.CreateNoWindow = true;
            _pythonProcess.StartInfo.RedirectStandardOutput = true;

            try
            {
                _pythonProcess.Start();
                _isRunning = true;

                var culture = CultureInfo.InvariantCulture;
                _readThread = new Thread(() => ReadPythonOutput(culture));
                _readThread.IsBackground = true;
                _readThread.Start();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Errore: {e.Message}");
            }
        }

        private void ReadPythonOutput(CultureInfo culture)
        {
            using (StreamReader reader = _pythonProcess.StandardOutput)
            {
                while (_isRunning && !_pythonProcess.HasExited)
                {
                    string text = reader.ReadLine()?.Trim().ToLower();
                    if (string.IsNullOrEmpty(text)) continue;

                    // LOGICA DEBOUNCE EMOZIONI
                    if (_emotions.TryGetValue(text, out EmotionType detected))
                    {
                        if (detected == _lastDetectedEmotion)
                        {
                            _confirmationCounter++;
                        }
                        else
                        {
                            _lastDetectedEmotion = detected;
                            _confirmationCounter = 0;
                        }

                        // Applica l'emozione solo se confermata per X volte
                        if (_confirmationCounter >= _framesToConfirm)
                        {
                            CurrentEmotion = detected;
                        }
                    }
                    // LOGICA BLINK
                    else if (float.TryParse(text, NumberStyles.Any, culture, out float val))
                    {
                        RawBlinkValue = val;
                        float calibrated = (val + _eyeOffset) * _sensitivity;
                        IsBlinked = calibrated > _blinkingThreshold;
                    }

                    Thread.Sleep(1);
                }
            }
        }

        private void EndPython()
        {
            _isRunning = false;
            if (_pythonProcess != null && !_pythonProcess.HasExited)
            {
                _pythonProcess.Kill();
                _pythonProcess.Dispose();
            }
            if (_readThread != null && _readThread.IsAlive) _readThread.Abort();
        }
    }

    public enum EmotionType { Happy, Sad, Angry, Neutral, Fear }
}