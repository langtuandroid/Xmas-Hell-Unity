using UnityEditor;

[CustomPropertyDrawer(typeof(BossTypeToGameObjectDictionary))]
public class EnumGameObjectSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(BossTypeToSpriteDictionary))]
public class EnumSpriteSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(ScreenCornerToGameObjectDictionary))]
public class ScreenCornerGameObjectSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(StringToFloatDictionary))]
public class StringToFloatDictionarySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }