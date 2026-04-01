using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogManager : MonoBehaviour
{
    public HashSet<int> allBranchIDs = new HashSet<int>();
    public HashSet<int> spawnableBranchIDs = new HashSet<int>();

    private Queue<DialogLine> dialogQueue = new Queue<DialogLine>();
    private List<DialogLine> allDialogs = new List<DialogLine>();

    public static DialogManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadDialogCSV("DialogDB"); // Resources/DialogDB.csv
    }

    public void LoadDialogCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        StringReader reader = new StringReader(csvFile.text);

        bool isFirstLine = true;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            // 첫 줄이 헤더면 건너뛴다
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            string[] values = line.Split(',');
            System.Array.Resize(ref values, 9);

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
            }

            int branch = int.Parse(values[0]);
            int backGround = string.IsNullOrEmpty(values[1]) ? 0 : int.Parse(values[1]);

            string ocp = values[2];

            int characterImageID = 0;
            if (!string.IsNullOrEmpty(values[3]))
                int.TryParse(values[3], out characterImageID);

            string name = values[4];
            string dialog = values[5];

            int questionType = 0;
            if (!string.IsNullOrEmpty(values[6]))
                int.TryParse(values[6], out questionType);

            int acceptBranch = -1;
            int declineBranch = -1;

            if (!string.IsNullOrEmpty(values[7]))
                int.TryParse(values[7], out acceptBranch);

            if (!string.IsNullOrEmpty(values[8]))
                int.TryParse(values[8], out declineBranch);

            allDialogs.Add(new DialogLine(branch, backGround, characterImageID,ocp, name, dialog,questionType, acceptBranch, declineBranch));
           
            spawnableBranchIDs.Add(branch);
        }
    }

    public void LoadDialogByBranch(int branchID)
    {
        dialogQueue.Clear();

        foreach (var dialog in allDialogs)
        {
            if (dialog.branch == branchID)
            {
                dialogQueue.Enqueue(dialog);
            }
        }

        Debug.Log($"[DialogManager] branch {branchID} 로드 / {dialogQueue.Count}개 라인");
    }

    public DialogLine GetNextDialog()
    {
        if (dialogQueue.Count > 0)
        {
            return dialogQueue.Dequeue();
        }
        return null;
    }

    public bool HasBranch(int id)
    {
        return allBranchIDs.Contains(id);
    }

    public bool HasMoreDialog()
    {
        return dialogQueue.Count > 0;
    }

    int GetHundreds(int value) => (value / 100) % 10;
    int GetTens(int value) => (value / 10) % 10;
    int GetOnes(int value) => value % 10;

    int GetDigitSimilarity(int target, int candidate)
    {
        int diff =
            Mathf.Abs(GetHundreds(target) - GetHundreds(candidate)) * 3 +
            Mathf.Abs(GetTens(target) - GetTens(candidate)) * 2 +
            Mathf.Abs(GetOnes(target) - GetOnes(candidate)) * 1;

        return diff;
    }

    public int FindMostSimilarBranch(int targetID, int baseID)
    {
        int bestID = -1;
        int bestScore = int.MaxValue;

        foreach (int id in allBranchIDs)
        {
            // 같은 월 범위만
            if (id < baseID || id >= baseID + 1000)
                continue;

            int score = GetDigitSimilarity(targetID, id);

            if (score < bestScore)
            {
                bestScore = score;
                bestID = id;
            }
        }

        return bestID;
    }

    public List<int> GetSpawnableBranchIDs()
    {
        return new List<int>(spawnableBranchIDs);
    }

    public void ClearQueue()  // ⬅️ 거절 등으로 대화 중단 시 사용
    {
        dialogQueue.Clear();
    }

}
