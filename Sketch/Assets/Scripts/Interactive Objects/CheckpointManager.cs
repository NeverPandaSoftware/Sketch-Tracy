using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint[] checkpointList;

    public Checkpoint CurrentCheckpoint { get; set; }

	void Awake()
    {
        checkpointList = gameObject.GetComponentsInChildren<Checkpoint>().OrderBy(go=>go.gameObject.name).ToArray<Checkpoint>();
    }

    public void CheckpointActivated()
    {
        Checkpoint farthestCheckpoint = null;

        for (int i = 0; i < checkpointList.Length; i++)
        {
            if (checkpointList[i].IsActivated())
            {
                farthestCheckpoint = checkpointList[i];
            }
        }

        CurrentCheckpoint = farthestCheckpoint;
    }
}
