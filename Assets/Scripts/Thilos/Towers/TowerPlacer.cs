using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class TowerPlacer : MonoBehaviour
{
    public Tilemap tm;

    bool isInDragMode = false;
    public towerScriptableObject towerPrefab;
    public GameObject standartTowerPrefab;
    public GameObject dragPrefab;
    GameObject dragObject;
    Transform dragger;

    public UnityEvent onPlace;

    public static towerAction[,] towerMap = new towerAction[TileMapBuilder.width, TileMapBuilder.height];

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
        if (!isInDragMode)
            return;

        Destroy(dragObject);

        Vector3Int initCell = tm.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2Int mapPos = TileMapBuilder.tileToMap(initCell);
        Vector2Int reverseMapPos = new Vector2Int(mapPos.x, 30-mapPos.y + 38);
        Vector3Int cellPos = TileMapBuilder.mapToTile(mapPos);
        Vector3Int reverseCellPos = TileMapBuilder.mapToTile(reverseMapPos);
        Vector2 position = tm.CellToWorld(cellPos) + new Vector3(0, tm.cellSize.y / 2, 0);
        Vector2 reversPosition = tm.CellToWorld(reverseCellPos) + new Vector3(0, tm.cellSize.y / 2, 0);
        //Vector2 position = new Vector2(dragger.position.x, dragger.position.y);
        //Vector2 reversPosition = tm.CellToWorld(TileMapBuilder.mapToTile(reverseMapPos));

        isInDragMode = false;

        if (mapPos.x < 1 || mapPos.x > 14 || mapPos.y < 3 || mapPos.y > 30)
            return;

        if (TileMapBuilder.map[mapPos.x, mapPos.y] > 1)
            return;

        onPlace.Invoke();

        if (towerMap[mapPos.x, mapPos.y] != null && towerMap[mapPos.x, mapPos.y].prefab == towerPrefab && !towerMap[mapPos.x, mapPos.y].Upgraded)
        {
            towerMap[mapPos.x, mapPos.y].Upgraded = true;
            towerMap[mapPos.x, mapPos.y].body.SetBool("Upgrade", true);
            if (towerMap[reverseMapPos.x, reverseMapPos.y] != null)
            {
                towerMap[reverseMapPos.x, reverseMapPos.y].Upgraded = true;
                towerMap[reverseMapPos.x, reverseMapPos.y].body.SetBool("Upgrade", true);
            }
        }
        else if(towerMap[mapPos.x, mapPos.y] == null)
        {
            towerAction actionScript = Instantiate(standartTowerPrefab).GetComponent<towerAction>();
            towerMap[mapPos.x, mapPos.y] = actionScript;
            actionScript.transform.parent = parrent;
            actionScript.transform.position = new Vector3(position.x, position.y, 0);
            actionScript.transform.localPosition = new Vector3(actionScript.transform.localPosition.x, actionScript.transform.localPosition.y, -500 + Mathf.Abs(actionScript.transform.localPosition.y));
            actionScript.layer = 0;
            actionScript.viewDirection = 0;
            actionScript.body.runtimeAnimatorController = towerPrefab.prefab;
            actionScript.prefab = towerPrefab;
            actionScript.Setup();


            if (TileMapBuilder.map[reverseMapPos.x, reverseMapPos.y] > 1)
            {
                towerPrefab = null;
                return;
            }

            actionScript = Instantiate(standartTowerPrefab).GetComponent<towerAction>();
            towerMap[reverseMapPos.x, reverseMapPos.y] = actionScript;
            actionScript.transform.parent = parrent;
            actionScript.transform.position = new Vector3(reversPosition.x, reversPosition.y, 0);
            actionScript.transform.localPosition = new Vector3(actionScript.transform.localPosition.x, actionScript.transform.localPosition.y, -500 + Mathf.Abs(actionScript.transform.localPosition.y));
            actionScript.layer = 1;
            actionScript.viewDirection = 0;
            actionScript.prefab = towerPrefab;
            actionScript.body.runtimeAnimatorController = towerPrefab.reverseTower.prefab;
            actionScript.prefab = towerPrefab.reverseTower;
            actionScript.Setup();
        }

        towerPrefab = null;
    }

}