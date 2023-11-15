using Unity.Collections;
using UnityEngine;

namespace Horror
{
    [CreateAssetMenu(fileName = "PathStorage", menuName = "Gameplay/Path Storage")]
    public class PathStorageSO : DescriptionBaseSO
    {
        [Space]
        [ReadOnly] public PathSO lastPathTaken;
    }
}
