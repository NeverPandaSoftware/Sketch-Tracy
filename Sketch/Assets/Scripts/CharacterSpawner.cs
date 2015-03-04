using UnityEngine;
using System.Collections;

public class CharacterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CharacterPreFabs
    {
        public GameObject Sketch;
        public GameObject Tracy;
        public GameObject Team;
    }
    public CharacterPreFabs Characters;

	// Use this for initialization
	void Start ()
    {
        SpawnCharacter();
	}
	
	void SpawnCharacter()
    {
        switch (GameManager.Instance.GetCharacterState())
        {
            case CharacterState.Sketch:
                GameObject sketch = (GameObject)Instantiate(Characters.Sketch, transform.position, transform.rotation);
                sketch.name = "Sketch";
                break;
            case CharacterState.Tracy:
                GameObject tracy = (GameObject)Instantiate(Characters.Tracy, transform.position, transform.rotation);
                tracy.name = "Sketch";
                break;
            case CharacterState.Team:
                GameObject team = (GameObject)Instantiate(Characters.Team, transform.position, transform.rotation);
                team.name = "Team";
                break;
            default:
                GameObject defaultCase = (GameObject)Instantiate(Characters.Sketch, transform.position, transform.rotation);
                defaultCase.name = "Sketch";
                break;
        }
    }
}
