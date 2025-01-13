using UnityEngine;
public class GameManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private RSE_Quit rseQuit;

    private void OnEnable() => rseQuit.action += Quit;
    private void OnDisable() => rseQuit.action -= Quit;
    
    private void Quit()
    {
        Application.Quit();
    }
}