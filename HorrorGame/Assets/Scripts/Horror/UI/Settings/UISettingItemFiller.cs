using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;

namespace Horror
{
    public class UISettingItemFiller : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _currentSelectedOption_LocalizedEvent;
        [SerializeField] private LocalizeStringEvent _title;
        [SerializeField] private TextMeshProUGUI _currentSelectedOption_Text;

        public void FillSettingDropDown(int paginationCount, int selectedPaginationIndex, string selectedOption_int)
        {
            _currentSelectedOption_Text.text = selectedOption_int.ToString();
        }
    }
}
