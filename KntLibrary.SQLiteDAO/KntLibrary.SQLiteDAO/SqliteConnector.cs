using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace KntLibrary.SQLiteDAO
{
	/// <summary>
	/// SQLite接続クラス
	/// </summary>
	internal sealed class SqliteConnector : IDisposable
	{
		private SQLiteConnection _InnerConnection = null;

        internal SQLiteConnection Connection
		{
			get
			{
				return this._InnerConnection;
			}
		}

        internal SqliteConnector()
		{
			this._InnerConnection = new SQLiteConnection();
            this._InnerConnection.ConnectionString = string.Format("Data Source={0}", ConfigurationManager.AppSettings["ConnectString"]);
		}

        internal SqliteConnector(string databasePath)
		{
            this._InnerConnection = new SQLiteConnection(string.Format("Data Source={0}" + databasePath));
		}
		
		public void Open()
		{
			if(string.IsNullOrEmpty(this._InnerConnection.ConnectionString))
			{
				throw new SQLiteException("接続文字列が空白または設定に誤りがあります。");
			}

			if (this._InnerConnection.State != ConnectionState.Open)
			{
				this._InnerConnection.Open();
			}
		}

		public void Close()
		{
			if ( (null != this._InnerConnection) && (this._InnerConnection.State != ConnectionState.Closed) )
			{
				this._InnerConnection.Close();
			}
		}

		public void Dispose()
		{
			if ( (this._InnerConnection != null) && (this._InnerConnection.State != ConnectionState.Closed) )
			{
				this._InnerConnection.Close();
			}

			this._InnerConnection.Dispose();
			this._InnerConnection = null;
		}
	}
}
