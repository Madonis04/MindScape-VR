using UnityEngine;
using System.Collections.Generic;

public class SceneOrchestrator : MonoBehaviour
{
    // You can adjust this value if bonds are missing or if too many are created.
    // It represents the maximum distance between two atoms to be considered "bonded".
    public float bondDistanceThreshold = 1.8f;

    public void BuildScene()
    {
        Debug.Log("Starting to build scene from JSON...");

        // --- Step 1: Load and parse the JSON (Same as before) ---
        TextAsset jsonFile = Resources.Load<TextAsset>("scene_script");
        SceneScript script = JsonUtility.FromJson<SceneScript>(jsonFile.text);
        
        if (script == null || script.scene_objects == null)
        {
            Debug.LogError("JSON parsing failed. Check the JSON structure.");
            return;
        }

        // --- Step 2: Instantiate all atoms ---
        // We no longer need to look for a "central" atom.
        List<GameObject> createdAtoms = new List<GameObject>();
        foreach (SceneObject objData in script.scene_objects)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + objData.asset_id);
            if (prefab != null)
            {
                Vector3 pos = new Vector3(objData.position[0], objData.position[1], objData.position[2]);
                GameObject newAtom = Instantiate(prefab, pos, Quaternion.identity);
                newAtom.name = objData.name;
                newAtom.transform.localScale = new Vector3(objData.scale[0], objData.scale[1], objData.scale[2]);
                TrySetColor(newAtom, objData.color);
                createdAtoms.Add(newAtom);
            }
        }

        // --- Step 3: Create bonds based on proximity ---
        // This new logic compares every atom to every other atom.
        for (int i = 0; i < createdAtoms.Count; i++)
        {
            for (int j = i + 1; j < createdAtoms.Count; j++)
            {
                GameObject atomA = createdAtoms[i];
                GameObject atomB = createdAtoms[j];

                // Calculate the distance between the two atoms.
                float distance = Vector3.Distance(atomA.transform.position, atomB.transform.position);

                // If they are close enough, create a bond.
                if (distance <= bondDistanceThreshold)
                {
                    CreateBond(atomA, atomB);
                }
            }
        }
    }

    // The CreateBond and TrySetColor functions remain unchanged.
    void CreateBond(GameObject atomA, GameObject atomB)
    {
        GameObject bondPrefab = Resources.Load<GameObject>("Prefabs/Bond");
        if (bondPrefab == null) return;

        Vector3 midpoint = (atomA.transform.position + atomB.transform.position) / 2f;
        float distance = Vector3.Distance(atomA.transform.position, atomB.transform.position);

        GameObject newBond = Instantiate(bondPrefab, midpoint, Quaternion.identity);
        newBond.name = $"Bond_{atomA.name}-{atomB.name}";
        newBond.transform.up = atomB.transform.position - atomA.transform.position;
        newBond.transform.localScale = new Vector3(newBond.transform.localScale.x, distance / 2f, newBond.transform.localScale.z);
    }
    
    void TrySetColor(GameObject obj, float[] colorArray)
    {
        if (colorArray == null || colorArray.Length < 4) return;
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = new Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
        }
    }
}