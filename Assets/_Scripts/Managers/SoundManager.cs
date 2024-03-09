using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: Make it better

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    private const string PLAYER_PREF_SOUNDEFFECT_VOLUME = "SoundEffectVolume";


    [SerializeField] private AudioClipRefsSO AudioClipRefsSO;


    private float volume = 1f;
    private void Awake()
    {
        instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREF_SOUNDEFFECT_VOLUME, 1f);

    }
    private void Start()
    {
        DeliveryMangaer.Instance.OnRecipeFailed += Deliverymanager_OnRecipeFailed;
        DeliveryMangaer.Instance.OnRecipeSuccess += Deliverymanager_OnRecipeSuccess;
        DeliveryMangaer.Instance.OnRecipeSpwaned += Deliverymanager_OnRecipeSpwaned;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlaceHere += BaseCounter_OnAnyObjectPlaceHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void Deliverymanager_OnRecipeSpwaned(object sender, System.EventArgs e)
    {
        TableBell tableBell = TableBell.Instance;
        PlaySound(AudioClipRefsSO.Bell, tableBell.transform.position, 0.6f);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(AudioClipRefsSO.objecDrop, trashCounter.transform.position, 0.6f);
    }

    private void BaseCounter_OnAnyObjectPlaceHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(AudioClipRefsSO.objecDrop, baseCounter.transform.position, 0.6f);
    }

    private void Player_OnPickSomething(object sender, System.EventArgs e)
    {
        PlaySound(AudioClipRefsSO.objecPickup, Player.Instance.transform.position, 0.6f);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingcounter = sender as CuttingCounter;
        PlaySound(AudioClipRefsSO.chop, cuttingcounter.transform.position, 0.6f);
    }

    private void Deliverymanager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(AudioClipRefsSO.deliverSuccess,deliveryCounter.transform.position,0.6f);
    }

    private void Deliverymanager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;

        PlaySound(AudioClipRefsSO.deliverFail, deliveryCounter.transform.position,0.6f);

    }




    //=====================================================================================
    private void PlaySound(AudioClip[] audioClipArray, Vector3 postion, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], postion, volume);
    }
    public void PlayFootstepsSound(Vector3 postion, float volume = 1f)
    {
        PlaySound(AudioClipRefsSO.footSteps, postion, volume);
    }
    public void PlayCountdownSound()
    {
        PlaySound(AudioClipRefsSO.warning, Vector3.zero);
    }

    public void PlayWarningSound( Vector3 postion)
    {
        PlaySound(AudioClipRefsSO.warning, postion);
    }
    private void PlaySound(AudioClip audioClip, Vector3 postion, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, postion, volumeMultiplier * volume);
    }


    //=======================================================================




    public void ChangeVolume(float v)
    {

        volume = v;

        PlayerPrefs.SetFloat(PLAYER_PREF_SOUNDEFFECT_VOLUME, v);
        PlayerPrefs.Save();
    }
    public float GetVolume() => volume;

}
