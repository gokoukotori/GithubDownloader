using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace GKsLib.Configuration
{

	/// <summary>設定情報を JSON で出力する機能を提供します。</summary>
	/// <typeparam name="T">シリアライズするクラスの型。</typeparam>
	public class ConfigJsonSerializer<T> : IConfigSerializer<T>
		where T : class
	{
		#region Members

		/// <summary>ConfigJsonSerializer クラスの新しいインスタンスを作成します。</summary>
		public ConfigJsonSerializer()
		{
			KnownTypes = new List<Type>();
		}

		/// <summary>DataContractJsonSerializer のこのインスタンスを使用してシリアル化されるオブジェクト グラフ内に存在可能な型のコレクションを取得します。</summary>
		public IList<Type> KnownTypes { get; private set; }

		/// <summary>指定のインスタンスを保存します。</summary>
		/// <param name="path">シリアライズした内容を保存するパス。</param>
		/// <param name="instance">シリアライズする対象のインスタンス。</param>
		public void Serialize(string path, T instance)
		{
			var serializer = GetSerializer();
			using (MemoryStream ms = new MemoryStream())
			{
				serializer.WriteObject(ms, instance);
				string json = Encoding.UTF8.GetString(ms.ToArray());
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				File.WriteAllText(path, json);
			}
		}

		/// <summary>指定のパスからインスタンスを取得します。</summary>
		/// <param name="path">デシリアライズする内容を読み込むパス。</param>
		/// <returns>デシリアライズしたインスタンス。</returns>
		public T? Desilialize(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}

			var serializer = GetSerializer();
			byte[] bytes = Encoding.UTF8.GetBytes(File.ReadAllText(path, Encoding.UTF8));
			using (var stream = new MemoryStream(bytes))
			{
				return (T)serializer.ReadObject(stream);
			}
		}

		/// <summary>シリアライザのインスタンスを取得します。</summary>
		/// <returns>取得したシリアライザのインスタンス。</returns>
		private DataContractJsonSerializer GetSerializer()
		{
			return new DataContractJsonSerializer(typeof(T), KnownTypes);
		}

		#endregion
	}
}
