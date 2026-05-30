using UnityEngine;

public class SwappableMaterial : MonoBehaviour
{
    [Header("변경할 캐릭터의 Mesh Renderer")]
    public SkinnedMeshRenderer characterRenderer;

    [Header("상황별 머티리얼 리스트")]
    public Material defaultMaterial; // 기본 상태 (둘리/희동이 원본 색상)
    public Material swappedMaterial; // 바뀐 상태 (Scene 8용 특수 색상/글리치 버전)

    // 이 함수를 호출하면 겉모습(재질)이 바뀝니다. (세은/승희가 코드 연결할 예정)
    public void ChangeMaterial(bool useSwapped)
    {
        if (characterRenderer == null) return;

        characterRenderer.material = useSwapped ? swappedMaterial : defaultMaterial;
        Debug.Log($"{gameObject.name}'s Material changed");
    }
}