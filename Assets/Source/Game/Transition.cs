using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] private Button ContinueBtn;
    [SerializeField] private Button AgainBtn;
    [SerializeField] private TextMeshProUGUI ContinueText;
    [SerializeField] private TextMeshProUGUI AgainText;

    [SerializeField] private CanvasGroup LevelUpPage;
    [SerializeField] private CanvasGroup RewardCardPage;

    private List<CanvasGroup> ActivePages = new();

    private int currentIndex;
    private float inputEnableTime;

    private void Awake()
    {
        ContinueBtn.onClick.AddListener(OnContinueClicked);
        AgainBtn.onClick.AddListener(OnAgainClicked);
    }

    private void Start()
    {
        //ContinueText.text = LocalizationManager.Instance.GetText("ui.continue");
        //AgainText.text = LocalizationManager.Instance.GetText("ui.tryagain");

        TransitionContext context = GameManager.Instance.CurrentTransitionContext;

        if (context.ShowLevelUpPage)
        {
            ActivePages.Add(LevelUpPage);
        }

        if (context.ShowRewardCardPage && context.RewardCard != null)
        {
            ActivePages.Add(RewardCardPage);
        }

        if (ActivePages.Count == 0)
        {
            SetChoiceButtonsEnabled(true);
            return;
        }

        currentIndex = 0;
        ShowPage(currentIndex);
    }

    private void Update()
    {
        if (Time.unscaledTime < inputEnableTime)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            ShowNextPage();
        }
    }

    private void ShowNextPage()
    {
        if (currentIndex >= ActivePages.Count)
        {
            return;
        }

        currentIndex++;
        ShowPage(currentIndex);
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < ActivePages.Count; i++)
        {
            bool active = i == index;
            CanvasGroup page = ActivePages[i];

            page.alpha = active ? 1f : 0f;
            page.blocksRaycasts = active;
            page.interactable = active;
        }

        SetChoiceButtonsEnabled(index >= ActivePages.Count);
    }

    private void SetChoiceButtonsEnabled(bool enabled)
    {
        ContinueBtn.interactable = enabled;
        AgainBtn.interactable = enabled;

        if (!enabled && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OnContinueClicked()
    {
        EventsHandler.TriggerEvent(SceneEvents.NEXT_SCENE);
    }

    private void OnAgainClicked()
    {
        EventsHandler.TriggerEvent(SceneEvents.TRY_AGAIN);
    }

    private void OnDestroy()
    {
        ContinueBtn.onClick.RemoveListener(OnContinueClicked);
        AgainBtn.onClick.RemoveListener(OnAgainClicked);
    }
}
