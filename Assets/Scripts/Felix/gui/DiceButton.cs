using System.Collections;
using System.Collections.Generic;
using gui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceButton : MonoBehaviour, IPointerDownHandler
{
    public Inventory inv;

    public void OnPointerDown(PointerEventData eventData)
    {
        inv.startdragNDrop(gameObject);
    }
}
