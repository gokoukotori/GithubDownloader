using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GKsLib.Configuration
{

	/// <summary>設定ファイルの機能を提供する基本クラス。</summary>
	/// <typeparam name="T">設定情報を格納するクラス。</typeparam>
	[DataContract, Serializable]
	public abstract class ConfigBase<T> : INotifyPropertyChanged, IExtensibleDataObject where T : ConfigBase<T>
	{
		#region Fields

		/// <summary>インスタンスメンバーへのアクセスをロックするかどうか。</summary>
		[NonSerialized]
		private bool _isSynchronized;

		/// <summary>読み込んだ設定ファイルのパス。</summary>
		[NonSerialized]
		private string? _loadedFilePath;

		/// <summary>設定ファイルのシリアライズ・デシリアライズの機能を提供するクラス。</summary>
		[NonSerialized]
		private IConfigSerializer<T>? _serializer;

		/// <summary>プロパティの名前と値を格納するコレクション。</summary>
		[NonSerialized]
		private Dictionary<string, ConfigProperty>? _properties;

		/// <summary>新しいメンバーの追加によって拡張されたデータを格納するためのコンテナ。</summary>
		[NonSerialized]
		private ExtensionDataObject? _extensionData;

		/// <summary>デシリアライズが完了しているかどうかを示すフラグ。</summary>
		[NonSerialized]
		private bool _isLoaded;

		#endregion

		#region Property

		/// <summary>プロパティ情報を取得します。</summary>
		/// <param name="name">プロパティ名。</param>
		/// <returns>プロパティ情報を格納する </returns>
		public virtual ConfigProperty this[string name]
		{
			get { return GetProperties()[name]; }
		}

		/// <summary>コレクションを反復処理する列挙子を返します。</summary>
		public IEnumerator<ConfigProperty> GetEnumerator()
		{
			return GetProperties().Values.GetEnumerator();
		}

		/// <summary>インスタンスメンバーへのアクセスをロックするかどうかを取得または設定します。</summary>
		public virtual bool IsSynchronized
		{
			get { return _isSynchronized; }
			set { _isSynchronized = value; }
		}

		#endregion

		#region EventHandler

		public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs args);

		[NonSerialized]
		private PropertyChangingEventHandler? _propertyChanging;

		/// <summary>プロパティが変更される直前に発生します。</summary>
		public event PropertyChangingEventHandler PropertyChanging
		{
			add { _propertyChanging += value; }
			remove { _propertyChanging -= value; }
		}

		/// <summary>PropertyChanging イベントを発生させます。</summary>
		/// <param name="args">イベント情報。</param>
		protected virtual void OnPropertyChanging(PropertyChangingEventArgs args)
		{
			if (_propertyChanging != null)
			{
				_propertyChanging(this, args);
			}
		}

		[NonSerialized]
		private PropertyChangedEventHandler? _propertyChanged;

		/// <summary>プロパティが変更された直後に発生します。</summary>
		public event PropertyChangedEventHandler PropertyChanged
		{
			add { _propertyChanged += value; }
			remove { _propertyChanged -= value; }
		}

		/// <summary>PropertyChanged イベントを発生させます。</summary>
		/// <param name="args">イベント情報。</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			if (_propertyChanged != null)
			{
				_propertyChanged(this, args);
			}
		}

		[NonSerialized]
		private EventHandler? _loaded;

		/// <summary>設定を読み込み終った直後に発生します。</summary>
		public event EventHandler Loaded
		{
			add { _loaded += value; }
			remove { _loaded -= value; }
		}

		/// <summary>Loaded イベントを発生させます。</summary>
		/// <param name="args">イベント情報。</param>
		protected virtual void OnLoaded(EventArgs args)
		{
			if (_loaded != null)
			{
				_loaded(this, args);
			}
		}

		[NonSerialized]
		private CancelEventHandler? _saving;

		/// <summary>設定を保存する直前に発生します。</summary>
		public event CancelEventHandler Saving
		{
			add { _saving += value; }
			remove { _saving -= value; }
		}

		/// <summary>Saving イベントを発生させます。</summary>
		/// <param name="args">イベント情報。</param>
		protected virtual void OnSaving(CancelEventArgs args)
		{
			if (_saving != null)
			{
				_saving(this, args);
			}
		}

		#endregion

		#region Get / Set

		/// <summary>プロパティの値を取得します。</summary>
		/// <typeparam name="TValue">プロパティの型。</typeparam>
		/// <param name="expression">プロパティを参照するラムダ式 (a => a.Property)。</param>
		/// <returns>プロパティから取得した値。</returns>
		protected virtual TValue Get<TValue>(Expression<Func<T, TValue>> expression)
		{
			if (IsSynchronized)
			{
				lock (this)
				{
					return GetPropertyValue(expression);
				}
			}
			else
			{
				return GetPropertyValue(expression);
			}
		}

		/// <summary>プロパティの値を設定します。</summary>
		/// <typeparam name="TValue">プロパティの型。</typeparam>
		/// <param name="expression">プロパティを参照するラムダ式 (a => a.Property)。</param>
		/// <param name="value">プロパティに設定する値。</param>
		protected virtual void Set<TValue>(Expression<Func<T, TValue>> expression, TValue value)
		{
			if (IsSynchronized)
			{
				lock (this)
				{
					SetPropertyValue(expression, value);
				}
			}
			else
			{
				SetPropertyValue(expression, value);
			}
		}

		/// <summary>プロパティの値を取得します。</summary>
		/// <typeparam name="TValue">プロパティの型。</typeparam>
		/// <param name="expression">プロパティを参照するラムダ式 (a => a.Property)。</param>
		/// <returns>プロパティから取得した値。</returns>
		private TValue GetPropertyValue<TValue>(Expression<Func<T, TValue>> expression)
		{
			var member = GetMemberInfo(expression);
			if (member.MemberType != MemberTypes.Property)
			{
				throw new ArgumentException("expression がプロパティの参照式ではありません。");
			}

			var name = member.Name;
			if (GetProperties().ContainsKey(member.Name) == false)
			{
				GetProperties().Add(name, new ConfigProperty(member as PropertyInfo, default(TValue)));
			}

			return (TValue)GetProperties()[name].Value;
		}

		/// <summary>プロパティの値を設定します。</summary>
		/// <typeparam name="TValue">プロパティの型。</typeparam>
		/// <param name="expression">プロパティを参照するラムダ式 (a => a.Property)。</param>
		/// <param name="value">プロパティに設定する値。</param>
		private void SetPropertyValue<TValue>(Expression<Func<T, TValue>> expression, TValue value)
		{
			var member = GetMemberInfo(expression);
			if (member.MemberType != MemberTypes.Property)
			{
				throw new ArgumentException("expression がプロパティの参照式ではありません。");
			}

			var name = member.Name;
			if (!GetProperties().ContainsKey(member.Name))
			{
				GetProperties().Add(name, new ConfigProperty(member as PropertyInfo, default(TValue)));
			}

			var prop = GetProperties()[name];
			var args = new PropertyChangingEventArgs(prop, value);
			OnPropertyChanging(args);
			if (args.Cancel)
			{
				return;
			}

			prop.Value = value;
			prop.IsDirty = _isLoaded;
			OnPropertyChanged(new PropertyChangedEventArgs(name));
		}

		/// <summary>ラムダ式で参照しているプロパティ情報を取得します。</summary>
		/// <typeparam name="TValue">プロパティの型。</typeparam>
		/// <param name="expression">プロパティを参照するラムダ式 (a => a.Property)。</param>
		/// <returns>プロパティ情報。</returns>
		private MemberInfo GetMemberInfo<TValue>(Expression<Func<T, TValue>> expression)
		{
			return (expression.Body as MemberExpression).Member;
		}

		/// <summary>プロパティの名前と値を格納するコレクションを取得します。</summary>
		private Dictionary<string, ConfigProperty> GetProperties()
		{
			if (_properties == null)
			{
				_properties = new Dictionary<string, ConfigProperty>();
			}
			return _properties;
		}

		#endregion

		#region Load / Save

		/// <summary>指定の設定ファイルからデシリアライズしてインスタンスを取得します。</summary>
		/// <param name="path">設定ファイルのパス。省略した場合は ConfigXmlSerializer の既定値を使用します。</param>
		/// <param name="serializer">デシリアライズに使用するクラス。省略した場合は ConfigXmlSerializer を使用します。</param>
		/// <returns>デシリアライズしたインスタンス。</returns>
		protected static T Load(string? path = null, IConfigSerializer<T>? serializer = null)
		{
			path ??= GetSettingsFilePath();

			switch (GetConfigType())
			{
				case ConfigType.XML:
					serializer ??= new ConfigXmlSerializer<T>();
					break;
				case ConfigType.JSON:
					serializer ??= new ConfigJsonSerializer<T>();
					break;
				case ConfigType.BINARY:
					serializer ??= new ConfigBinarySerializer<T>();
					break;
				default:
					serializer ??= new ConfigXmlSerializer<T>();
					break;
			}

			T self = serializer.Desilialize(path) ?? Activator.CreateInstance(typeof(T), true) as T;

			self._loadedFilePath = path;
			self._serializer = serializer;
			self.OnLoaded(EventArgs.Empty);
			self._isLoaded = true;
			return self;
		}

		/// <summary>設定プロパティの現在の値を格納します。</summary>
		/// <param name="path">保存先のパス。省略した場合は Load メソッドで読み込んだファイルのパスを使用します。</param>
		/// <param name="serializer">シリアライズの機能を提供するクラス。省略した場合は Load メソッドで使用したクラスを使用します。</param>
		/// <returns>成功した場合は true、キャンセルした場合は false。</returns>
		public virtual bool Save(string? path = null, IConfigSerializer<T>? serializer = null)
		{
			if (IsSynchronized)
			{
				lock (this)
				{
					return SaveCore(path, serializer);
				}
			}
			else
			{
				return SaveCore(path, serializer);
			}
		}

		/// <summary>設定プロパティの現在の値を格納します。</summary>
		/// <param name="path">保存先のパス。</param>
		/// <param name="serializer">シリアライズの機能を提供するクラス。</param>
		/// <returns>成功した場合は true、キャンセルした場合は false。</returns>
		private bool SaveCore(string path, IConfigSerializer<T> serializer)
		{
			var args = new CancelEventArgs();
			OnSaving(args);
			if (args.Cancel)
			{
				return false;
			}

			path ??= _loadedFilePath;
			serializer ??= _serializer;
			serializer.Serialize(path, this as T);
			_ = GetProperties().Select(a => a.Value.IsDirty = false);
			return true;
		}

		/// <summary>クラスに付与されている属性から設定ファイルのパスを取得します。</summary>
		/// <returns>設定ファイルのパス。</returns>
		private static string GetSettingsFilePath()
		{
			var type = typeof(T);
			var attrs = type.GetCustomAttributes(typeof(ConfigPathAttribute), false);
			var attr = (attrs.Length > 0) ? (attrs[0] as ConfigPathAttribute) : new ConfigPathAttribute();
			return attr.FullPath;
		}
		/// <summary>クラスに付与されている属性から設定ファイルのフォーマットを取得します。</summary>
		/// <returns>設定ファイルのフォーマット。</returns>
		private static ConfigType GetConfigType()
		{
			var type = typeof(T);
			var attrs = type.GetCustomAttributes(typeof(ConfigFormatTypeAttribute), false);
			var attr = (attrs.Length > 0) ? (attrs[0] as ConfigFormatTypeAttribute) : new ConfigFormatTypeAttribute();
			return attr.ConfigType;
		}

		/// <summary>デシリアライズが完了しているかどうかを取得します。</summary>
		protected bool GetIsLoaded() => _isLoaded;

		#endregion

		#region IExtensibleDataObject

		/// <summary>新しいメンバーの追加によって拡張されたデータを格納するためのコンテナを取得または設定します。</summary>
		[IgnoreDataMember, XmlIgnore]
		ExtensionDataObject IExtensibleDataObject.ExtensionData { get => _extensionData; set => _extensionData = value; }

		#endregion
	}
}
