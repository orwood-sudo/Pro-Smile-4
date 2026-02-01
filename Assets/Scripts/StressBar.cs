using EmotionTracker;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StressBar : MonoBehaviour
{
    
    int StressIndex = 0;
    [SerializeField] private GameObject Stress0; //cambiato qua
    [SerializeField] private GameObject Stress1;
    [SerializeField] private GameObject Stress2;
    [SerializeField] private GameObject Stress3;
    [SerializeField] private GameObject Stress4;
    [SerializeField] private GameObject Stress5;

    void Start()
    {
        Stress0.SetActive(true);//cambiato qua
        Stress1.SetActive(false);
        Stress2.SetActive(false);
        Stress3.SetActive(false);
        Stress4.SetActive(false);
        Stress5.SetActive(false);
    }

    public void CheckStress()
    {
        Spawner Spw = FindFirstObjectByType<Spawner>();/*
        Debug.Log("PRINT COMPUTER" +Spw.getEncounterExpression());
        Debug.Log("PRINT EMOTION" +ComputerVisionReceiver.CurrentEmotion.ToString());*/
        
        if (ComputerVisionReceiver.CurrentEmotion.ToString()!=Spw.getEncounterExpression() && ComputerVisionReceiver.CurrentEmotion.ToString()!="Angry")
        {
                // Debug.Log("Aggiungi Stress");
                StressIndex++;
                stressDisplay();
                MaxStressReach();
        }
        else if(ComputerVisionReceiver.CurrentEmotion.ToString()=="Angry")
        {/*togli uno 
            if(StressIndex>0)
            {
                StressIndex--;
                if(StressIndex==0)
                {
                    Stress1.SetActive(false);
                }
                else if(StressIndex==1)
                {
                    Stress2.SetActive(false);
                }
                else if(StressIndex==2)
                {
                    Stress3.SetActive(false);
                }
                else if (StressIndex == 3)
                {
                    Stress4.SetActive(false);
                }
                else if (StressIndex == 4)
                {
                    Stress5.SetActive(false);
                }
            }*/
            if (StressIndex != 0)
            {
                CancellaTutto();
            }
        }

    }
    void stressDisplay()
    {
        if(StressIndex==1)
        {
            Stress1.SetActive(true);
            // Debug.Log("Stress 1 attivato");
        }
        else if(StressIndex==2)
        {
            Stress2.SetActive(true);
        }
        else if(StressIndex==3)
        {
            Stress3.SetActive(true);
        }
        else if (StressIndex == 4)
        {
            Stress4.SetActive(true);
        }
        else if (StressIndex == 5)
        {
            Stress5.SetActive(true);
            
            Invoke("cambiaScena", 5f);
        }
    }
    void cambiaScena()
    {
        SceneManager.LoadScene(6);
    }
    void CancellaTutto()
    {
        Stress1.SetActive(false);
        Stress2.SetActive(false);
        Stress3.SetActive(false);
        Stress4.SetActive(false);
        Stress5.SetActive(false);

        StressIndex = 0;
    }
    void MaxStressReach()
    {
        if(StressIndex==6)
        {
            Debug.Log("Game Over");
        }
    }   
}
/*
                        ___
                     .-'   `'.
                    /         \
                    |         ;
                    |         |           ___.--,
           _.._     |0) ~ (0) |    _.---'`__.-( (_.
    __.--'`_.. '.__.\    '--. \_.-' ,.--'`     `""`
   ( ,.--'`   ',__ /./;   ;, '.__.'`    __
   _`) )  .---.__.' / |   |\   \__..--""  """--.,_
  `---' .'.''-._.-'`_./  /\ '.  \ _.-~~~````~~~-._`-.__.'
        | |  .' _.-' |  |  \  \  '.               `~---`
         \ \/ .'     \  \   '. '-._)
          \/ /        \  \    `=.__`~-.
   N3llo  / /\         `) )    / / `"".`\
    , _.-'.'\ \        / /    ( (     / /
     `--~`   ) )    .-'.'      '.'.  | (
            (/`    ( (`          ) )  '-;
             `      '-;         (-'
*/
