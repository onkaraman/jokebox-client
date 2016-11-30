using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JokeBox.Core.Persistence.Models;
using SQLite;

namespace JokeBox.Core.Persistence.DB
{
	/// <summary>
	/// This class accesses to the established DB connection
    /// and uses it to do specific API operations.
	/// </summary>
	public class DBAccessor
	{
		private DBConnectionCore _db;
		private SQLiteConnection _conn
		{
			get { return _db.Connection; }
		}
                    
        public DBAccessor(DBConnectionCore DBConnection)
		{
			_db = DBConnection;
            create<SimpleItem>();
		}

        /// <summary>
        /// Will create a table of the passed type.
        /// Will return true if successful.
        /// </summary>
        private bool create<T>() where T : new()
        {
            try
            {
                if (IsEmpty<T>())
                {
                    _conn.CreateTable<T>();
                    return true;
                }
                return false;
            }
            catch (Exception) 
            {
                return false;
            }
        }

		/// <summary>
		/// Checks if model to add to the DB already exists.
		/// </summary>
        private bool exists<T>(T am) where T : new()
		{
            try
            {
                var table = Select<T>();

                foreach (var r in table)
                {
                    SimpleItem ex = (SimpleItem)Convert.ChangeType(r, typeof(T));
                    SimpleItem ne = (SimpleItem)Convert.ChangeType(am, typeof(T));

                    if (ex.Name == ne.Name) return true;
                }
                return false;
            }
            catch (Exception)
            {
                
            }
            return false;
		}

		#region Insertions
        /// <summary>
        /// Will insert the passed item to the table <T>.
        /// Will return true if successful.
        /// </summary>
        public bool Insert<T>(T a) where T : new()
        {
            try
            {
                if (!exists<T>(a))
                {
                    _conn.Insert(a);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Will insert a list of items to the table <T>.
        /// Will return false if at least one of the items
        /// could not be inserted.
        /// </summary>
        public bool Insert<T>(List<T> a) where T : new()
        {
            try
            {
                bool ret = true;
                foreach (T am in a)
                {
                    if (!Insert<T>(am)) ret = false;
                }
                return ret;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
		#endregion

		/// <summary>
		/// Returns the result of a sqli query.
		/// </summary>
		/// <param name="mode">Mode in which to make the select. Specific can only
		/// be passed, when SelectSpecs is given</param>
		public List<T> Select<T>(DBConnectionCore.SelectMode mode = DBConnectionCore.SelectMode.All, 
            DBSelectSpecs specs = null) where T : new()
		{
			List<T> l = new List<T>();

            try
            {
                if (mode == DBConnectionCore.SelectMode.All) foreach (var v in _conn.Table<T>()) l.Add(v);
                else if (mode == DBConnectionCore.SelectMode.Specific)
                {
                    if (specs == null)
                    {
                        throw new Exception("When querying with specifics, pass DBSelectSpecs object.");
                    }

                    //Get all content of table first, to filter out for resulting List.
                    var all = _conn.Table<T>();

                    foreach (var model in all)
                    {
                        if (specs.Max != 0)
                        {
                            l.Add(model);
                            if (l.Count <= specs.Max) break;
                        }
                    }

                    if (l.Count >= specs.Limit) l = l.Take(specs.Limit).ToList();
                }
            }
            catch (Exception)
            {
                
            }
            return l;
		}

		/// <summary>
		/// Returns the total size of given table.
		/// </summary>
		public int Size<T>() where T : new()
		{
            try
            {
                return _conn.Table<T>().Count();
            }
            catch (Exception)
            {
                Debug.WriteLine("Size empty");
            }
            return 0;
		}

        /// <summary>
        /// Returns true when the passed table is empty.
        /// </summary>
        /// <typeparam name="T">The table to check for emptiness.</typeparam>
        public bool IsEmpty<T>() where T : new()
        {
            try
            {
                return Size<T>() == 0;
            }
            catch (SQLiteException)
            {
                return true;
            }
        }

        /// <summary>
        /// Will delete all entries of this type.
        /// </summary>
        public bool Clear<T>() where T : new()
        {
            try
            {
                _conn.DeleteAll<T>();
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}

