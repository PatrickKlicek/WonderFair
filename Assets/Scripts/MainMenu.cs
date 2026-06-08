using Oculus.Interaction.Locomotion;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject startMenu;
    public GameObject instructions;
    public GameObject exitMenu;
    public GameObject playerController;
    public TeleportInteractable teleportScript;

    void Start()
    {
        playerController.SetActive(false);
        teleportScript.AllowTeleport = false;
    }

    public void Play()
    {
        playerController.SetActive(true);
        teleportScript.AllowTeleport = true;
        startMenu.SetActive(false);
        menu.SetActive(false);
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
