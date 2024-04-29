using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    public Color startColor;
    public Color endColor;
    public float pulseSpeed = 1.0f;
    private MaterialPropertyBlock propertyBlock;

    public GameObject turret;
    private TurretBlueprint turretBlueprint;

    BuildManager buildManager;
    [SerializeField]
    private bool isGround;

    private float elapsedTime = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        buildManager = BuildManager.instance;
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (!buildManager.turretIsSelected)
        {
            ResetPlacementState();
        }
        else
        {
            CheckAndUpdateColor();
        }
    }

    void ResetPlacementState()
    {
        if (propertyBlock.GetColor("_BaseColor") != originalColor)
        {
            SetBaseColor(originalColor);
        }

        elapsedTime = 0f;
    }

    void CheckAndUpdateColor()
    {
        if (turret == null && isGround == buildManager.GetTurretToBuild().isGroundTurret)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.PingPong(elapsedTime * pulseSpeed, 1.0f);

            Color lerpedColor = Color.Lerp(startColor, endColor, t);

            SetBaseColor(lerpedColor);
        }
        else
        {
            ResetPlacementState();
        }
    }

    void SetBaseColor(Color color)
    {
        propertyBlock.SetColor("_BaseColor", color);
        rend.SetPropertyBlock(propertyBlock);
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough Money");
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        GameObject effect = Instantiate(buildManager.buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        GameObject turret = Instantiate(blueprint.prefab, transform.position, Quaternion.identity);
        this.turret = turret;
        turretBlueprint = blueprint;
        if (turret.GetComponent<Turret>() != null)
        {
            turret.GetComponent<Turret>().turretBlueprint = turretBlueprint;
        }
        else
        {
            turret.GetComponent<Healer>().turretBlueprint = turretBlueprint;
        }
        buildManager.PlayBuildSFX();

        blueprint.button.SetActive(false);
	}

    public void SellTurret()
    {
        if (turret.GetComponent<Turret>() != null)
        {
            turret.GetComponent<Turret>().Die();
        }
        else
        {
            turret.GetComponent<Healer>().Die();
        } 
	}

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            buildManager.SelectedNode(this);
            return;
        }

        if (!buildManager.turretIsSelected)
            return;

        if (isGround != buildManager.GetTurretToBuild().isGroundTurret)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
        buildManager.SelectTurretToBuild(null);
    }

    void OnMouseEnter()
    {
        if (!buildManager.turretIsSelected)
            return;

        if (isGround != buildManager.GetTurretToBuild().isGroundTurret)
            return;

        buildManager.PreviewTurretOnNode(this, true);
    }

    void OnMouseExit()
    {
        buildManager.PreviewTurretOnNode(this, false);
    }
}
