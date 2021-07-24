using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstanciated;
    public float assignedTime;

    void Start() {
        timeInstanciated = SongManager.GetAudioSourceTime();
    }

    void Update() {
        double timeSinceInstanciated = SongManager.GetAudioSourceTime() - timeInstanciated;
        float t = (float)(timeSinceInstanciated / (SongManager.Instance.noteTime * 2));

        if(t > 1) {
            Destroy(gameObject);
        } else {
            transform.position = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, t);
            transform.position = new Vector3(transform.parent.position.x, transform.position.y, transform.position.z);
            GetComponent<SpriteRenderer>().enabled = true;
        }

    }

}
