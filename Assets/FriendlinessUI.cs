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
        FriendlinessData data = Datamanager.Instance.saveData.friendlinessData;

        danWolSlider.value = data.DanWol;
        hongNyeonGwiSlider.value = data.HongNyeonGwi;
        yaSeoSlider.value = data.YaSeo;
        jeonSangYeonSlider.value = data.JeonSangYeon;
        maCheonGyoSlider.value = data.MaCheonGyo;
    }
}