using UnityEngine;
public class MaterialSwapper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Material[] _materials;
    [SerializeField] private int _selectedMaterialIndexStart;
    
    [Header("References")]
    [SerializeField] private MeshRenderer _meshRenderer;


    private void Start()
    {
        _selectedMaterialIndexStart = Mathf.Clamp(_selectedMaterialIndexStart, 0, _materials.Length - 1);
        SwapMaterials();
    }

    public void SwapMaterials()
    {
        _meshRenderer.material = _materials[_selectedMaterialIndexStart];
        _selectedMaterialIndexStart = (_selectedMaterialIndexStart + 1) % _materials.Length;
    }
}