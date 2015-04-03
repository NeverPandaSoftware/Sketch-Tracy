using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public Transform CamPositionStart;
    public Transform CamPositionLoad;

    private bool inLoadMenu = false;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            NewGame();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            LoadGameMenu();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Options();
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
	}

    void NewGame()
    {
        DataController.Instance.NewGame();
    }

    void LoadGameMenu()
    {
        if (!inLoadMenu)
        {
            Camera.main.transform.position = CamPositionLoad.position;
            Camera.main.transform.rotation = CamPositionLoad.rotation;
            inLoadMenu = true;
        }
        else
        {
            Camera.main.transform.position = CamPositionStart.position;
            Camera.main.transform.rotation = CamPositionStart.rotation;
            inLoadMenu = false;
        }
    }

    public void LoadGame(int slotIndex)
    {
        DataController.Instance.LoadGame(slotIndex);
    }

    void Options()
    {

    }

    void Quit()
    {
        Application.Quit();
    }
}
