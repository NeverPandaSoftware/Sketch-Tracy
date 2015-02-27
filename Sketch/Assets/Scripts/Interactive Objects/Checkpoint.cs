using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager checkpointManager;

    private bool activated = false;

    public Vector2 Location { get { return gameObject.transform.position; } }

	void Awake()
    {
        checkpointManager = gameObject.GetComponentInParent<CheckpointManager>();
    }

    public void ActivateCheckpoint()
    {
        activated = true;
        checkpointManager.CheckpointActivated();
    }

    public bool IsActivated()
    {
        return activated;
    }
}
