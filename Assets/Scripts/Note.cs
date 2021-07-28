using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    private double _timeInstantiated;
    public float assignedTime;

    private RectTransform _rectTransform;
    private Image _image;

    private bool stop;

    private void Start()
    {
        _timeInstantiated = SongManager.GetAudioSourceTime();
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        stop = false;
    }

    private void Update()
    {
        if(!stop) {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
            float t = (float) (timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

            if (t > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                _rectTransform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY,
                    Vector3.up * SongManager.Instance.NoteDespawnY, t);
                _image.enabled = true;
            }
        }
    }

    public void StopNote() {

    }
}