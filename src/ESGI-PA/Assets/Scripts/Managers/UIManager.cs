using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameLoop loop;
    [SerializeField] private GameObject[] uiList;
    [SerializeField] private RaceConfig config;
    public GameObject[] Players;

    private void Start()
    {
        StartCoroutine(SetUIConfig());
    }

    public IEnumerator SetUIConfig()
    {
        yield return new WaitForSeconds(0.1f);
        switch (config.devices.Count)
        {
            case 1:
                uiList[0].SetActive(true);
                break;
            case 2:
                uiList[1].SetActive(true);
                break;
            case 3:
                uiList[2].SetActive(true);
                break;
            case 4:
                uiList[3].SetActive(true);
                break;
        }
        LinkUI(config.devices.Count);
    }

    private void LinkUI(int players)
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        for (var i = 0; i < players; i++)
        {
            uiList[players - 1].gameObject.transform.GetChild(i).gameObject.GetComponent<PlayerUI>().loop =
                loop;
            uiList[players - 1].gameObject.transform.GetChild(i).gameObject.GetComponent<PlayerUI>().player =
                Players[i];
        }
    }
}
