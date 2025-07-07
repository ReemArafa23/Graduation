using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OrganDisassembler : MonoBehaviour
{
    [Header("Disassembly Settings")]
    public List<Transform> disassemblyParts;
    public float moveDistance = 0.15f;
    public float moveDuration = 0.6f;
    public Vector3[] moveDirections;

    [Header("UI Buttons")]
    public Button nextButton;
    public Button backButton;

    private List<Vector3> originalPositions = new List<Vector3>();
    private int currentLevel = 0;
    private bool isAnimating = false;

    public static OrganDisassembler Instance;

    private void Start()
    {
        Instance = this;

        foreach (var part in disassemblyParts)
            originalPositions.Add(part.localPosition);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextStep);

        if (backButton != null)
            backButton.onClick.AddListener(PreviousStep);
    }

    public void NextStep()
    {
        if (isAnimating || currentLevel >= disassemblyParts.Count)
            return;

        StartCoroutine(MovePart(disassemblyParts[currentLevel], moveDirections[currentLevel], true));
    }

    public void PreviousStep()
    {
        if (isAnimating || currentLevel <= 0)
            return;

        currentLevel--;
        StartCoroutine(MovePart(disassemblyParts[currentLevel], moveDirections[currentLevel], false));
    }

    private IEnumerator MovePart(Transform part, Vector3 direction, bool forward)
    {
        isAnimating = true;

        Vector3 startPos = part.localPosition;
        Vector3 targetPos = forward
            ? originalPositions[currentLevel] + direction.normalized * moveDistance
            : originalPositions[currentLevel];

        float elapsed = 0;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            part.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            yield return null;
        }

        part.localPosition = targetPos;

        if (forward)
            currentLevel++;

        isAnimating = false;
    }

    public bool IsPartDisassembled(Transform part)
    {
        int index = disassemblyParts.IndexOf(part);
        return index < currentLevel;
    }
}
