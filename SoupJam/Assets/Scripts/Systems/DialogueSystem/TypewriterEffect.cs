using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [System.Serializable]
    private struct Punctuation
    {
        public List<char> punctuations;
        public float waitTime;
    }

    [Header("Settings")]
    [Tooltip("Delay between letters typed in seconds")]
    [SerializeField] private float delay;

    [Tooltip("Special delay timings for punctuation")]
    [SerializeField] private List<Punctuation> punctuations = new List<Punctuation>();

    //vars
    private Coroutine typingCoroutine;
    //states
    public bool isRunning { get; private set; }

    //------------------manage state------------------------
    public void Run(string msg, TMP_Text label)
    {
        isRunning = true;
        typingCoroutine = StartCoroutine(TypeText(msg, label));
    }

    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }

    //---------------run effect---------------
    private IEnumerator TypeText(string msg, TMP_Text label)
    {
        //reset label
        label.text = string.Empty;
        //type one character at a time
        for (int i = 0; i < msg.Length; i++) {
            label.text = msg.Substring(0, i + 1);
            //punctuation check
            bool isLast = i == msg.Length - 1;
            if (!isLast && IsPunctuation(msg[i], out float waitTime) && !IsPunctuation(msg[i + 1], out _)) {
                yield return new WaitForSeconds(waitTime);
            }
            //generic pause
            yield return new WaitForSeconds(delay);
        }
        //end
        isRunning = false;
    }

    //---------------Punctuation------------------
    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation puncCategory in punctuations) {
            if (puncCategory.punctuations.Contains(character)) {
                waitTime = puncCategory.waitTime;
                return true;
            }
        }
        waitTime = default;
        return false;
    }
}
