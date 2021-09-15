using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;

    public bool IsRunning
    {
        get;
        private set;
    }

    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation( new HashSet<char>() {'.', '!', '?'}, 0.6f),
        new Punctuation(new HashSet<char>() {',', ';', ':'}, 0.3f)
    };

    private Coroutine typingCoroutine;

    public void Run(string textToType, TMP_Text textLabel)
    {
        typingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        IsRunning = false;
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        IsRunning = true;

        textLabel.text = string.Empty;

        float time = 0;
        int characterIndex = 0;

        while (characterIndex < textToType.Length)
        {
            int lastCharacterIndex = characterIndex;

            time += Time.deltaTime * typeWriterSpeed;
            characterIndex = Mathf.FloorToInt(time);
            characterIndex = Mathf.Clamp(characterIndex, 0, textToType.Length);

            for (int i = lastCharacterIndex; i < characterIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;

                textLabel.text = textToType.Substring(0, i + 1);

                textLabel.text = textToType.Substring(0, characterIndex);

                if (IsPunctuation(textToType[i], out float waitTime) && !isLast && !IsPunctuation(textToType[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }

            yield return null;
        }

        IsRunning = false;
    }

    public bool IsPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }

    }
}
