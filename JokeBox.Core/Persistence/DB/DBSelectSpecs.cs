using JokeBox.Core.Persistence.Models;

namespace JokeBox.Core.Persistence.DB
{
	/// <summary>
	/// This class should be used to specify the table request.
	/// Given fields will be used to filter, when assigned to.
	/// </summary>
	public class DBSelectSpecs
	{
		public enum SortOrder { Ascending, Descending }

		private SimpleItem _model;
		public SimpleItem Model { get { return _model; } }

		private int _max;
		public int Max { get { return _max; } }

        private int _limit;
		public int Limit { get {return _limit;}}

		public DBSelectSpecs (SimpleItem model, int max = 0, 
			int limit = 0)
		{
			_model = model;
			if (max != 0) _max = max;
			if (limit != 0) _limit = limit;
		}
	}
}

