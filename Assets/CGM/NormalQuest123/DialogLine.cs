[System.Serializable]
public class DialogLine
{
    public int branch;
    public string name;
    public string dialog;
    public int questionType;
    public int acceptBranch;
    public int declineBranch;

    public DialogLine(int branch, string name, string dialog, int questionType, int acceptBranch = -1, int declineBranch = -1)
    {
        this.branch = branch;
        this.name = name;
        this.dialog = dialog;
        this.questionType = questionType;
        this.acceptBranch = acceptBranch;
        this.declineBranch = declineBranch;
    }
}
