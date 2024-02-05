using Michsky.UI.Dark;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;

namespace Horror
{
    [System.Serializable]
    public class ShadowDistanceTier
    {
        public float Distance;
        public string TierDescription;
    }

    public class UISettingsGraphics : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] private GameObject triggerObject;
        [SerializeField] private GameObject _dropBox;
        [SerializeField] private Transform _dropDownitemParent;
        [SerializeField] private GameObject _dropDownitemPrefabs;
        [SerializeField] private GameObject scrollbar;

        private Transform currentListParent;
        [HideInInspector] public int siblingIndex = 0;

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

        [Header("Option Settings")]
        [SerializeField] private List<ShadowDistanceTier> _shadowDistanceTierList = new List<ShadowDistanceTier>();
        [SerializeField] private UniversalRenderPipelineAsset _uRPAsset;

        [Header("Field Settings")]
        [SerializeField] private UISettingItemFiller _fullscreenField;

        [Header("CustomDropdown")]
        [SerializeField] private CustomDropdown Test;

        private int _savedResolutionIndex;
        private bool _savedFullscreenState;
        private bool _isFullscreen;

        public event UnityAction<int, int, float, bool> _save = delegate { };

        private int _currentResolutionIndex;
        private List<Resolution> _resolutionsList;
        [SerializeField] private UISettingItemFiller _resolutionsField;
        private Resolution _currentResolution;

        private string _dropTag_WindowMode = "Window Mode";

        public void Init()
        {
            currentListParent = _dropBox.transform.parent;

            _resolutionsList = GetResolutionsList();

            _currentResolution = Screen.currentResolution;
            _currentResolutionIndex = GetCurrentResolutionIndex();
            _isFullscreen = GetCurrentFullscreenState();

            // DropBox
            //Test.dropdownItems[PlayerPrefs.GetInt(_dropTag_WindowMode)].OnItemSelection.AddListener(OnFullscreenChange);
            for (int i = 0; i < Test.dropdownItems.Count; i++)
            {
                Test.dropdownItems[i].OnItemSelection.AddListener(OnFullscreenChange);
            }

            // Save Data
            _savedResolutionIndex = _currentResolutionIndex;
            _savedFullscreenState = _isFullscreen;
        }

        public void Setup()
        {
            Init();
            SetResolutionField();
            SetFullscreen();
        }

        #region Resolution
        private void SetResolutionField()
        {
            string displayText = _resolutionsList[_currentResolutionIndex].ToString();
            displayText = displayText.Substring(0, displayText.IndexOf("@"));

            _resolutionsField.FillSettingDropDown(displayText);

            // 각 해상도 드롭다운 생성
            SetupDropDownItem();
        }

        /// <summary>
        /// 해상도 드롭다운 생성
        /// </summary>
        private void SetupDropDownItem()
        {
            foreach (Transform child in _dropDownitemParent)
                Destroy(child.gameObject);

            for (int i = 0; i < _resolutionsList.Count; ++i)
            {
                GameObject dropDownItem = Instantiate(_dropDownitemPrefabs, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                dropDownItem.transform.SetParent(_dropDownitemParent, false);

                dropDownItem.GetComponentInChildren<TextMeshProUGUI>().text = _resolutionsList[i].ToString();

                Button itemButton = dropDownItem.GetComponent<Button>();

                itemButton.onClick.AddListener(Animate);
                itemButton.onClick.AddListener(delegate
                {
                    //OnResolutionChange(dropDownItem.transform.GetSiblingIndex());
                    //SaveSetting();
                });
            }
        }

        /// <summary>
        /// 해상도 목록 호출
        /// </summary>
        private List<Resolution> GetResolutionsList()
        {
            List<Resolution> options = new List<Resolution>();
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                options.Add(Screen.resolutions[i]);
            }

            return options;
        }
        private int GetCurrentResolutionIndex()
        {
            if (_resolutionsList == null)
            { _resolutionsList = GetResolutionsList(); }
            int index = _resolutionsList.FindIndex(o => o.width == _currentResolution.width && o.height == _currentResolution.height);
            return index;
        }

        private void OnResolutionChange()
        {
            _currentResolution = _resolutionsList[_currentResolutionIndex];
            Screen.SetResolution(_currentResolution.width, _currentResolution.height, _isFullscreen);
            SetResolutionField();
        }

        
        public void Animate()
        {
            if (dropdownAnimator == null)
                dropdownAnimator = _dropBox.GetComponent<Animator>();

            if (isOn == false)
            {
                dropdownAnimator.Play("Dropdown In");
                isOn = true;

                if (isListItem == true)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    _dropBox.transform.SetParent(gameObject.transform.parent, true);
                }
            }

            else if (isOn == true)
            {
                dropdownAnimator.Play("Dropdown Out");
                isOn = false;

                if (isListItem == true)
                {
                    _dropBox.transform.SetParent(currentListParent, true);
                    _dropBox.transform.SetSiblingIndex(siblingIndex);
                }
            }

            if (enableTrigger == true && isOn == false)
                triggerObject.SetActive(false);

            else if (enableTrigger == true && isOn == true)
                triggerObject.SetActive(true);

            if (outOnPointerExit == true)
                triggerObject.SetActive(false);
        }
        #endregion
        #region FullScrean
        private void SetFullscreen()
        {
            if (_isFullscreen)
            {
                _fullscreenField.FillSettingField_Localized("On");
            }
            else
            {
                _fullscreenField.FillSettingField_Localized("Off");
            }
        }

        private bool GetCurrentFullscreenState()
        {
            return Screen.fullScreen;
        }

        public void OnFullscreenChange()
        {
            // 기타 다른 모드
            //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            //Screen.fullScreenMode = FullScreenMode.Windowed;
            //Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            //Screen.fullScreenMode = FullScreenMode.MaximizedWindow;

            _isFullscreen = !_isFullscreen;
            Screen.fullScreen = _isFullscreen;
            SetFullscreen();
        }
        #endregion
    }
}
