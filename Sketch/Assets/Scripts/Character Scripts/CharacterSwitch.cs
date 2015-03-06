using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Character Scripts/Character Switch")]
public class CharacterSwitch : MonoBehaviour
{
    #region Variables

    [System.Serializable]
    public class CharacterPreFabs
    {
        public GameObject Sketch;
        public GameObject Tracy;
        public GameObject Team;
    }
    public CharacterPreFabs Characters;

    private GameObject sketchObject;
    private GameObject tracyObject;
    private GameObject teamObject;
    private GameObject cameraObject;

    private Camera cam;

    private bool teamedUp = false;
    public float teamDistance = 0.75f;

    #endregion

    #region Initialization

    // Use this for initialization
    void Awake()
    {
        GameManager.Instance.SetCharacterState(CharacterState.Sketch);
        cam = Camera.main;
    }

    #endregion

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchCharacter") && BothCharactersExist())
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

    #endregion

    #region Boolean Helpers

    private bool BothCharactersExist()
    {
        sketchObject = GameObject.FindGameObjectWithTag("Sketch");
        tracyObject = GameObject.FindGameObjectWithTag("Tracy");

        if (sketchObject != null && tracyObject != null)
            return true;
        else
            return false;
    }

    private bool CanTeamUp()
    {
        if (BothCharactersExist())
        {
            if (Mathf.Abs(Vector3.Distance(sketchObject.transform.position, tracyObject.transform.position)) < teamDistance)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Switching Methods

    void TeamUp()
    {
        teamedUp = true;

        Vector2 spawnLocation = sketchObject.transform.position;
        Quaternion rotation = sketchObject.transform.rotation;

        Destroy(sketchObject);
        Destroy(tracyObject);

        teamObject = (GameObject)Instantiate(Characters.Team, spawnLocation, rotation);
        teamObject.name = "Team";

        GameManager.Instance.SetCharacterState(CharacterState.Team);

        ChangeCameraTarget(teamObject);
    }

    void UnTeam()
    {
        teamObject = GameObject.FindGameObjectWithTag("Team");
        Vector3 spawnLocation = teamObject.transform.position;
        Quaternion rotation = teamObject.transform.rotation;

        Destroy(teamObject);

        sketchObject = (GameObject)Instantiate(Characters.Sketch, spawnLocation - new Vector3(0.5f, 0, 0), rotation);
        sketchObject.name = "Sketch";

        tracyObject = (GameObject)Instantiate(Characters.Tracy, spawnLocation + new Vector3(0.5f, 0, 0), rotation);
        tracyObject.name = "Tracy";

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

    #endregion

    #region Camera Switch

    void ChangeCameraTarget(GameObject newTarget)
    {
        cam.GetComponent<CameraFollow>().SetTarget(newTarget.transform);
    }

    #endregion

}

