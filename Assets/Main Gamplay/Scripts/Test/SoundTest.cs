using UnityEngine;

public class SoundTest : MonoBehaviour {
    void Start() {
        SoundManager.Instance.PlayMusicNoFade("test");
    }
}
