#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    [CustomPropertyDrawer(typeof(SoundData))]
    public class SoundDataDrawer : PropertyDrawer
    {
        private const float VolumeLabelWidth = 50f;
        private const float ClipLabelWidth = 30f;
        private const float LinesAmount = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * LinesAmount;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float lineHeight = position.height / LinesAmount;

            float clipX = position.x;
            Rect clipLabelRect = new Rect(clipX, position.y, ClipLabelWidth, lineHeight);
            Rect clipRect = new Rect(clipX += ClipLabelWidth, position.y, position.width - ClipLabelWidth, lineHeight);

            EditorGUI.LabelField(clipLabelRect, new GUIContent("Clip"));
            EditorGUI.PropertyField(clipRect, property.FindPropertyRelative("_clip"), GUIContent.none);

            float volumeX = position.x;
            float volumeY = position.y + lineHeight;
            Rect volumeLabelRect = new(volumeX, volumeY, VolumeLabelWidth, lineHeight);
            Rect volumeRect = new(volumeX += VolumeLabelWidth, volumeY, position.width - VolumeLabelWidth, lineHeight);

            EditorGUI.LabelField(volumeLabelRect, new GUIContent("Volume"));
            EditorGUI.Slider(volumeRect, property.FindPropertyRelative("_volume"), 0, 1, GUIContent.none);
            
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
#endif