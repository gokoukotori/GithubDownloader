using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace GKsLib.Configuration
{
	public class ConfigBinarySerializer<T> : IConfigSerializer<T>
		where T : class
	{
		#region Members

		/// <summary>ConfigBinarySerializer クラスの新しいインスタンスを作成します。</summary>
		public ConfigBinarySerializer()
		{
			_binaryFormatter = new BinaryFormatter();
		}

		/// <summary>指定されたパラメーターに基づいて、PortableSettingsValue クラスの新しいインスタンスを作成します。</summary>
		/// <param name="selector">サロゲート セレクター。</param>
		/// <param name="context">シリアル化されたデータの転送元と転送先。</param>
		public ConfigBinarySerializer(ISurrogateSelector selector, StreamingContext context)
		{
			_binaryFormatter = new BinaryFormatter(selector, context);
		}

		/// <summary>バイナリシリアライズ機能を提供するクラス。</summary>
		private readonly BinaryFormatter _binaryFormatter;

		public bool IsCompression { get; set; }

		/// <summary>アセンブリの検索と読み込みに関するデシリアライザの動作を取得または設定します。</summary>
		public FormatterAssemblyStyle AssemblyFormat
		{
			get { return _binaryFormatter.AssemblyFormat; }
			set { _binaryFormatter.AssemblyFormat = value; }
		}

		/// <summary>シリアル化されたオブジェクトから型へのバインディングを制御する、SerializationBinder 型のオブジェクトを取得または設定します。</summary>
		public SerializationBinder Binder
		{
			get { return _binaryFormatter.Binder; }
			set { _binaryFormatter.Binder = value; }
		}


		/// <summary>対象のフォーマッタで使用する StreamingContext を取得または設定します。</summary>
		public StreamingContext Context
		{
			get { return _binaryFormatter.Context; }
			set { _binaryFormatter.Context = value; }
		}


		/// <summary>BinaryFormatter が実行する自動逆シリアル化の TypeFilterLevel を取得または設定します。</summary>
		public TypeFilterLevel FilterLevel
		{
			get { return _binaryFormatter.FilterLevel; }
			set { _binaryFormatter.FilterLevel = value; }
		}


		/// <summary>シリアル化中および逆シリアル化中に行われる型の置換を制御する ISurrogateSelector を取得または設定します。</summary>
		public ISurrogateSelector SurrogateSelector
		{
			get { return _binaryFormatter.SurrogateSelector; }
			set { _binaryFormatter.SurrogateSelector = value; }
		}


		/// <summary>シリアル化されたストリームにおける型の記述のレイアウト形式を取得または設定します。</summary>
		public FormatterTypeStyle TypeFormat
		{
			get { return _binaryFormatter.TypeFormat; }
			set { _binaryFormatter.TypeFormat = value; }
		}

		/// <summary>指定のインスタンスを保存します。</summary>
		/// <param name="path">シリアライズした内容を保存するパス。</param>
		/// <param name="instance">シリアライズする対象のインスタンス。</param>
		public void Serialize(string path, T instance)
		{
			using (var stream = GetStream(path, FileMode.Create, CompressionMode.Compress))
			{
				_binaryFormatter.Serialize(stream, instance);
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

			using (var stream = GetStream(path, FileMode.Open, CompressionMode.Decompress))
			{
				return _binaryFormatter.Deserialize(stream) as T;
			}
		}

		/// <summary>ストリームを取得します。</summary>
		/// <param name="path">リアライズまたはデシリアライズするファイルのパス。</param>
		/// <param name="fileMode">ファイルを開く方法。</param>
		/// <param name="compression">ストリームを圧縮するか圧縮解除するかどうか。</param>
		/// <returns>作成したストリーム。</returns>
		private Stream GetStream(string path, FileMode fileMode, CompressionMode compression)
		{
			Stream stream = File.Open(path, fileMode);
			if (IsCompression)
			{
				stream = new DeflateStream(stream, compression);
			}
			return stream;
		}

		#endregion
	}
}
