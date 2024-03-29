using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{

    private const string POPUP = "Popup";

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI deleveryMessageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;

    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryMangaer.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        DeliveryMangaer.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        deleveryMessageText.text = "Delivery\nFailed";
        animator.SetTrigger(POPUP);
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        deleveryMessageText.text = "Delivery\nSuccess";
        animator.SetTrigger(POPUP);

    }
}


