using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField, Tooltip("If true, player look movements will be smoothed")]
    private bool lookSmoothing = false;
    private float lookSmoothFactor = 10.0f;


    /*    [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;*/

    [Tooltip("Toggle mouse look on and off")]
    public bool enableMouseLook = true;

    public bool clampVerticalRotation = true;
    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField, Range(1, 180)] public float MaximumX = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField, Range(-180, 1)] public float MinimumX = -90.0f;


    [SerializeField, Range(0, 10)] public float lookSpeedX = 2.0f;
    [SerializeField, Range(0, 10)] public float lookSpeedY = 2.0f;



    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    // View limiting stuff (for "docked" type interactions)
    private bool restrictLook = false;
    private Vector2 lookRestrictionAngles = Vector2.zero;
    private Vector2 rotationSum = Vector2.zero;
    private Vector2 lastRotationChanges = Vector2.zero;

    private PlayerInputManager inputHander;


    public void Init(Transform character, Transform camera)
    {

        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;

    }
    // Start is called before the first frame update
    void Start()
    {
        inputHander = GetComponent<PlayerInputManager>();
    }

    public void LookRotation(Transform character, Transform camera)
    {

        if (enableMouseLook)
        {
            //lastRotationChanges = inputHander.MouseInput;
            lastRotationChanges.x = inputHander.Look.x * lookSpeedX;
            lastRotationChanges.y = inputHander.Look.y * lookSpeedY;

            if (restrictLook)
            {

                if ((rotationSum.y + lastRotationChanges.y) > lookRestrictionAngles.y)
                {
                    lastRotationChanges.y = (lookRestrictionAngles.y - rotationSum.y);
                }
                else if ((rotationSum.y + lastRotationChanges.y) < -lookRestrictionAngles.y)
                {
                    lastRotationChanges.y = (-lookRestrictionAngles.y - rotationSum.y);
                }

                rotationSum.y += lastRotationChanges.y;

                if ((rotationSum.x + lastRotationChanges.x) > lookRestrictionAngles.x)
                {
                    lastRotationChanges.x = (lookRestrictionAngles.x - rotationSum.x);
                }
                else if ((rotationSum.x + lastRotationChanges.x) < -lookRestrictionAngles.x)
                {
                    lastRotationChanges.x = (-lookRestrictionAngles.x - rotationSum.x);
                }

                rotationSum.x += lastRotationChanges.x;

            }

            m_CharacterTargetRot *= Quaternion.Euler(0.0f, lastRotationChanges.x, 0.0f);
            m_CameraTargetRot *= Quaternion.Euler(lastRotationChanges.y, 0.0f, 0.0f);

            // Only clamp when not restricting look
            if (!restrictLook && clampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }

            if (lookSmoothing)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot, lookSmoothFactor * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot, lookSmoothFactor * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

        }

    }


    public void LookAtPosition(Transform character, Transform camera, Vector3 focalPoint)
    {

        // Make character face target //
        Vector3 relativeCharPosition = focalPoint - character.position;
        Quaternion rotation = Quaternion.LookRotation(relativeCharPosition);
        Vector3 flatCharRotation = rotation.eulerAngles;
        flatCharRotation.x = 0.0f;
        flatCharRotation.z = 0.0f;
        character.localRotation = Quaternion.Euler(flatCharRotation);
        // Key: make target rotation our current rotation :)
        m_CharacterTargetRot = character.localRotation;

        // Make Camera face target //
        Vector3 relativeCamPosition = focalPoint - camera.position;
        Quaternion camRotation = Quaternion.LookRotation(relativeCamPosition);
        Vector3 flatCamRotation = camRotation.eulerAngles;
        flatCamRotation.y = 0.0f;
        flatCamRotation.z = 0.0f;
        camera.localRotation = Quaternion.Euler(flatCamRotation);
        // Key: Make cam target rotation our current cam rotation
        m_CameraTargetRot = camera.localRotation;

    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
