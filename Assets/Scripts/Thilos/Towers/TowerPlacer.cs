using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacer : MonoBehaviour
{
    public Tilemap tm;

    bool isInDragMode = true;
    public towerScriptableObject towerPrefab;
    public GameObject standartTowerPrefab;
    public GameObject dragPrefab;
    GameObject dragObject;
    Transform dragger;

    towerAction[,] towerMap = new towerAction[TileMapBuilder.width, TileMapBuilder.height];

    public Transform parrent;

    void Start()
    {
        InputManager.get().requestInput("PlaceTower", onDragStart, true, false, false);
        InputManager.get().requestInput("PlaceTower", onDrag, false, false, true);
        InputManager.get().requestInput("PlaceTower", onDragStop, false, true, false);

        dragger = new GameObject().transform;
        dragger.parent = parrent;
    }

    private void onDragStart()
    {
        if (towerPrefab == null)
            return;
        isInDragMode = true;
        dragObject = Instantiate(dragPrefab);
        dragObject.transform.parent = dragger;
        dragObject.transform.localPosition = Vector3.zero;
    }

    private void onDrag()
    {
        if (!isInDragMode)
            return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellpos = tm.WorldToCell(pos);
        dragger.transform.position = tm.CellToWorld(cellpos) + new Vector3(0, tm.cellSize.y / 2, 0);
    }

    private void onDragStop()
    {
        Destroy(dragObject);

        Vector3Int cellpos = tm.WorldToCell(dragger.transform.position);
        Vector2Int mapPos = TileMapBuilder.tileToMap(cellpos);
        Vector2 position = new Vector2(dragger.position.x, dragger.position.y);

        towerAction actionScript = Instantiate(standartTowerPrefab).GetComponent<towerAction>();
        actionScript.pos = position;
        actionScript.transform.position = new Vector3(position.x, position.y, 0);
        actionScript.transform.parent = parrent;
        actionScript.layer = 0;
        actionScript.viewDirection = 0;
        actionScript.prefab = towerPrefab;
        actionScript.bodySprite.sprite = towerPrefab.body;
        actionScript.turretSprite.sprite = towerPrefab.turret;
        actionScript.Setup();

        isInDragMode = false;
    }

}