using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{

    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private PlateCounter plateCounter;

    private void Awake()
    {
        plateCounter = GetComponentInParent<PlateCounter>();
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        plateCounter.OnplateSpwaned += PlateCounter_OnplateSpwaned;
        plateCounter.OnplateRemoved += PlateCounter_OnplateRemoved;

    }

    private void PlateCounter_OnplateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObj = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObj);
        Destroy(plateGameObj);
    }

    private void PlateCounter_OnplateSpwaned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform =Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0,plateOffsetY * plateVisualGameObjectList.Count,0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);

    }
}
