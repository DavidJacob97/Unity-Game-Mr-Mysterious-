using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public void exitGame()
    {
        Application.Quit();
    }

    public void levelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelSelection"));
    }

    public void start()
    {
        SceneManager.LoadScene("Level1");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));
        Debug.Log("Lets go");
    }

    public void goBack()
    {
        SceneManager.LoadScene("StartMenu");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartMenu"));
    }

    public void loadLevel()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(EventSystem.current.currentSelectedGameObject.name)); 
    }
}
