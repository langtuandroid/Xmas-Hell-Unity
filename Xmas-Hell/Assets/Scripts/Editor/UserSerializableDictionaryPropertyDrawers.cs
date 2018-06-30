using UnityEditor;

[CustomPropertyDrawer(typeof(BossTypeToPrefabDictionary))]
public class EnumGameObjectSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(BossTypeToBossBallDictionary))]
public class EnumSpriteSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(ScreenCornerToGameObjectDictionary))]
public class ScreenCornerGameObjectSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }