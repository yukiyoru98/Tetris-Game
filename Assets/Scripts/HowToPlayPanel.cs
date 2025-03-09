using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HowToPlayPanel : MonoBehaviour
{
    [SerializeField] private Button PrevBtn, NextBtn;
    [SerializeField] private TextMeshProUGUI PageNumText, PageTitleText;
    [SerializeField] private Animation InstructionAnimation;

    private int currentPageIndex = 0;
    private readonly PageInfo[] PAGE_INFOs = {
        new("MOVE", "MoveAnimation"),
        new("ROTATE", "RotateAnimation"),
        new("HARD DROP", "HardDropAnimation")
    };


    #region UI Functions
    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }
    public void NextPage()
    {
        SetPage(currentPageIndex + 1);
    }

    public void PrevPage()
    {
        SetPage(currentPageIndex - 1);
    }
    #endregion

    #region Private Functions
    private void Awake()
    {
        SetPage(0);
    }

    private void SetPage(int page)
    {
        if (page < 0 || page >= PAGE_INFOs.Length)
        {
            return;
        }
        currentPageIndex = page;
        UpdatePageBtnImage(currentPageIndex);
        UpdatePageText(currentPageIndex);
        UpdateTitleText(currentPageIndex);
        UpdateAnimationClip(currentPageIndex);
    }

    private void UpdatePageBtnImage(int page)
    {
        PrevBtn.interactable = page != 0;
        NextBtn.interactable = page != 2;
    }

    private void UpdatePageText(int page)
    {
        PageNumText.text = $"{page + 1}/{PAGE_INFOs.Length}";
    }

    private void UpdateTitleText(int page)
    {
        PageTitleText.text = PAGE_INFOs[page].Title;
    }

    private void UpdateAnimationClip(int page)
    {
        InstructionAnimation.Play(PAGE_INFOs[page].AnimationClipName);
    }
    #endregion


    private struct PageInfo
    {
        public string Title;
        public string AnimationClipName;

        public PageInfo(string title, string animationClipName)
        {
            Title = title;
            AnimationClipName = animationClipName;
        }
    }

}
