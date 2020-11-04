namespace GKsLib.Configuration
{
	public interface IConfigSerializer<T> where T : class
	{
		/// <summary>指定のインスタンスを保存します。</summary>
		/// <param name="path">シリアライズした内容を保存するパス。</param>
		/// <param name="instance">シリアライズする対象のインスタンス。</param>
		void Serialize(string path, T instance);

		/// <summary>指定のパスからインスタンスを取得します。</summary>
		/// <param name="path">デシリアライズする内容を読み込むパス。</param>
		/// <returns>デシリアライズしたインスタンス。</returns>
		T Desilialize(string path);
	}
}
