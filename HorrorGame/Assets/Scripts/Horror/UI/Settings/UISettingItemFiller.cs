using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;

namespace Horror
{
    public class UISettingItemFiller : MonoBehaviour
    {
        [SerializeField] private SettingFieldType _fieldType;
        [SerializeField] private LocalizeStringEvent _currentSelectedOption_LocalizedEvent;
        [SerializeField] private LocalizeStringEvent _title;
        [SerializeField] private TextMeshProUGUI _currentSelectedOption_Text;

        public void FillSettingDropDown(int paginationCount, int selectedPaginationIndex, string selectedOption_int)
        {
            _currentSelectedOption_Text.text = selectedOption_int.ToString();
        }

        public void FillSettingField(string selectedOption_int)
        {
            _title.StringReference.TableEntryReference = _fieldType.ToString();
            _currentSelectedOption_Text.text = selectedOption_int.ToString();
        }
    }
}
