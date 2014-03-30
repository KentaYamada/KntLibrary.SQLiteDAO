using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Transactions;

namespace KntLibrary.SQLiteDAO
{
	public static class SqliteCommander
	{
        private static void SetParameters(SQLiteCommand command, SQLiteParameter[] args)
        {
            command.Parameters.Clear();

            if ((null != args) && (0 < args.Length))
            {
                command.Parameters.AddRange(args);
            }
        }
		
		public static DataTable Select(string query, params SQLiteParameter[] args)
		{
			using (var connector = new SqliteConnector())
			using (var command = new SQLiteCommand(query, connector.Connection))
			{
				SqliteCommander.SetParameters(command, args);

				using (var adapt = new SQLiteDataAdapter(command))
				{
					var table = new DataTable();

					adapt.Fill(table);

					return table;
				}
			}
		}

        /// <summary>
        /// データ登録
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
		public static int Save(List<string> queries)
		{
            int affectedRow = 0;

			foreach (string query in queries)
            {
                affectedRow += SqliteCommander.Save(query);
            }

            return affectedRow;
		}

		/// <summary>
		/// データ登録
		/// </summary>
		/// <param name="query">SQL文字列</param>
		/// <param name="args">パラメータ</param>
		/// <returns>True:正常終了 / False:エラー</returns>
		public static int Save(string query, params SQLiteParameter[] args)
		{
			using (var connector = new SqliteConnector())
			using (var command = new SQLiteCommand(query, connector.Connection))
			{
				SqliteCommander.SetParameters(command, args);

				connector.Open();
                
                int result = 0;

                using (var tran = new TransactionScope())
                {
                    result = command.ExecuteNonQuery();

                    if (0 < result)
                    {
                        tran.Complete();
                    }
                }

				connector.Close();

				return result;
			}
		}

		/// <summary>
		/// データ削除
		/// </summary>
		public static int Delete(List<string> queries)
		{
			return SqliteCommander.Save(queries);
		}

		/// <summary>
		/// データ削除
		/// </summary>
		public static int Delete(string query, params SQLiteParameter[] args)
		{
			return SqliteCommander.Save(query, args);
		}

        /// <summary>
        /// 単一行取得
        /// </summary>
        /// <param name="query">SQL文字列</param>
        /// <param name="args">パラメータ</param>
        /// <returns>件数</returns>
        public static int Scalor(string query, params SQLiteParameter[] args)
        {
            using (var connector = new SqliteConnector())
            using (var command = new SQLiteCommand(query, connector.Connection))
            {
				SqliteCommander.SetParameters(command, args);

                connector.Open();

                int count = (int)command.ExecuteScalar();

                connector.Close();

                return count;
            }
		}

		/// <summary>
		/// テーブル作成クエリ実行
		/// </summary>
		/// <param name="query">テーブル作成クエリ</param>
		/// <returns>True:成功</returns>
		/// <exception cref="System.Data.SQLite.SQLiteException">
		/// クエリ実行に失敗した場合、例外をスローします。
		/// </exception>
		public static bool ExecuteCreateTable(string query)
		{
			try
			{
				using (var connector = new SqliteConnector())
				using (var command = new SQLiteCommand(query, connector.Connection))
				{
					connector.Open();

					using (var tran = new TransactionScope())
					{
						command.ExecuteNonQuery();
						tran.Complete();
					}

					connector.Close();
				}

				return true;
			}
			catch
			{
				throw new SQLiteException("テーブル作成に失敗しました。");
			}
		}
	}
}
