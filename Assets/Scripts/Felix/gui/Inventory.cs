using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

namespace gui
{
    public class Inventory : MonoBehaviour
    {
        public GameObject dieA, dieB;
        public towerScriptableObject[] towers;
        public Button rerollButton;
        public GuiManager gui;
        private Image AImage, BImage;
        private Button AButton, BButton;
        public Sprite[] sprites;
        private int[] dice;
        private bool waveStarted = false;
        public TowerPlacer tp;
        private GameObject dieToDeactivate;

        private void Awake()
        {
            dice = new [] {0, 0};
        }

        private void Start()
        {
            AImage = dieA.GetComponent<Image>();
            BImage = dieB.GetComponent<Image>();
            AButton = dieA.GetComponent<Button>();
            BButton = dieB.GetComponent<Button>();
            rollDice();
            if (TargetManager.onChangeEnemyCount == null)
            {
                TargetManager.onChangeEnemyCount = new UnityEvent<int>();
            }
            TargetManager.onChangeEnemyCount.AddListener(onEnemyChange);
        }

        public void StartWave()
        {
            waveStarted = true;
        }

        public void onEnemyChange(int count)
        {
            if (waveStarted && count == 0)
            {
                rerollButton.interactable = true;
            }
            rollDice();
        }

        public void rollDice()
        {
            StartCoroutine(TheySeeMeRolling());
        }

        public void startdragNDrop(int num, GameObject die)
        {
            tp.towerPrefab = towers[num - 1];
            rerollButton.interactable = false;
            dieToDeactivate = die;
        }

        public void endDragNDrop()
        {
            tp.towerPrefab = null;
            rerollButton.interactable = false;
            if (dieToDeactivate.Equals(dieA))
            {
                setDice(new[]{0, dice[1]});
                return;
            }
            setDice(new[]{dice[0], 0});
        }

        private IEnumerator TheySeeMeRolling()
        {
            tp.towerPrefab = null;
            gui.DisableFlip = true;
            var t = Time.time;
            AButton.enabled = false;
            BButton.enabled = false;
            while (Time.time - t < 2)
            {
                setDice(new[] {UnityEngine.Random.Range(1, 7), UnityEngine.Random.Range(1, 7)});
                yield return new WaitForSeconds(.1f);
            }
            AButton.enabled = false;
            BButton.enabled = false;
            gui.DisableFlip = false;
        }

        public void setDice(int[] _dice)
        {

            for(var i = 0; i < dice.Length; i++)
            {
                dice[i] = _dice[i];
            }

            if (dice[0] == 0)
            {
                dieA.SetActive(false);
            }
            else
            {
                dieA.SetActive(true);
                AImage.sprite = sprites[dice[0] - 1];
            }

            if (dice[1] == 0)
            {
                dieB.SetActive(false);
            }
            else
            {
                dieB.SetActive(true);
                BImage.sprite = sprites[dice[1] - 1];
            }
        }
    }
}