using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadSceneBir()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadSceneÝki()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadSceneUc()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSceneDort()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    /* public void LoadPause()
     {
         SceneManager.LoadScene(3);
     } */

    public void QuitGame()
    {
        Application.Quit();//konsola yazýyor
        Debug.Log("Oyun kapatýldý.");
    }
}