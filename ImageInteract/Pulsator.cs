using UnityEngine;

public class Pulsator : MonoBehaviour
{
    public float minScale = 0.05f;
    public float maxScale = 0.1f;
    public float pulseDuration = 2f;

    private float originalScale;

    void Start()
    {
    }

    void Update()
    {
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time / pulseDuration, 1));
        transform.localScale = new Vector3(scale, scale, scale);
    }


}
