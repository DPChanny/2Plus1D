using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool dieWhenCollide;

    [SerializeField]
    private GameObject powerSource;

    [SerializeField]
    private bool isReversePower;

    [SerializeField]
    private GameObject mesh;

    private void Update()
    {
        if (powerSource != null)
        {
            bool power = true;

            if (powerSource.TryGetComponent<SwitchSlab>(out SwitchSlab _switch))
            {
                power = _switch.isPowered;
            }
            if (powerSource.TryGetComponent<Slab>(out Slab _slab))
            {
                power = _slab.isPowered;
            }
            if (isReversePower)
            {
                mesh.SetActive(!power);
            }
            else
            {
                mesh.SetActive(power);
            }
        }
    }
}
