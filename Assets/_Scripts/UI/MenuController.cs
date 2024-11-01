using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
/*    [SerializeField] private GameObject _optionsMenu;*/
    [SerializeField] private GameObject _bulletsMenu;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void BulletMenu()
    {
        _mainMenu.SetActive(false);
        _bulletsMenu.SetActive(true);
    }
/*    public void OptionsMenu()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }*/
    public void BackToMainMenu()
    {
        _mainMenu.SetActive(true);
/*        _optionsMenu.SetActive(false);*/
        _bulletsMenu.SetActive(false);
    }
}
