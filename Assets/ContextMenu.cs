using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public GameObject menuPanel;

    void Start()
    {
        menuPanel.SetActive(false);
    }

    void Update()
    {
        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.GetComponent<Navigation>())
                {
                    OpenMenu();
                }
            }
        }

        // Left click outside menu closes it
        if (Input.GetMouseButtonDown(0))
        {
            menuPanel.SetActive(false);
        }
    }

    void OpenMenu()
    {
        menuPanel.SetActive(true);

        // Move menu to mouse position
        menuPanel.transform.position = Input.mousePosition;
    }
}