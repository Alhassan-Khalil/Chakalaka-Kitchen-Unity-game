using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class StoveSound : MonoBehaviour
{
    [SerializeField] OvenCounter OvenCounter;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Start()
    {
        OvenCounter.OnStateChanged += OvenCounter_OnStateChanged;
        OvenCounter.OnProgressChanged += OvenCounter_OnProgressChanged;
    }

    private void OvenCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArg e)
    {
        float bunShowProgressAmount = 0.5f;
        playWarningSound = OvenCounter.isFried() && e.progressNormalized >= bunShowProgressAmount;


    }

    private void OvenCounter_OnStateChanged(object sender, OvenCounter.OnStateChangeEventArgs e)
    {
        bool playSound = e.state == OvenCounter.State.Frying || e.state == OvenCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {

        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = .1f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.instance.PlayWarningSound(OvenCounter.transform.position);
            }
        }
    }
}
