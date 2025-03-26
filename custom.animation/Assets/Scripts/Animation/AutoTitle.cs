using System.Collections;
using TMPro;
using UnityEngine;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private float _letterBounceScale;
        [SerializeField] private float _letterBounceDuration;

        [HideInInspector] [SerializeField] private string _fullText;
        [HideInInspector] [SerializeField] private TMP_TextInfo _textInfo;

        private void Awake()
        {
            if (_textMeshPro == null)
            {
                _textMeshPro = GetComponent<TextMeshProUGUI>();
            }

            _fullText = _textMeshPro.text;
            _textMeshPro.ForceMeshUpdate();
            _textInfo = _textMeshPro.textInfo;
        }

        private void OnEnable()
        {
            RestartEffect();
        }

        private void RestartEffect()
        {
            StopAllCoroutines();
            _textMeshPro.text = "";
            _textMeshPro.ForceMeshUpdate();
            StartCoroutine(ShowText());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _textMeshPro.text = string.Empty;
        }

        private IEnumerator ShowText()
        {
            for (int i = 0; i < _fullText.Length; i++)
            {
                _textMeshPro.text = _fullText.Substring(0, i + 1);
                _textMeshPro.ForceMeshUpdate();
                _textInfo = _textMeshPro.textInfo;
                StartCoroutine(AnimateLetterScale(i));
                yield return new WaitForSeconds(_letterBounceDuration);
            }
        }

        private IEnumerator AnimateLetterScale(int index)
        {
            float time = 0f;
            while (time < _letterBounceDuration)
            {
                float scale = Mathf.Lerp(1f, _letterBounceScale, time / (_letterBounceDuration / 2));
                if (time >= _letterBounceDuration / 2)
                {
                    scale = Mathf.Lerp(_letterBounceScale, 1f, (time - (_letterBounceDuration / 2)) / (_letterBounceDuration / 2));
                }

                if (index < _textInfo.characterCount)
                {
                    var charInfo = _textInfo.characterInfo[index];
                    if (charInfo.isVisible)
                    {
                        int meshIndex = charInfo.materialReferenceIndex;
                        int vertexIndex = charInfo.vertexIndex;
                        Vector3[] vertices = _textInfo.meshInfo[meshIndex].vertices;

                        Vector3 center = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
                        for (int j = 0; j < 4; j++)
                        {
                            vertices[vertexIndex + j] = center + (vertices[vertexIndex + j] - center) * scale;
                        }
                        _textMeshPro.UpdateVertexData();
                    }
                }
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
