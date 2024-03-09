using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private OvenCounter OvenCounter;

    private void Start()
    {
        OvenCounter.OnProgressChanged += OvenCounter_OnProgressChanged;

        Hide();
    }

    private void OvenCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArg e)
    {
        float bunShowProgressAmount = 0.5f;
        bool show = OvenCounter.isFried() && e.progressNormalized >= bunShowProgressAmount;

        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
