using UnityEngine;

// ---------------
//  EBoss => GameObject
// ---------------
[UnityEditor.CustomPropertyDrawer(typeof(BossTypePrefabDictionary))]
public class BossTypePrefabDictionaryDrawer : SerializableDictionaryDrawer<EBoss, GameObject> {
    protected override SerializableKeyValueTemplate<EBoss, GameObject> GetTemplate() {
        return GetGenericTemplate<SerializableBossTypePrefabTemplate>();
    }
}
internal class SerializableBossTypePrefabTemplate : SerializableKeyValueTemplate<EBoss, GameObject> {}
