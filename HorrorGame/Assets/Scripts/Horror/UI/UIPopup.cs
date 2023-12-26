using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Horror
{
    public enum PopupButtonType
    {
        Confirm,
        Cancel,
        Close,
        DoNothing,
    }

    public enum PopupType
    {
        Quit,
        NewGame,
        BackToMenu,
    }

    public class UIPopup : MonoBehaviour
    {
        [Header("BRUSH ANIMATION")]
        public Animator brushAnimator;
        public bool enableSplash = true;

        private Animator mWindowAnimator;

        [Header("PopupSetting")]
        //[SerializeField] private InputReader _inputReader;

        [SerializeField] private LocalizeStringEvent _titleText;
        [SerializeField] private LocalizeStringEvent _descriptionText;
        //[SerializeField] private Button _buttonClose;
        [SerializeField] private UIGenericButton _popupButton1;
        [SerializeField] private UIGenericButton _popupButton2;

        private PopupType _actualType;

        public event UnityAction<bool> ConfirmationResponseAction;
        public event UnityAction ClosePopupAction;

        private void Start()
        {
            mWindowAnimator = gameObject.GetComponent<Animator>();
        }

        private void OnDisable()
        {
            _popupButton2.Clicked -= CancelButtonClicked;
            _popupButton1.Clicked -= ConfirmButtonClicked;
            //_inputReader.MenuCloseEvent -= ClosePopupButtonClicked;
        }

        public void ModalWindowIn()
        {
            mWindowAnimator.Play("Modal Window In");

            if (enableSplash == true)
            {
                brushAnimator.Play("Transition Out");
            }
        }

        public void ModalWindowOut()
        {
            mWindowAnimator.Play("Modal Window Out");

            if (enableSplash == true)
            {
                brushAnimator.Play("Transition In");
            }
        }

        public void SetPopup(PopupType popupType)
        {
            _actualType = popupType;
            bool isConfirmation = false;
            bool hasExitButton = false;
            //_titleText.StringReference.TableEntryReference = _actualType.ToString() + "_Popup_Title";
            //_descriptionText.StringReference.TableEntryReference = _actualType.ToString() + "_Popup_Description";
            //string tableEntryReferenceConfirm = PopupButtonType.Confirm + "_" + _actualType;
            //string tableEntryReferenceCancel = PopupButtonType.Cancel + "_" + _actualType;
            _titleText.StringReference.TableEntryReference = "AUDIO";
            _descriptionText.StringReference.TableEntryReference = "AUDIO";
            string tableEntryReferenceConfirm = "AUDIO";
            string tableEntryReferenceCancel = "AUDIO";
            switch (_actualType)
            {
                case PopupType.NewGame:
                case PopupType.BackToMenu:
                    isConfirmation = true;

                    _popupButton1.SetButton(tableEntryReferenceConfirm, false);
                    _popupButton2.SetButton(tableEntryReferenceCancel, false);
                    hasExitButton = true;
                    break;
                case PopupType.Quit:
                    isConfirmation = true;
                    _popupButton1.SetButton(tableEntryReferenceConfirm, true);
                    _popupButton2.SetButton(tableEntryReferenceCancel, false);
                    hasExitButton = false;
                    break;
                default:
                    isConfirmation = false;
                    hasExitButton = false;
                    break;
            }

            if (isConfirmation) // 투 버튼용
            {
                _popupButton1.gameObject.SetActive(true);
                _popupButton2.gameObject.SetActive(true);

                _popupButton1.Clicked += CancelButtonClicked;
                _popupButton2.Clicked += ConfirmButtonClicked;
            }
            else // 원 버튼용
            {
                _popupButton1.gameObject.SetActive(true);
                _popupButton2.gameObject.SetActive(false);

                _popupButton1.Clicked += ConfirmButtonClicked;
            }

            //_buttonClose.gameObject.SetActive(hasExitButton);

            if (hasExitButton)
            {
                //_inputReader.MenuCloseEvent += ClosePopupButtonClicked;
            }
        }

        public void ClosePopupButtonClicked()
        {
            ClosePopupAction.Invoke();
        }

        void ConfirmButtonClicked()
        {
            ConfirmationResponseAction.Invoke(true);
        }

        void CancelButtonClicked()
        {
            ConfirmationResponseAction.Invoke(false);
        }
    }
}
