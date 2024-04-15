using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource = null;
    private Button[] buttons = null;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        
        foreach (Button button in buttons)
        {
            button?.onClick.AddListener(CallButtonSound);
        }

        audioSource = GetComponent<AudioSource>();

    }

    private void OnDestroy()
    {
        foreach (Button button in buttons)
        {
            button?.onClick.RemoveListener(CallButtonSound);
        }
    }

    private void CallButtonSound()
    {
        audioSource?.Play();
    }
}
