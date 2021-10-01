using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public string gameSceneToLoad;
    //private PlayerControls playerControls;
    [SerializeField] private GameObject playButton;

    private void Start()
    {
        //EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(gameSceneToLoad);
    }

    public void QuitToDeskTop()
    {
        Application.Quit();
    }
}
