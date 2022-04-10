using UnityEngine;

public class BottleBreakingSound : MonoBehaviour
{
    AudioSource bottleBreaking;
    void Start()
    {
        bottleBreaking = GetComponent<AudioSource>();
        bottleBreaking.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bottleBreaking.isPlaying) {
            Destroy(this);
        }
    }
}
