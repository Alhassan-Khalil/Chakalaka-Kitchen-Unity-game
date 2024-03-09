using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{

    [SerializeField] private Image IconImage;
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        IconImage.sprite = kitchenObjectSO.sprite;
    }
}
