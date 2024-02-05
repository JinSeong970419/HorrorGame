using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;
using UnityEngine.UI;

namespace Horror
{
    public class UISettingItemFiller : MonoBehaviour
    {
        [SerializeField] private SettingFieldType _fieldType;
        [SerializeField] private LocalizeStringEvent _currentSelectedOption_LocalizedEvent;
        [SerializeField] private LocalizeStringEvent _title;
        [SerializeField] private TextMeshProUGUI _currentSelectedOption_Text;

        [SerializeField] private Slider _childrenSlider;

        private void OnValidate()
        {
            _childrenSlider = gameObject.GetComponentInChildren<Slider>();
        }

        public void FillSettingField_Localized(string selectedOption)
        {
            _title.StringReference.TableEntryReference = _fieldType.ToString();
            _currentSelectedOption_LocalizedEvent.StringReference.TableEntryReference = _fieldType + "_" + selectedOption;
        }

        public void FillSettingDropDown(string selectedOption_int)
        {
            _currentSelectedOption_Text.text = selectedOption_int.ToString();
        }

        public void FillSettingFieldSlider(string selectedOption_int, float selectedVolumeValue)
        {
            _title.StringReference.TableEntryReference = _fieldType.ToString();
            _currentSelectedOption_Text.text = selectedOption_int.ToString();
            
            if(_childrenSlider != null)
                _childrenSlider.value = selectedVolumeValue;
        }
    }
}
