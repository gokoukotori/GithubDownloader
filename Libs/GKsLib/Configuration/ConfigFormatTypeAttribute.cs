using System;

namespace GKsLib.Configuration
{

	/// <summary>そのクラスをシリアライズまたはデシリアライズするときの種類（XML、JSON、BINARY）を指定します。</summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ConfigFormatTypeAttribute : Attribute
	{
		#region Members

		/// <summary>ConfigFormatTypeAttribute クラスの新しいインスタンスを作成します。</summary>
		public ConfigFormatTypeAttribute()
		{
			ConfigType = ConfigType.JSON;
		}

		/// <summary>設定ファイルのあるディレクトリ名。</summary>
		public virtual ConfigType ConfigType { get; set; }

		#endregion
	}
}
