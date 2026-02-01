using UnityEngine;
using UnityEngine.SceneManagement;
public class CambiaScena : MonoBehaviour
{
    public void Bottone_Cambia_Scena()
    {
        SceneManager.LoadScene("1_Park");
    }

    public void GoScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
