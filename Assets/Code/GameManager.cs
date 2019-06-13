using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int[] objectsToFix;
    public GameObject[] wormHoles;
    int curFix = 0;
    int curLevel = 0;

    public void ChangeFix(int add)
    {
        curFix += add;

        if (curFix == objectsToFix[curLevel])
        {
            curFix = 0;
            wormHoles[curLevel].SetActive(true);
            curLevel++;
        }
    }
}
