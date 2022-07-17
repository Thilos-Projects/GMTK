using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace gui
{
    public class GuiManager : MonoBehaviour
    {
        [SerializeField] private float flipSpeed = 1;
        [FormerlySerializedAs("Inventory")] public GameObject inventory;
        public GameObject rerollButton;
        public bool DisableFlip { get; set; }

        private Coroutine flip;
        private bool stageIsFlipped = false;

        [SerializeField] private RectTransform toolPanel;

        private float toolPanelStartingYPos;

        private void Start()
        {
            toolPanelStartingYPos = toolPanel.anchoredPosition.y;
        }

        public void FlipStage()
        {
            if (DisableFlip) return;
            if(flip != null) StopCoroutine(flip);

            stageIsFlipped = !stageIsFlipped;
            //inventory.SetActive(!stageIsFlipped);
            //rerollButton.SetActive(!stageIsFlipped);

            flip = StartCoroutine(FlipEnumerator(stageIsFlipped));
        }

        private IEnumerator FlipEnumerator(bool flipped)
        {
            if (flipped)
            {
                Vector2 move = Vector2.up * flipSpeed;
                while (toolPanel.anchorMin.y < 1)
                {
                    yield return null;
                    toolPanel.anchorMin += move * Time.deltaTime;
                    toolPanel.anchorMax += move * Time.deltaTime;
                    toolPanel.pivot += move * Time.deltaTime;
                }

                var targetVector = new Vector2(.5f, 1);
                toolPanel.anchorMin = targetVector;
                toolPanel.anchorMax = targetVector;
                toolPanel.pivot = targetVector;
            }
            else
            {
                Vector2 move = Vector2.down * flipSpeed;
                while (toolPanel.anchorMin.y > 0)
                {
                    yield return null;
                    toolPanel.anchorMin += move * Time.deltaTime;
                    toolPanel.anchorMax += move * Time.deltaTime;
                    toolPanel.pivot += move * Time.deltaTime;
                }

                var targetVector = new Vector2(.5f, 0);
                toolPanel.anchorMin = targetVector;
                toolPanel.anchorMax = targetVector;
                toolPanel.pivot = targetVector;
            }
        }
    }
}
