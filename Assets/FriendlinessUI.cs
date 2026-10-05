using UnityEngine;
using UnityEngine.UI;

public class FriendlinessUI : MonoBehaviour
{
    [Header("슬라이더")]
    public Slider danWolSlider;
    public Slider hongNyeonGwiSlider;
    public Slider yaSeoSlider;
    public Slider jeonSangYeonSlider;
    public Slider maCheonGyoSlider;

    [Header("최대 우호도")]
    public int maxFriendliness = 9;

    private void Start()
    {
        danWolSlider.minValue = 0;
        hongNyeonGwiSlider.minValue = 0;
        yaSeoSlider.minValue = 0;
        jeonSangYeonSlider.minValue = 0;
        maCheonGyoSlider.minValue = 0;

        // 슬라이더 최대값 설정
        danWolSlider.maxValue = maxFriendliness;
        hongNyeonGwiSlider.maxValue = maxFriendliness;
        yaSeoSlider.maxValue = maxFriendliness;
        jeonSangYeonSlider.maxValue = maxFriendliness;
        maCheonGyoSlider.maxValue = maxFriendliness;

        UpdateUI();
    }

    public void UpdateUI()
    {
        RelationshipData data = Datamanager.Instance.saveData.relationship;

        danWolSlider.value = data.danwol;
        hongNyeonGwiSlider.value = data.hongryeon;
        yaSeoSlider.value = data.yaseo;
        jeonSangYeonSlider.value = data.JeonSangYeon;
        maCheonGyoSlider.value = data.macheon;
    }
}