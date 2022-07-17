using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace gui
{
    public class Inventory : MonoBehaviour
    {
        public GameObject dieA, dieB;
        public GuiManager gui;
        private Image AImage, BImage;
        private Button AButton, BButton;
        public Sprite[] sprites;
        private int[] dice;

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
        }

        public void rollDice()
        {
            StartCoroutine(TheySeeMeRolling());
        }

        private IEnumerator TheySeeMeRolling()
        {
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