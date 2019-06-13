using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] ConstructBehaviour.MetalType wantedType = ConstructBehaviour.MetalType.none;
    [SerializeField] Transform portal;
    [SerializeField] float portalTargetHeight = 1.4f, portalRiseSpeed = 0.4666667f;
    [SerializeField] Vector3[] positions;

    List<Transform> done;

    // Start is called before the first frame update
    void Start()
    {
        done = new List<Transform>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (wantedType != ConstructBehaviour.MetalType.none && done.Count < positions.Length && other.tag == "Construct" && other.transform.parent == null && other.GetComponent<ConstructBehaviour>().metalType == wantedType)
        {
            // make sure the player doesn't touch it
            other.GetComponent<ConstructBehaviour>().enabled = false;
            other.tag = "Untagged";
            // remember you have it
            done.Add(other.transform);
            // gently put it where it belongs and stroke it while muttering "preciousssssss"
            other.transform.position = positions[done.Count - 1];
        }

        if (done.Count >= positions.Length && portal.position.y <= portalTargetHeight)
        {
            portal.Translate(0, Time.deltaTime * portalRiseSpeed, 0);

            if (portal.position.y >= portalTargetHeight)
            {
                portal.position = new Vector3(portal.position.x, portalTargetHeight, portal.position.z);
                portal.GetComponent<WormHoleBehaviour>().active = true;
            }
        }
    }
}
