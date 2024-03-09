using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCounterVisual : MonoBehaviour
{
    private OvenCounter ovenCounter;
    private void Awake()
    {
        ovenCounter = GetComponentInParent<OvenCounter>();
    }
    [SerializeField] private GameObject StoveOnGameObject;
    [SerializeField] private GameObject ParticlesGameObject;


    private void Start()
    {
        ovenCounter.OnStateChanged += OvenCounter_OnStateChanged;
    }

    private void OvenCounter_OnStateChanged(object sender, OvenCounter.OnStateChangeEventArgs e)
    {
        bool showVisual = e.state == OvenCounter.State.Frying || e.state == OvenCounter.State.Fried;

        StoveOnGameObject.SetActive(showVisual);
        ParticlesGameObject.SetActive(showVisual);

    }
}
