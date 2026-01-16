public class SpecialQuestInstance
{
    // ===== 기본 데이터 =====
    public SpecialQuestData data;
    public SpecialQuestUI ui;

    // ===== 진행 상태 =====
    public int breakCount;
    public int baseHeight;

    // ===== 타이머 =====
    public float remainSeconds;
    public int remainDropCount;

    // ===== 상태 =====
    public bool isFinished;
}