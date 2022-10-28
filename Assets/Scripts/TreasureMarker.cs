using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureMarker : MonoBehaviour
{
    private void Awake() {
        if (GameManager.instance.treasuresCollected.Contains(name)) {
            Destroy(transform.parent.gameObject);
        }
    }
}
