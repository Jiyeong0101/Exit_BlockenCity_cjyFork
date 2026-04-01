[System.Serializable]
public class DialogLine
{
    public int branch;
    public int backGround;
    public string ocp;
    public int characterImageID;
    public string name;
    public string dialog;
    public int questionType;
    public int acceptBranch;
    public int declineBranch;

    public DialogLine(int branch, int backGround, int characterImageID, string ocp, string name, string dialog, int questionType, int acceptBranch, int declineBranch)
    {
        this.branch = branch;
        this.backGround = backGround;
        this.characterImageID = characterImageID;
        this.ocp = ocp;
        this.name = name;
        this.dialog = dialog;
        this.questionType = questionType;
        this.acceptBranch = acceptBranch;
        this.declineBranch = declineBranch;
    }
}
