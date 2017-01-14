using UnityEditor;
using UnityEngine;

namespace AshleySeric.Secretary
{
	public class Secretary : EditorWindow
	{
		public bool dontPlayWhileCompiling = true;
		public bool colorizeBackground = true;
		public bool minimalDisplay = false;
		public Color compilingColor = Color.red;
		public Color finishedColor = Color.green;

		[MenuItem("Window/Ashley Seric/Secretary")]
		static void Init()
		{
			EditorWindow window = EditorWindow.GetWindowWithRect(typeof(Secretary), new Rect(0, 0, 165, 40));
			window.titleContent = new GUIContent("Secretary", "Created by Ashley Seric");
			window.Show();
		}
		void Awake()
		{
			PullPrefs();
		}
		public void OnGUI()
		{
			if (GUILayout.Button("Settings", EditorStyles.toolbarButton))
			{
				SecretarySettingsWindow settingsWindow = GetWindow<SecretarySettingsWindow>(true, "Secretary Settings", true);
				settingsWindow.secretary = this;
			} 
			if (colorizeBackground) GUI.backgroundColor = EditorApplication.isCompiling ? compilingColor : finishedColor;
			EditorGUILayout.HelpBox(EditorApplication.isCompiling ? "Compiling Scripts" : "Scripts Compiled", minimalDisplay ? MessageType.None : (EditorApplication.isCompiling ? MessageType.Warning : MessageType.Info));
		}
		void Update()
		{
			if (dontPlayWhileCompiling && EditorApplication.isCompiling)
			{
				EditorApplication.isPlaying = false;
			}
		}
		void OnInspectorUpdate()
		{
			Repaint();
		}
		public void PushPrefs()
		{
			EditorPrefs.SetBool("AshleySeric_Secretary_dontPlayWhileCompiling", dontPlayWhileCompiling);
			EditorPrefs.SetBool("AshleySeric_Secretary_colorizeBackground", colorizeBackground);
			EditorPrefs.SetBool("AshleySeric_Secretary_minimalDisplay", minimalDisplay);
			EditorPrefs.SetString("AshleySeric_Secretary_compilingColor", "#" + ColorUtility.ToHtmlStringRGBA(compilingColor));
			EditorPrefs.SetString("AshleySeric_Secretary_finishedColor", "#" + ColorUtility.ToHtmlStringRGBA(finishedColor));
		}
		public void PullPrefs()
		{
			dontPlayWhileCompiling = EditorPrefs.GetBool("AshleySeric_Secretary_dontPlayWhileCompiling", true);
			colorizeBackground = EditorPrefs.GetBool("AshleySeric_Secretary_colorizeBackground", true);
			minimalDisplay = EditorPrefs.GetBool("AshleySeric_Secretary_minimalDisplay", true);
			ColorUtility.TryParseHtmlString(EditorPrefs.GetString("AshleySeric_Secretary_compilingColor", "#ff0000"), out compilingColor);
			ColorUtility.TryParseHtmlString(EditorPrefs.GetString("AshleySeric_Secretary_finishedColor", "#00ff00"), out finishedColor);
		}
	}

	public class SecretarySettingsWindow : EditorWindow
	{
		public Secretary secretary;

		void OnGUI()
		{
			if (!secretary) secretary = EditorWindow.GetWindow(typeof(Secretary)) as Secretary;
			secretary.dontPlayWhileCompiling = GUILayout.Toggle(secretary.dontPlayWhileCompiling, "Block Playmode while compiling");
			secretary.minimalDisplay = GUILayout.Toggle(secretary.minimalDisplay, "Minimal Display");
			EditorGUILayout.Space();
			secretary.colorizeBackground = EditorGUILayout.BeginToggleGroup("Colorize Background", secretary.colorizeBackground);
			secretary.compilingColor = EditorGUILayout.ColorField(new GUIContent("Compiling Color"), secretary.compilingColor, true, false, false, null);
			secretary.finishedColor = EditorGUILayout.ColorField(new GUIContent("Finished Color"), secretary.finishedColor, true, false, false, null);
			EditorGUILayout.EndToggleGroup();
			EditorGUILayout.Space();
			if (GUILayout.Button("Ok"))
			{
				this.Close();
			}
		}
		void OnLostFocus()
		{
			secretary.PushPrefs();
		}
		void OnDestroy()
		{
			secretary.PushPrefs();
		}
	}
}