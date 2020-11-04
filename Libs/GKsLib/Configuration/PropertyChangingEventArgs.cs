using System;

namespace GKsLib.Configuration
{

	/// <summary>PropertyChanging イベントのイベント情報を提供します。</summary>
	public class PropertyChangingEventArgs : EventArgs
	{
		/// <summary>指定されたパラメーターに基づいて、PropertyChangingEventArgs クラスの新しいインスタンスを作成します。</summary>
		/// <param name="property">変更されるプロパティ情報。</param>
		/// <param name="newValue">変更後のプロパティの値。</param>
		public PropertyChangingEventArgs(ConfigProperty property, object newValue)
		{
			_property = property;
			NewValue = newValue;
		}

		/// <summary>変更されるプロパティ情報。</summary>
		private readonly ConfigProperty _property;

		/// <summary>変更をキャンセルするかどうかを取得または設定します。</summary>
		public bool Cancel { get; set; }

		/// <summary>プロパティ名を取得します。</summary>
		public object Name { get { return _property.Name; } }

		/// <summary>変更前のプロパティの値を取得します。</summary>
		public object Value { get { return _property.Value; } }

		/// <summary>変更後のプロパティの値を取得します。</summary>
		public object NewValue { get; private set; }
	}
}
