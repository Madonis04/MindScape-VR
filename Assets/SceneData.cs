using System;
using System.Collections.Generic;

[Serializable]
public class SceneObject
{
    public string name;
    public string asset_id;
    public float[] position;
    public float[] scale;
    public float[] color; // Changed from 'string' to 'float[]'
}

[Serializable]
public class SceneScript
{
    public List<SceneObject> scene_objects;
}