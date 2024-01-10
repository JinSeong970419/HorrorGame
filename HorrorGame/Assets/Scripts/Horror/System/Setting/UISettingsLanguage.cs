using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Horror
{
    public class UISettingsLanguage : MonoBehaviour, IPointerExitHandler
    {
        [SerializeField] private UISettingItemFiller _languageField = default;

        [Header("Object")]
        [SerializeField] private GameObject triggerObject;
        [SerializeField] private GameObject _dropBox;
        [SerializeField] private Transform _dropDownitemParent;
        [SerializeField] private GameObject _dropDownitemPrefabs;
        [SerializeField] private GameObject scrollbar;

        public event UnityAction<Locale> _save = delegate { };

        // Lange Setting
        private int _currentSelectedOption = 0;
        private int _savedSelectedOption;
        private AsyncOperationHandle _initializeOperation;
        private List<string> _languagesList = new List<string>();

        // Anim Setting
        private Animator dropdownAnimator;

        public bool enableIcon = true;
        public bool enableTrigger = true;
        public bool enableScrollbar = true;
        public bool setHighPriorty = true;
        public bool outOnPointerExit;
        public bool isListItem;
        public bool invokeAtStart = false;

        private bool isOn;

        private void OnEnable()
        {
            _initializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (_initializeOperation.IsDone)
            {
                InitializeCompleted(_initializeOperation);
            }
            else
            {
                _initializeOperation.Completed += InitializeCompleted;
            }
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
        }

        private void Start()
        {
            dropdownAnimator = _dropBox.GetComponent<Animator>();
            SetupDropDownItem();

            if (enableScrollbar == true)
            {
                scrollbar.SetActive(true);
            }
            else
            {
                scrollbar.SetActive(false);
            }
        }

        private void InitializeCompleted(AsyncOperationHandle obj)
        {
            _initializeOperation.Completed -= InitializeCompleted;
            
            // 각 언어별 드롭다운 생성
            _languagesList = new List<string>();

            List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;

            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    _currentSelectedOption = i;

                var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
                _languagesList.Add(displayName);
            }
            _languageField.FillSettingDropDown(_languagesList.Count, _currentSelectedOption, _languagesList[_currentSelectedOption]);
            _savedSelectedOption = _currentSelectedOption;
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        private void LocalizationSettings_SelectedLocaleChanged(Locale locale)
        {
            // 드롭다운 메인 세팅
            var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            _languageField.FillSettingDropDown(_languagesList.Count, selectedIndex, _languagesList[selectedIndex]);
        }

        private void SetupDropDownItem()
        {
            foreach (Transform child in _dropDownitemParent)
                Destroy(child.gameObject);

            for (int i = 0; i < _languagesList.Count; ++i)
            {
                GameObject dropDownItem = Instantiate(_dropDownitemPrefabs, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                dropDownItem.transform.SetParent(_dropDownitemParent, false);

                dropDownItem.GetComponentInChildren<TextMeshProUGUI>().text = _languagesList[i];

                Button itemButton = dropDownItem.GetComponent<Button>();

                itemButton.onClick.AddListener(Animate);
                itemButton.onClick.AddListener(delegate
                {
                    OnSelectionChanged(dropDownItem.transform.GetSiblingIndex());
                    SaveSetting();
                });
            }
        }

        private void OnSelectionChanged(int currentSeletedOption)
        {
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

            _currentSelectedOption = currentSeletedOption;
            var locale = LocalizationSettings.AvailableLocales.Locales[_currentSelectedOption];

            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
            LocalizationSettings.SelectedLocale = locale;
        }

        private void SaveSetting()
        {
            Locale _currentLocale = LocalizationSettings.AvailableLocales.Locales[_currentSelectedOption];
            _savedSelectedOption = _currentSelectedOption;
            _save.Invoke(_currentLocale);
        }

        public void Animate()
        {
            if (isOn == false)
            {
                dropdownAnimator.Play("Dropdown In");
                isOn = true;
            }

            else if (isOn == true)
            {
                dropdownAnimator.Play("Dropdown Out");
                isOn = false;
            }

            if (enableTrigger == true && isOn == false)
                triggerObject.SetActive(false);

            else if (enableTrigger == true && isOn == true)
                triggerObject.SetActive(true);

            if (outOnPointerExit == true)
                triggerObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (outOnPointerExit == true)
            {
                if (isOn == true)
                {
                    Animate();
                    isOn = false;
                }

                if (isListItem == true)
                    gameObject.transform.SetParent(_dropBox.transform, true);
            }
        }
    }
}
