using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScVFXController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioSource typingSFX;

    [SerializeField]
    private AudioSource typingEndSFX;

    [Header("Text")]
    [SerializeField]
    private Text text1;

    [SerializeField]
    private Text text2;

    [SerializeField]
    private Text text3;

    [Header("Typing Settings")]
    [Min(0.001f)]
    [SerializeField]
    private float typingSpeed = 0.08f;

    [SerializeField]
    private string cursorChar = "|";

    [Min(0.01f)]
    [SerializeField]
    private float cursorBlinkRate = 0.5f;

    private Coroutine sequenceCoroutine;
    private Coroutine blinkCoroutine;

    private string finalText1 = string.Empty;
    private string finalText2 = string.Empty;
    private string finalText3 = string.Empty;

    public bool IsPlaying =>
        sequenceCoroutine != null;


    public void PlaySequence(
        string message1,
        string message2,
        string message3)
    {
        StopSequence();

        finalText1 = message1 ?? string.Empty;
        finalText2 = message2 ?? string.Empty;
        finalText3 = message3 ?? string.Empty;

        if (text1 != null)
        {
            text1.text = string.Empty;
        }

        if (text2 != null)
        {
            text2.text = string.Empty;
        }

        if (text3 != null)
        {
            text3.text = string.Empty;
        }

        sequenceCoroutine = StartCoroutine(
            SequenceRoutine()
        );
    }

    private IEnumerator SequenceRoutine()
    {
        yield return TypeSingle(
            text1,
            finalText1,
            false
        );

        yield return TypeSingle(
            text2,
            finalText2,
            false
        );

        yield return TypeSingle(
            text3,
            finalText3,
            true
        );

        sequenceCoroutine = null;
    }

    private IEnumerator TypeSingle(
        Text target,
        string message,
        bool allowBlink)
    {
        if (target == null)
        {
            yield break;
        }

        string currentText = string.Empty;

        if (typingSFX != null)
        {
            typingSFX.Play();
        }

        foreach (char character in message)
        {
            currentText += character;
            target.text =
                currentText + cursorChar;

            yield return new WaitForSecondsRealtime(
                typingSpeed
            );
        }

        if (typingSFX != null)
        {
            typingSFX.Stop();
        }

        if (typingEndSFX != null)
        {
            typingEndSFX.Play();
        }

        if (allowBlink)
        {
            blinkCoroutine = StartCoroutine(
                CursorBlink(
                    target,
                    currentText
                )
            );
        }
        else
        {
            target.text = currentText;
        }
    }

    private IEnumerator CursorBlink(
        Text target,
        string finalMessage)
    {
        bool visible = true;

        while (true)
        {
            target.text = visible
                ? finalMessage + cursorChar
                : finalMessage +
                  "<color=#00000000>" +
                  cursorChar +
                  "</color>";

            visible = !visible;

            yield return new WaitForSecondsRealtime(
                cursorBlinkRate
            );
        }
    }

    public void CompleteImmediately()
    {
        StopSequence();

        if (text1 != null)
        {
            text1.text = finalText1;
        }

        if (text2 != null)
        {
            text2.text = finalText2;
        }

        if (text3 != null)
        {
            text3.text = finalText3;
        }
    }

    public void StopSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (typingSFX != null)
        {
            typingSFX.Stop();
        }

        if (typingEndSFX != null)
        {
            typingEndSFX.Stop();
        }
    }

    private void OnDisable()
    {
        StopSequence();
    }
}