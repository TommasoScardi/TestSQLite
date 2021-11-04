using System;
using System.Data.SQLite;

namespace TestSQLite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing SQLite DB");

            //SQLiteConnection.CreateFile("TestDatabase.db");
            //Console.WriteLine("DB File Created");
            string dbConnString = @"Data Source = .\TestDatabase.db; Version = 3;";

            using (SQLiteConnection dbConn = new SQLiteConnection(dbConnString))
            {
                dbConn.Open();

                using (SQLiteCommand dbCmd = new SQLiteCommand(dbConn))
                {
                    //Creazione nuova tabella se non esiste
                    //string sql = "CREATE TABLE IF NOT EXISTS highscores (name varchar(20), score int)";
                    string sql = @"CREATE TABLE IF NOT EXISTS articles (
                                    articlesId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    articlesTitle TEXT NOT NULL,
                                    articlesAuthor TEXT NOT NULL,
                                    articlesData TEXT NOT NULL,
                                    articlesText TEXT NOT NULL
                                   );";
                    dbCmd.CommandText = sql;
                    Console.WriteLine( dbCmd.ExecuteNonQuery() > 0 ? "Tabella articles creata": "Tabella articles gia presente all interno del DB");

                    //Cancellazione di tutti i record presenti nella tabella
                    sql = "DELETE FROM articles;";
                    dbCmd.CommandText = sql;
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine("Tabella articles svuotata completamente");

                    //Inserimento di un Record in una tabella nel modo classico
                    sql = @"INSERT INTO articles (
                                articlesTitle,
                                articlesAuthor,
                                articlesData,
                                articlesText
                            )

                            VALUES(
                                'test da console classico',
                                'tester',
                                datetime('now', 'localtime'),
                                'eseguito un test da console di invio di una query di inserimento'
                            ); ";
                    dbCmd.CommandText = sql;
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine("Record Aggiunto alla tabella articles tramite metodo classico");

                    //Inserimento di un Record in una tabella rimpiazzando i placeholder con il valore di una variabile
                    sql = @"INSERT INTO articles (
                                articlesTitle,
                                articlesAuthor,
                                articlesData,
                                articlesText
                            )

                            VALUES(
                                @articlesTitle,
                                @articlesAuthor,
                                datetime('now', 'localtime'),
                                @articlesText
                            ); ";
                    dbCmd.CommandText = sql;
                    dbCmd.Parameters.AddWithValue("articlesTitle",  _ = "test da console con placeholder");
                    dbCmd.Parameters.AddWithValue("articlesAuthor", _ = "tester");
                    dbCmd.Parameters.AddWithValue("articlesText", _ = "eseguito un test da console di invio di una query di inserimento con placeholder nella query che sono stati rimpiazzati correttamente dal compilatore in fase RUN-TIME");
                    dbCmd.ExecuteNonQuery();
                    Console.WriteLine("Record Aggiunto alla tabella articles tramite placeholder");

                    //Lettura di dei Record da una tabella secondo le condizioni specificate
                    sql = "SELECT * FROM articles";
                    dbCmd.CommandText = sql;
                    using (SQLiteDataReader queryRes = dbCmd.ExecuteReader())
                    {
                        while (queryRes.Read())
                        {
                            Console.WriteLine($"articleId: {queryRes["articlesId"]} \n articleTitle: {queryRes["articlesTitle"]} \n articleAuthor: {queryRes["articlesAuthor"]} \n articleData: {queryRes["articlesData"]} \n articleText: {queryRes["articlesText"]} \nEND ARTICLE\n");
                        }
                    }
                }

                dbConn.Close();
            }

            Console.ReadKey();
        }
    }
}
