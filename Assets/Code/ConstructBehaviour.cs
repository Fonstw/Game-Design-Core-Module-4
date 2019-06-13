using UnityEngine;

public class ConstructBehaviour : MonoBehaviour
{
    public enum MetalType
    {
        silver = 1,
        copper = 2,
        gold = 3,
        chrome = 4,
        doublane = 5,
        none = 0
    }

    public MetalType metalType = MetalType.silver;
}
