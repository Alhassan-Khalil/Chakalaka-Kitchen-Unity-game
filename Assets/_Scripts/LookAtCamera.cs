using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAT,
        LookATInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;

    // Start is called before the first frame update
    private void LateUpdate()
    {
        switch (mode    )
        {
            case Mode.LookAT:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookATInverted:
                Vector3 dirFromCamer = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamer);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            default:
                break;
        }
    }
}
