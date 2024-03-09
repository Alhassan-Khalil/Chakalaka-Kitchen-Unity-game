using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private BaseCounter clearCounter;
    //private GameObject visualGameobj;

    private void Awake()
    {
        clearCounter = GetComponentInParent<BaseCounter>();
    }

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;

    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == clearCounter)
        {
            gameObject.SetActive(true);

        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
