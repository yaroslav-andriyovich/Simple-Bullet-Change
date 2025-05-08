using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class UIStatusBar : MonoBehaviour
    {
        [SerializeField] private Image _fillingImage;
        [SerializeField] private TMP_Text _textInfo;

        public void ReportProgress(float percent) => 
            _fillingImage.fillAmount = Mathf.Clamp01(percent);

        public void UpdateTextInfo(string text) => 
            _textInfo.text = text;
    }
}