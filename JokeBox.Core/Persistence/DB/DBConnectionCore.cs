using SQLite;

namespace JokeBox.Core.Persistence.DB
{
    /// <summary>
    /// To be inherited by each platform and called asap.
    /// Relevant classes are DBAccessor, DBSelectSpecs.
    /// </summary>
	public abstract class DBConnectionCore
	{
        public enum SelectMode { All, Specific }
       	protected string dbPath;

		private string _dbName = "jokebox.db3";
		protected string dbName { get { return _dbName; } }

		public SQLiteConnection Connection;

		public DBConnectionCore()
		{
			setup();
		}

		/// <summary>
		/// Implement in specific platform for native folder path
		/// and native sqli client.
		/// </summary>
		protected abstract void setup();


	}
}

