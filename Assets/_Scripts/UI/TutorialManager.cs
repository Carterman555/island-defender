using DG.Tweening;
using IslandDefender.Environment;
using IslandDefender.Management;
using System;
using System.Collections;
using TarodevController;
using TMPro;
using UnityEngine;

namespace IslandDefender {
	public class TutorialManager : StaticInstance<TutorialManager> {

        [SerializeField] private TextMeshProUGUI storyText;
        [SerializeField] private Transform tutorialSpawnPoint;

        private void Start() {
            if (GameManager.Instance.PlayIntroAndTutorial()) {
                StartCoroutine(ShowStoryTextThenTutorial());
                MovePlayerToBeach();
            }
            else {
                FadePanel.Instance.FadeIn();
            }
        }

        private IEnumerator ShowStoryTextThenTutorial() {

            FadePanel.Instance.StayFadedOut();

            storyText.enabled = true;

            storyText.color = new Color(1, 1, 1, 0); // clear white

            float fadeDuration = 1f;
            storyText.DOFade(1, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);

            float showDuration = 4f;
            yield return new WaitForSeconds(showDuration);

            storyText.DOFade(0, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);

            storyText.enabled = false;

            FadePanel.Instance.FadeIn();

            float panelFadeDuration = 0.5f;
            yield return new WaitForSeconds(panelFadeDuration);

            StartTutorial();
        }

        private void MovePlayerToBeach() {
            FindObjectOfType<PlayerController>().transform.position = tutorialSpawnPoint.position;
        }


        #region Tutorial Step Management

        private BaseTutorialStep[] tutorialSteps;
        private int currentStepIndex;

        private bool tutorialInProgress;

        [SerializeField] private TextMeshProUGUI tutorialText;

        [Header ("Positions")]
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 showPos;
        [SerializeField] private Vector3 exitPos;

        protected override void Awake() {
            base.Awake();

            tutorialSteps = new BaseTutorialStep[] {
                new BasicMovementStep(), new PickupStep(), new GatherWoodAttackStep(), new RangedAttackStep(),
                new FindKeepStep(), new BuildStep()
            };
        }

        private void StartTutorial() {
            currentStepIndex = 0;
            tutorialSteps[currentStepIndex].Initialize();
            tutorialInProgress = true;
        }

        private bool currentStepIsActive;

        /// <summary>
        /// Tick the current step to allow it to run it's logic. Check if the current step has been completed current
        /// and if step needs to be activated.
        /// </summary>
        private void Update() {

            if (!tutorialInProgress) return;

            BaseTutorialStep currentStep = tutorialSteps[currentStepIndex];

            currentStep.Tick();

            // check if completed and check to activate
            if (currentStep.IsCompleted()) {
                CompleteCurrentStep();
                currentStepIsActive = false;
            }
            else if (!currentStepIsActive && currentStep.IsReady()) {
                StartCoroutine(ActivateCurrentStep());
                currentStepIsActive = true;
            }
        }

        /// <summary>
        /// Go to the next step, check if all steps are complete, and move the panel off screen.
        /// </summary>
        private void CompleteCurrentStep() {
            tutorialSteps[currentStepIndex].Deinitialize();

            currentStepIndex++;
            bool tutorialOver = currentStepIndex >= tutorialSteps.Length;

            // move text off screen
            float moveDuration = 0.5f;
            tutorialText.GetComponent<RectTransform>().DOAnchorPos(exitPos, moveDuration).SetEase(Ease.OutSine).OnComplete(() => {
                if (tutorialOver) {
                    EndTutorial();
                }
            });

            if (tutorialOver) {
                tutorialInProgress = false;
                return;
            }

            tutorialSteps[currentStepIndex].Initialize();
        }

        /// <summary>
        /// Setup the current panel's text and move it on screen
        /// </summary>
        private IEnumerator ActivateCurrentStep() {

            int stepIndexBefore = currentStepIndex;

            float activateDelay = 1f;
            yield return new WaitForSeconds(activateDelay);

            //... if the step did not change during activation delay
            if (stepIndexBefore == currentStepIndex) {
                BaseTutorialStep currentStep = tutorialSteps[currentStepIndex];

                tutorialText.text = currentStep.GetText();

                float moveDuration = 0.5f;
                tutorialText.GetComponent<RectTransform>().anchoredPosition = startPos;
                tutorialText.GetComponent<RectTransform>().DOAnchorPos(showPos, moveDuration).SetEase(Ease.OutSine);
                //tutorialText.transform.DOMove(showPos, moveDuration).SetEase(Ease.OutSine);
            }
        }

        private void EndTutorial() {
            tutorialInProgress = false;
            tutorialText.enabled = false;
        }

        #endregion
    }

    #region Tutorial Steps

    public abstract class BaseTutorialStep {

        public virtual void Initialize() {

        }

        public virtual void Deinitialize() {

        }

        public virtual void Tick() {

        }

        public virtual string GetText() {
            return "Base";
        }

        public virtual bool IsReady() {
            return false;
        }

        public virtual bool IsCompleted() {
            return false;
        }
    }

    public class BasicMovementStep : BaseTutorialStep {

        private bool pressedA;
        private bool pressedD;
        private bool pressedSpace;

        public override void Initialize() {
            base.Initialize();

            pressedA = false;
            pressedD = false;
            pressedSpace = false;
        }

        public override void Tick() {
            base.Tick();

            if (Input.GetKeyDown(KeyCode.A)) {
                pressedA = true;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                pressedD = true;
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                pressedSpace = true;
            }
        }

        public override string GetText() {
            return "Use <color=yellow>a</color> and <color=yellow>d</color> to move\nand <color=yellow>spacebar</color> " +
                "to jump";
        }

        public override bool IsReady() {
            return true;
        }

        public override bool IsCompleted() {
            return pressedA && pressedD && pressedSpace;
        }
    }

    public class PickupStep : BaseTutorialStep {

        private bool pickedUpResource;

        public override void Initialize() {
            base.Initialize();

            pickedUpResource = false;

            PlayerResources.OnResourceAdded += CheckIfPickedUp;
        }

        public override void Deinitialize() {
            base.Deinitialize();

            PlayerResources.OnResourceAdded -= CheckIfPickedUp;
        }

        private void CheckIfPickedUp(ResourceType type, int amount) {
            if (type == ResourceType.Stone || type == ResourceType.Fiber) {
                pickedUpResource = true;
            }
        }

        public override string GetText() {
            return "Find a plant or\nrock and press <color=yellow>e</color>.";
        }

        public override bool IsReady() {
            return true;
        }

        public override bool IsCompleted() {
            return pickedUpResource;
        }
    }

    public class GatherWoodAttackStep : BaseTutorialStep {
        private bool gatheredWood;

        public override void Initialize() {
            base.Initialize();

            gatheredWood = false;

            PlayerResources.OnResourceAdded += CheckIfGatheredWood;
        }

        public override void Deinitialize() {
            base.Deinitialize();

            PlayerResources.OnResourceAdded -= CheckIfGatheredWood;
        }

        private void CheckIfGatheredWood(ResourceType type, int amount) {
            if (type == ResourceType.Wood) {
                gatheredWood = true;
            }
        }

        public override string GetText() {
            return "<color=yellow>Left click</color> to melee attack.\nFind a tree to harvest\nby attacking";
        }

        public override bool IsReady() {
            return true;
        }

        public override bool IsCompleted() {
            return gatheredWood;
        }
    }

    public class RangedAttackStep : BaseTutorialStep {

        private bool firedBow;

        public override void Initialize() {
            base.Initialize();

            firedBow = false;
        }
        
        public override void Tick() {
            base.Tick();

            if (Input.GetMouseButtonDown(1)) {
                firedBow = true;
            }
        }

        public override string GetText() {
            return "<color=yellow>Right click</color> to fire\nyour bow";
        }

        public override bool IsReady() {
            return true;
        }

        public override bool IsCompleted() {
            return firedBow;
        }
    }

    public class FindKeepStep : BaseTutorialStep {

        private Transform player;
        private bool foundKeep;

        public override void Initialize() {
            base.Initialize();

            player = GameObject.FindObjectOfType<PlayerController>().transform;
            foundKeep = false;
        }

        public override void Tick() {
            base.Tick();

            float playerXPosToFindKeep = -15;
            if (player.position.x > playerXPosToFindKeep) {
                foundKeep = true;
            }
        }

        public override string GetText() {
            return "Find Your Keep.\n(to the right)";
        }

        public override bool IsReady() {
            return true;
        }

        public override bool IsCompleted() {
            return foundKeep;
        }
    }

    public class BuildStep : BaseTutorialStep {

        private bool built;

        public override void Initialize() {
            base.Initialize();

            built = false;

            PlayerBuild.OnBuild += Built;
        }

        public override void Deinitialize() {
            base.Deinitialize();

            PlayerBuild.OnBuild -= Built;
        }

        private void Built(BuildingType type) {
            built = true;
        }

        public override string GetText() {
            return "Click the build buttons\nand <color=yellow>Right click</color> to build";
        }

        public override bool IsReady() {
            return true;
        }

        public override bool IsCompleted() {
            return built;
        }
    }

    #endregion
}