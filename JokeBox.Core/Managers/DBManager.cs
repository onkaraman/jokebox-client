

using JokeBox.Core.Patterns;
using JokeBox.Core.Persistence.DB;

namespace JokeBox.Core.Managers
{
	/// <summary>
	/// This class manages the created db connection and its accessor.
	/// </summary>
	public class DBManager : LazyStatic<DBManager>
	{
		//Needs to be referenced to a db connection when the app starts.
		private DBConnectionCore _db;

		//Gets created with the referencing of the db connection.
		private DBAccessor _accessor;
		public DBAccessor DBAccessor
		{
			get { return _accessor; }
		}

        public void Init(DBConnectionCore db)
        {
            _db = db;
            _accessor = new DBAccessor(_db);
        }

		public DBManager ()
		{}
	}
}

