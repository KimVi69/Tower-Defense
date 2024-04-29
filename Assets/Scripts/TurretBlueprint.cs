using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TurretBlueprint
{
    public GameObject prefab;
    public int cost;
    public bool isGroundTurret;
	public GameObject button;
    public int cooldown;
}
