using Oculus.Interaction.Locomotion;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject startMenu;
    public GameObject instructions;
    public GameObject exitMenu;
    public GameObject teleportInteractor;
    public FirstPersonLocomotor continuousLocomotor;

    public void Play()
    {
        teleportInteractor.SetActive(true);
        continuousLocomotor.enabled = true;
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
