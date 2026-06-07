using Oculus.Interaction.Locomotion;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject startMenu;
    public GameObject instructions;
    public GameObject exitMenu;
    public GameObject playerController;

    void Start()
    {
        playerController.SetActive(false);        
    }

    public void Play()
    {
        playerController.SetActive(true);
        startMenu.SetActive(false);
        canvas.SetActive(false);
        exitMenu.SetActive(true);
    }

    public void ToggleInstructions()
    {
        startMenu.SetActive(!startMenu.activeInHierarchy);
        instructions.SetActive(!instructions.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
