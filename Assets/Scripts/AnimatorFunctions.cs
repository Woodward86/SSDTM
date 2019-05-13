using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    public bool disableOnce;

    private void Start()
    {
        menuButtonController = GetComponentInParent<MenuButtonController>();
    }

    public void PlaySound(AudioClip whichSound)
    {
        if (!disableOnce)
        {
            menuButtonController.audioSource.PlayOneShot(whichSound);
        }
        else
        {
            disableOnce = false;
        }
    }
}
