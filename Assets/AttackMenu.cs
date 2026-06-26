using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class AttackMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Button buttonPrefab;
    public List<WeaponGroup> weaponGroups;

    void Update()
    {
        panel.SetActive(true);

        foreach (Transform child in buttonParent) Destroy(child.gameObject);

        foreach (var group in weaponGroups)
        {
            Button btn = Instantiate(buttonPrefab, buttonParent, false);
            btn.GetComponentInChildren<TMP_Text>().text = group.displayName;
            btn.onClick.AddListener(() => SelectGroup(group));
        }
    }

    public void ShowMenu()
    {
        panel.SetActive(true);

        foreach (Transform child in buttonParent) Destroy(child.gameObject);

        foreach (var group in weaponGroups)
        {
            Button btn = Instantiate(buttonPrefab, buttonParent, false);
            btn.GetComponentInChildren<TMP_Text>().text = group.displayName;
            btn.onClick.AddListener(() => SelectGroup(group));
        }
    }

    private void SelectGroup(WeaponGroup group)
    {
        panel.SetActive(false);
        // switch to that weapon group here
    }
}