using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    public int index = 0;
    [SerializeField] bool keyDown = false;
    [SerializeField] int maxIndex = 0;
    public AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (Input.GetAxis("Vertical_P1") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical_P1") < 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Vertical_P1") > 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }
}
