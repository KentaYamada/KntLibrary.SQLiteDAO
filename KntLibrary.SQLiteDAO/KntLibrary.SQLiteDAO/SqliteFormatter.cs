using System;

namespace KntLibrary.SQLiteDAO
{
	public class SqliteFormatter
	{
		public static string CvtSqlChar(object value)
		{
			return string.Format("'{0}'", value);
		}
	}
}
