using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace GKsLib.Configuration
{

	/// <summary>クラスのプロパティ情報を提供します。</summary>
	[DebuggerDisplay("{DebuggerString}")]
	public class ConfigProperty
	{
		#region Members

		/// <summary>指定されたパラメーターに基づいて、PortableSettingsValue クラスの新しいインスタンスを作成します。</summary>
		/// <param name="propertyInfo">プロパティ情報。</param>
		/// <param name="value">プロパティの値。</param>
		public ConfigProperty(PropertyInfo propertyInfo, object value)
		{
			PropertyInfo = propertyInfo;
			Name = propertyInfo.Name;
			Value = value;
			IsDirty = false;
		}

		/// <summary>プロパティ情報を取得します。</summary>
		public PropertyInfo PropertyInfo { get; private set; }

		/// <summary>プロパティ名を取得します。</summary>
		public string Name { get; private set; }

		/// <summary>プロパティの値を取得または設定します。</summary>
		public object Value { get; set; }

		/// <summary>値の更新状態を取得または設定します。</summary>
		public bool IsDirty { get; set; }

		/// <summary>デバッグ用のインスタンス文字列を取得します。</summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		private string DebuggerString
		{
			get { return (Value == null) ? "null" : "\"" + Value.ToString() + "\""; }
		}

		#endregion
	}
}
