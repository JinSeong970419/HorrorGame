using Michsky.UI.Dark;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public enum MenuType
    {
        MENU,
        SETTING,
    }

    public class UIMenuManager : MonoBehaviour
    {
        [Header("PANEL LIST")]
        public List<GameObject> panels = new List<GameObject>();

        [Header("Panel")]
        [SerializeField] private UIPopup _popupPanel = default;
        [SerializeField] private UISettingsController _settingsPanel = default;
        //[SerializeField] private UICredits _creditsPanel = default;
        [SerializeField] private UIMainMenu _mainMenuPanel = default;

        //[SerializeField] private SaveSystem _saveSystem = default;

        [SerializeField] private InputReader _inputReader = default;

        [Header("RESOURCES")]
        public BlurManager homeBlurManager;

        [Header("Broadcasting on")]
        [SerializeField]
        private GameEventVoid _startNewGameEvent = default;
        [SerializeField]
        private GameEventVoid _continueGameEvent = default;

        [Header("SETTINGS")]
        //public int currentPanelIndex = 0;
        public MenuType currentPanelIndex = MenuType.MENU;
        public bool enableBrushAnimation = true;
        public bool enableHomeBlur = true;
        private bool _hasSaveData;
        private string panelFadeIn = "Panel In";
        private string panelFadeOut = "Panel Out";

        private IEnumerator Start()
        {
            //_inputReader.EnableMenuInput();

            GameObject currentPanel = panels[Convert.ToInt32(currentPanelIndex)];
            Animator currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);

            if (enableHomeBlur == true)
                homeBlurManager.BlurInAnim();

            yield return new WaitForSeconds(0.4f);
            SetMenuScreen();
        }

        private void SetMenuScreen()
        {
            //_hasSaveData = _saveSystem.LoadSaveDataFromDisk();
            //_mainMenuPanel.SetMenuScreen(_hasSaveData);
            //_mainMenuPanel.ContinueButtonAction += _continueGameEvent.Invoke;
            _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
            _mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
            _mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
            _mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;
        }

        private void ButtonStartNewGameClicked()
        {
            //if (!_hasSaveData)
            //{
            //    ConfirmStartNewGame();
            //}
            //else
            //{
                ShowStartNewGameConfirmationPopup();
            //}
        }

        private void ConfirmStartNewGame()
        {
            _startNewGameEvent.Invoke();
        }

        private void ShowStartNewGameConfirmationPopup()
        {
            _popupPanel.ConfirmationResponseAction += StartNewGamePopupResponse;
            _popupPanel.ClosePopupAction += HidePopup;

            //_popupPanel.gameObject.SetActive(true);
            _popupPanel.ModalWindowIn();
            _popupPanel.SetPopup(PopupType.NewGame);
        }

        private void StartNewGamePopupResponse(bool startNewGameConfirmed)
        {
            _popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
            _popupPanel.ClosePopupAction -= HidePopup;

            //_popupPanel.gameObject.SetActive(false);
            _popupPanel.ModalWindowOut();

            if (startNewGameConfirmed)
            {
                ConfirmStartNewGame();
            }
            else
            {
                //_continueGameEvent.Invoke();
                _startNewGameEvent.Invoke();
            }

            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }

        private void HidePopup()
        {
            _popupPanel.ClosePopupAction -= HidePopup;
            //_popupPanel.gameObject.SetActive(false);
            _popupPanel.ModalWindowOut();
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }

        public void OpenSettingsScreen()
        {
            //_settingsPanel.gameObject.SetActive(true);
            //_settingsPanel.Closed += CloseSettingsScreen;

            PanelAnim(MenuType.SETTING);
            _settingsPanel.Closed += CloseSettingsScreen;
        }

        public void CloseSettingsScreen()
        {
            //_settingsPanel.Closed -= CloseSettingsScreen;
            //_settingsPanel.gameObject.SetActive(false);
            //_mainMenuPanel.SetMenuScreen(_hasSaveData);

            _settingsPanel.Closed -= CloseSettingsScreen;
            PanelAnim(MenuType.MENU);
        }

        public void OpenCreditsScreen()
        {
            //_creditsPanel.gameObject.SetActive(true);
            //_creditsPanel.OnCloseCredits += CloseCreditsScreen;
        }

        public void CloseCreditsScreen()
        {
            //_creditsPanel.OnCloseCredits -= CloseCreditsScreen;
            //_creditsPanel.gameObject.SetActive(false);
            //_mainMenuPanel.SetMenuScreen(_hasSaveData);
        }

        public void ShowExitConfirmationPopup()
        {
            _popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
            _popupPanel.gameObject.SetActive(true);
            _popupPanel.SetPopup(PopupType.Quit);
        }

        private void HideExitConfirmationPopup(bool quitConfirmed)
        {
            _popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
            _popupPanel.gameObject.SetActive(false);
            if (quitConfirmed)
            {
                Application.Quit();
            }
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }

        private void OnDestroy()
        {
            _popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
            _popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
        }

        public void PanelAnim(MenuType newPanel)
        {
            if (newPanel != currentPanelIndex)
            {
                GameObject currentPanel = panels[Convert.ToInt32(currentPanelIndex)];

                currentPanelIndex = newPanel;
                GameObject nextPanel = panels[Convert.ToInt32(currentPanelIndex)];

                Animator currentPanelAnimator = currentPanel.GetComponent<Animator>();
                Animator nextPanelAnimator = nextPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBrushAnimation == true)
                {
                    PanelBrushManager currentBrush = currentPanel.GetComponent<PanelBrushManager>();
                    if (currentBrush.brushAnimator != null)
                        currentBrush.BrushSplashOut();
                    PanelBrushManager nextBrush = nextPanel.GetComponent<PanelBrushManager>();
                    if (nextBrush.brushAnimator != null)
                        nextBrush.BrushSplashIn();
                }

                if (currentPanelIndex == 0 && enableHomeBlur == true)
                    homeBlurManager.BlurInAnim();
                else if (currentPanelIndex != 0 && enableHomeBlur == true)
                    homeBlurManager.BlurOutAnim();
            }
        }

        #region Test
        //[SerializeField] private UIPopup _popupPanel;

        //[Header("PANEL LIST")]
        //public List<GameObject> panels = new List<GameObject>();

        //[Header("RESOURCES")]
        //public BlurManager homeBlurManager;
        //public BlurManager windowBlurManager;

        //[Header("SETTINGS")]
        //public int currentPanelIndex = 0;
        //public bool enableBrushAnimation = true;
        //public bool enableHomeBlur = true;

        //private GameObject currentPanel;
        //private GameObject nextPanel;
        //private Animator currentPanelAnimator;
        //private Animator nextPanelAnimator;

        //private string panelFadeIn = "Panel In";
        //private string panelFadeOut = "Panel Out";

        //private PanelBrushManager currentBrush;
        //private PanelBrushManager nextBrush;

        ////[SerializeField] private UISettingsController _settingsPanel = default;
        ////[SerializeField] private UICredits _creditsPanel = default;
        ////[SerializeField] private UIMainMenu _mainMenuPanel = default;
        ////[SerializeField] private SaveSystem _saveSystem = default;
        //[SerializeField] private UIMainMenu _mainMenuPanel = default;

        //[SerializeField] private InputReader _inputReader = default;

        //[Header("Broadcasting on")]
        //[SerializeField]
        //private GameEventVoid _startNewGameEvent = default;
        //[SerializeField]
        //private GameEventVoid _continueGameEvent = default;

        //private bool _hasSaveData;

        //private IEnumerator Start()
        //{
        //    //_inputReader.EnableMenuInput();

        //    currentPanel = panels[currentPanelIndex];
        //    currentPanelAnimator = currentPanel.GetComponent<Animator>();
        //    currentPanelAnimator.Play(panelFadeIn);

        //    if (enableHomeBlur == true)
        //        homeBlurManager.BlurInAnim();

        //    yield return new WaitForSeconds(0.4f); // 장면 대기
        //    SetMenuScreen();
        //}

        //private void SetMenuScreen()
        //{
        //    _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
        //}

        //private void ButtonStartNewGameClicked()
        //{
        //    ShowStartNewGameConfirmationPopup();
        //}

        //private void ShowStartNewGameConfirmationPopup()
        //{
        //    _popupPanel.ConfirmationResponseAction += StartNewGamePopupResponse;
        //    _popupPanel.ClosePopupAction += HidePopup;

        //    // Popup 실행
        //    //_popupPanel.gameObject.SetActive(true);
        //    windowBlurManager.BlurInAnim();

        //    _popupPanel.SetPopup(PopupType.NewGame);
        //}

        //void StartNewGamePopupResponse(bool startNewGameConfirmed)
        //{
        //    _popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
        //    _popupPanel.ClosePopupAction -= HidePopup;

        //    //_popupPanel.gameObject.SetActive(false);

        //    //if (startNewGameConfirmed)
        //    //{
        //    //    //ConfirmStartNewGame();
        //    //}
        //    //else
        //    //{
        //    //    _continueGameEvent.Invoke();
        //    //}

        //    //_mainMenuPanel.SetMenuScreen(_hasSaveData);
        //}

        //private void HidePopup()
        //{
        //    _popupPanel.ClosePopupAction -= HidePopup;
        //    _popupPanel.gameObject.SetActive(false);
        //    _mainMenuPanel.SetMenuScreen(_hasSaveData);

        //}

        //public void PanelAnim(int newPanel)
        //{
        //    if (newPanel != currentPanelIndex)
        //    {
        //        currentPanel = panels[currentPanelIndex];

        //        currentPanelIndex = newPanel;
        //        nextPanel = panels[currentPanelIndex];

        //        currentPanelAnimator = currentPanel.GetComponent<Animator>();
        //        nextPanelAnimator = nextPanel.GetComponent<Animator>();
        //        currentPanelAnimator.Play(panelFadeOut);
        //        nextPanelAnimator.Play(panelFadeIn);

        //        if (enableBrushAnimation == true)
        //        {
        //            currentBrush = currentPanel.GetComponent<PanelBrushManager>();
        //            if (currentBrush.brushAnimator != null)
        //                currentBrush.BrushSplashOut();
        //            nextBrush = nextPanel.GetComponent<PanelBrushManager>();
        //            if (nextBrush.brushAnimator != null)
        //                nextBrush.BrushSplashIn();
        //        }

        //        if (currentPanelIndex == 0 && enableHomeBlur == true)
        //            homeBlurManager.BlurInAnim();
        //        else if (currentPanelIndex != 0 && enableHomeBlur == true)
        //            homeBlurManager.BlurOutAnim();
        //    }
        //}
        #endregion
    }
}
