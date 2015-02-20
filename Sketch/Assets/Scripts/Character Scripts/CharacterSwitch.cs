using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Character Scripts/Character Switch")]
public class CharacterSwitch : MonoBehaviour
{
    public GameObject sketchPreFab;
    public GameObject tracyPreFab;
    public GameObject teamPreFab;

    public GameObject sketchObject;
    public GameObject tracyObject;
    public GameObject teamObject;
    public GameObject cameraObject;

    private PlayerController sketch;
    private PlayerController tracy;
    private CameraFollow cam;
    private bool sketchActiveStatus;
    private bool tracyActiveStatus;

    private bool teamedUp = false;
    public float teamDistance = 0.75f;

    // Use this for initialization
    void Start()
    {
        sketch = sketchObject.GetComponent<PlayerController>();
        tracy = tracyObject.GetComponent<PlayerController>();
        cam = cameraObject.GetComponent<CameraFollow>();
        GameManager.Instance.SetCharacterState(CharacterState.Sketch);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchCharacter"))
        {
            SwitchCharacters();
        }

        if (Input.GetButtonDown("TeamUp"))
        {
            if (!teamedUp)
            {
                if (CanTeamUp())
                    TeamUp();
            }
            else
            {
                UnTeam();
            }
        }
    }

    private bool CanTeamUp()
    {
        if (Mathf.Abs(Vector3.Distance(sketchObject.transform.position, tracyObject.transform.position)) < teamDistance)
            return true;
        else
            return false;
    }

    void TeamUp()
    {
        teamedUp = true;

        Vector3 spawnLocation = sketchObject.transform.position;
        Quaternion rotation = sketchObject.transform.rotation;

        Destroy(sketchObject);
        Destroy(tracyObject);

        teamObject = (GameObject)Instantiate(teamPreFab, spawnLocation, rotation);
        teamObject.name = "Team";

        GameManager.Instance.SetCharacterState(CharacterState.Team);

        ChangeCameraTarget(teamObject);
    }

    void UnTeam()
    {
        Vector3 spawnLocation = teamObject.transform.position;
        Quaternion rotation = teamObject.transform.rotation;

        Destroy(teamObject);

        sketchObject = (GameObject)Instantiate(sketchPreFab, spawnLocation - new Vector3(0.5f, 0, 0), rotation);
        sketchObject.name = "Sketch";
        sketch = sketchObject.GetComponent<PlayerController>();

        tracyObject = (GameObject)Instantiate(tracyPreFab, spawnLocation + new Vector3(0.5f, 0, 0), rotation);
        tracyObject.name = "Tracy";
        tracy = tracyObject.GetComponent<PlayerController>();

        GameManager.Instance.SetCharacterState(CharacterState.Sketch);

        teamedUp = false;

        ChangeCameraTarget(sketchObject);
    }

    void SwitchCharacters()
    {
        CharacterState curCharacterState = GameManager.Instance.GetCharacterState();

        if (curCharacterState == CharacterState.Tracy)
        {
            GameManager.Instance.SetCharacterState(CharacterState.Sketch);
            ChangeCameraTarget(sketchObject);
        }
        else if (curCharacterState == CharacterState.Sketch)
        {
            GameManager.Instance.SetCharacterState(CharacterState.Tracy);
            ChangeCameraTarget(tracyObject);
        }
    }

    void ChangeCameraTarget(GameObject newTarget)
    {
        cam.SetTarget(newTarget.transform);
    }
}

