using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    double timeInstanciated;
    public float assignedTime;

    private RectTransform _rectTransform;
    private Image _image;
    
    void Start() {
        timeInstanciated = SongManager.GetAudioSourceTime();
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstanciated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

        if(t > 1) {
            Destroy(gameObject);
        } else
        {
            _rectTransform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.NoteDespawnY, t);
            //_rectTransform.localPosition = new Vector3(0, 110, 0);
            //transform.position = new Vector3(transform.parent.position.x, transform.position.y, transform.position.z);
            //GetComponent<SpriteRenderer>().enabled = true;
            _image.enabled = true;
        }

    }

}
