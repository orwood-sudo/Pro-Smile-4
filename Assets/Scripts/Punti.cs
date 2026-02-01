using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Punti : MonoBehaviour
{
    public int punteggio;
    public TextMeshProUGUI testoPunti;
    public void pointGain(int punti)
    {
        punteggio += punti;
        testoPunti.text = punteggio.ToString(); 
    }
    public void rimuoviPunti(int punti)
    {
        punteggio -= punti;
        if (punteggio <= 0)
        {
            //testoPunti.text = "0";
            SceneManager.LoadScene(5);
        }
        else
        {
            testoPunti.text = punteggio.ToString();
        }

    }
    public void UpdatePunteggio(bool GainOrRemove,int punti)
    {
        if (GainOrRemove)
        {
            pointGain(punti);
        }
        else
        {
            rimuoviPunti(punti);
        }
    }
    public string rtS()
    {
        return punteggio.ToString() ;
    }
}
