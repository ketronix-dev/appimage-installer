using Microsoft.Data.Sqlite;
using ServiceClasses;

public class CloudRepoUtils
{
    public static SqliteConnection Connection = new SqliteConnection($"Data Source={$"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps"}/repo.base;");

    public static App? ReadAppInfo(SqliteConnection conn, string appName)
    {
        conn.Open();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Apps WHERE ShortName=@app";
        cmd.Parameters.AddWithValue("@app",appName);
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                reader.Read();
                var name = reader.GetString(0);
                var shortName = reader.GetString(1);
                var maintainer = reader.GetString(2);
                var news = reader.GetString(3);
                var link = reader.GetString(4);
                var version = reader.GetString(5);

                return new App() {Name = name, ShortName = shortName, Maintainer = maintainer, NewVersionSources = news, Link = link, Version = version};
            }
            else
            {
                return null;
            }
        }
    }

    public static bool checkApp(SqliteConnection conn, string appName)
    {
        conn.Open();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Apps WHERE ShortName=@id";
        cmd.Parameters.AddWithValue("@id",appName);
        SqliteDataReader reader = cmd.ExecuteReader();  
        if(reader.HasRows)return true;
        else return false;
    }
}