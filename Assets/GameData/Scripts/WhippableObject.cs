using UnityEngine;

public class WhippableObject : MonoBehaviour, IGravityWhippable
{
    public bool CanBeWhipped()
    {
        return true;
    }
}