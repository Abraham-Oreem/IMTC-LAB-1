using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private List<GameObject> neon1;
    [SerializeField] private List<GameObject> neon2;
    [SerializeField] private List<GameObject> neon3;

    private int currentColor = 1;
    private int direction = 1;

    private void Start()
    {
        SetNeonState();
    }

    public bool Interact()
    {
        currentColor += direction;

        if (currentColor >= 3)
        {
            currentColor = 3;
            direction = -1;
        }
        else if (currentColor <= 1)
        {
            currentColor = 1;
            direction = 1;
        }

        SetNeonState();

        return true;
    }

    private void SetNeonState()
    {
        SetActive(neon1, currentColor == 1);
        SetActive(neon2, currentColor == 2);
        SetActive(neon3, currentColor == 3);
    }

    private void SetActive(List<GameObject> objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
}