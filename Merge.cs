using UnityEngine;

public class MeshMerger : MonoBehaviour
{
    [ContextMenu("Merge Children Meshes")]
    public void Merge()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].transform == transform)
            {
                i++;
                continue;
            }

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false); // optional
            i++;
        }

        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();

        mf.mesh = new Mesh();
        mf.mesh.CombineMeshes(combine);

        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>();

        // Optionally copy material from first child
        if (meshFilters.Length > 1 && meshFilters[1].GetComponent<MeshRenderer>() != null)
            mr.material = meshFilters[1].GetComponent<MeshRenderer>().sharedMaterial;

        Debug.Log("✅ Meshes merged into: " + gameObject.name);
    }
}
