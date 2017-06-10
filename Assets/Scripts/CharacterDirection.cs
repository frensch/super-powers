using UnityEngine;
using System.Collections;

public class CharacterDirection : MonoBehaviour {

    public GameObject bridgeLeft;
    public GameObject bridgeRight;
    public GameObject[] towersTop;
    public GameObject[] towersBottom;

    private bool[] towersTopStatus = { true, true, true};
    private bool[] towersBottomStatus = { true, true, true};

    GameObject CalculateClosestTower(GameObject[] towers, Vector2 position, bool[] towerStatus, bool towerStatusTrue)
    {
        int minIndex = 0;
        float minDist = 1000;
        for (int i = 0; i < towers.Length; ++i)
        {
            float dist = Vector2.Distance(position, towers[i].transform.position);
            if(dist < minDist && (towerStatus[i] || !towerStatusTrue) )
            {
                minDist = dist;
                minIndex = i;
            }
        }
        return towers[minIndex];
    }

	// Update is called once per frame
	public Vector2 CalcDirection (bool bottomTeam, bool fly, Vector2 position) {
        Vector2 direction = Vector2.zero;
        if (bottomTeam)
        {
            GameObject tower = CalculateClosestTower(towersTop, position, towersTopStatus, fly);
            if(!fly && position.y < bridgeLeft.transform.position.y - 0.5f)
            {
                GameObject bridge = bridgeLeft;
                if(Vector2.Distance(position, bridgeLeft.transform.position) >
                   Vector2.Distance(position, bridgeRight.transform.position))
                {
                    bridge = bridgeRight;
                }
                direction = ((Vector2)bridge.transform.position - position);
            } else
            {
                direction = ((Vector2)tower.transform.position - position);
            }
        }
        return direction;
    }
}
