using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Horror
{
    public class UISettingTabFiller : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _localizedTabNormalTitle;
        [SerializeField] private LocalizeStringEvent _localizedTabHighlightedTitle;
        //[SerializeField] private Image _bgSelectedTab;

        private SettingsType _currentTabType;

        public UnityAction<SettingsType> Clicked;

        public void SetTab(SettingsType settingTab, bool isSelected)
        {
            //_localizedTabNormalTitle.StringReference.TableEntryReference = settingTab.ToString();
            //_localizedTabHighlightedTitle.StringReference.TableEntryReference = settingTab.ToString();

            _currentTabType = settingTab;

            if (isSelected)
                SelectTab();
            else
                UnselectTab();
        }

        public void SetTab(SettingsType tabType)
        {
            bool isSelected = (_currentTabType == tabType);
            if (isSelected)
                SelectTab();
            else
                UnselectTab();
        }

        void SelectTab()
        {
            //_bgSelectedTab.enabled = true;
        }

        void UnselectTab()
        {
            //_bgSelectedTab.enabled = false;
        }

        public void Click()
        {
            Clicked.Invoke(_currentTabType);
        }
    }
}
