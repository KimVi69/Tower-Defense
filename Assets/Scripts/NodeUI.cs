using UnityEngine;

public class NodeUI : MonoBehaviour
{
    private Node target;
    public GameObject ui;
    public GameObject rangeUI;

    void Update()
    {
        if (GameManager.gameEnded)
        {
            Hide();
        }
    }

    public void SetTarget(Node node)
    {
        target = node;

        transform.position = target.transform.position;

        if (target.turret.GetComponent<Turret>() != null)
        {
            rangeUI.GetComponent<RectTransform>().sizeDelta = target.turret.GetComponent<Turret>().
            rangeUI.GetComponent<RectTransform>().sizeDelta;

            rangeUI.GetComponent<RectTransform>().anchoredPosition = target.turret.GetComponent<Turret>().
            rangeUI.GetComponent<RectTransform>().anchoredPosition;
        }
        else
        {
            rangeUI.GetComponent<RectTransform>().sizeDelta = target.turret.GetComponent<Healer>().
            rangeUI.GetComponent<RectTransform>().sizeDelta;

            rangeUI.GetComponent<RectTransform>().anchoredPosition = target.turret.GetComponent<Healer>().
            rangeUI.GetComponent<RectTransform>().anchoredPosition;
        }

        ui.SetActive(true);
        rangeUI.SetActive(true);
        Time.timeScale = 0.25f;
    }

    public void Hide()
    {
        ui.SetActive(false);
        rangeUI.SetActive(false);
        Time.timeScale = PlayerStats.gameSpeed;
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
