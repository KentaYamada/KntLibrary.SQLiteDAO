using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace KntLibrary.SQLiteDAO
{
	public class SqliteParamCreator
    {
        #region Fields

        private List<SQLiteParameter> _InnerParameters = null;

		public SQLiteParameter[] Parameters
		{
			get
			{
				return this._InnerParameters.ToArray();
			}
		}

        #endregion

        #region Constructors

        public SqliteParamCreator()
		{
			this._InnerParameters = new List<SQLiteParameter>();
		}

        #endregion

        #region Private Methods

        private object CvtDBNullValue(object value)
		{
            if (null == value)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
		}

        #endregion

        #region Public Methods

        public void Add(string parameterName, DbType type, object value)
		{
			var param = new SQLiteParameter();

			param.ParameterName = parameterName;
			param.DbType = type;
			param.Value = this.CvtDBNullValue(value);
			param.Direction = ParameterDirection.Input;

			this._InnerParameters.Add(param);
        }

        #endregion
    }
}
