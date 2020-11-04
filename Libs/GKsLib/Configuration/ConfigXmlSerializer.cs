using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace GKsLib.Configuration
{
	/// <summary>設定情報を XML で出力する機能を提供します。</summary>
	/// <typeparam name="T">シリアライズするクラスの型。</typeparam>
	public class ConfigXmlSerializer<T> : IConfigSerializer<T>
		where T : class
	{
		#region Members

		/// <summary>ConfigXmlSerializer クラスの新しいインスタンスを作成します。</summary>
		public ConfigXmlSerializer()
		{
			XmlWriterSettings = new XmlWriterSettings { Indent = true };
			KnownTypes = new List<Type>();
		}

		/// <summary>XML の出力形式を制御するために使用するコンポーネントを取得または設定します。</summary>
		public XmlWriterSettings XmlWriterSettings { get; set; }

		/// <summary>既知のコントラクト型に xsi:type 宣言を動的にマップするのに使用するコンポーネントを取得または設定します。</summary>
		public DataContractResolver DataContractResolver { get; set; }

		/// <summary>DataContractSerializer のこのインスタンスを使用してシリアル化されるオブジェクト グラフ内に存在可能な型のコレクションを取得します。</summary>
		public IList<Type> KnownTypes { get; private set; }

		/// <summary>指定のインスタンスを保存します。</summary>
		/// <param name="path">シリアライズした内容を保存するパス。</param>
		/// <param name="instance">シリアライズする対象のインスタンス。</param>
		public void Serialize(string path, T instance)
		{
			var serializer = GetSerializer();
			using (MemoryStream stream = new MemoryStream())
			using (var writer = XmlWriter.Create(stream, XmlWriterSettings))
			{
				serializer.WriteObject(writer, instance);
				writer.Flush();
				string json = Encoding.UTF8.GetString(stream.ToArray());
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
		private DataContractSerializer GetSerializer()
		{
			return new DataContractSerializer(typeof(T), KnownTypes);
		}

		#endregion
	}
}
