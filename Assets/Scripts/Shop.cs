using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint standardTurret2;
    public TurretBlueprint standardTurret3;
    public TurretBlueprint standardTurret4;

    BuildManager buildManager;
    void Start()
    {
        buildManager = BuildManager.instance;
    }

	void Update()
	{
        if (standardTurret.cost > PlayerStats.Money)
        {
            standardTurret.button.GetComponent<CanvasGroup>().interactable = false;
        }
        else if (standardTurret.cost <= PlayerStats.Money && !standardTurret.button.GetComponent<TurretCooldown>().startCooldown)
        {
            standardTurret.button.GetComponent<CanvasGroup>().interactable = true;
        }

		if (standardTurret2.cost > PlayerStats.Money)
		{
			standardTurret2.button.GetComponent<CanvasGroup>().interactable = false;
		}
        else if (standardTurret2.cost <= PlayerStats.Money && !standardTurret2.button.GetComponent<TurretCooldown>().startCooldown)
        {
            standardTurret2.button.GetComponent<CanvasGroup>().interactable = true;
        }

        if (standardTurret3.cost > PlayerStats.Money)
		{
			standardTurret3.button.GetComponent<CanvasGroup>().interactable = false;
		}
        else if (standardTurret3.cost <= PlayerStats.Money && !standardTurret3.button.GetComponent<TurretCooldown>().startCooldown)
        {
            standardTurret3.button.GetComponent<CanvasGroup>().interactable = true;
        }

        if (standardTurret4.cost > PlayerStats.Money)
        {
            standardTurret4.button.GetComponent<CanvasGroup>().interactable = false;
        }
        else if (standardTurret4.cost <= PlayerStats.Money && !standardTurret4.button.GetComponent<TurretCooldown>().startCooldown)
        {
            standardTurret4.button.GetComponent<CanvasGroup>().interactable = true;
        }
    }
	public void SelectTurret()
    {
        if (buildManager.GetTurretToBuild() == standardTurret)
        {
            buildManager.SelectTurretToBuild(null);
            return;
        }

        buildManager.SelectTurretToBuild(standardTurret);
        buildManager.PreviewTurretOn();
    }

    public void SelectTurret2()
    {
        if (buildManager.GetTurretToBuild() == standardTurret2)
        {
            buildManager.SelectTurretToBuild(null);
            return;
        }

        buildManager.SelectTurretToBuild(standardTurret2);
        buildManager.PreviewTurretOn();
    }

    public void SelectTurret3()
    {
        if (buildManager.GetTurretToBuild() == standardTurret3)
        {
            buildManager.SelectTurretToBuild(null);
            return;
        }

        buildManager.SelectTurretToBuild(standardTurret3);
        buildManager.PreviewTurretOn();
    }

    public void SelectTurret4()
    {
        if (buildManager.GetTurretToBuild() == standardTurret4)
        {
            buildManager.SelectTurretToBuild(null);
            return;
        }

        buildManager.SelectTurretToBuild(standardTurret4);
        buildManager.PreviewTurretOn();
    }
}
