using Microsoft.Data.Sqlite;
using ServiceClasses;

public class DataBaseUtils
{
    public static SqliteConnection Connection = new SqliteConnection($"Data Source={$"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps"}/installed_apps.db;");

    public static void CreateTableOrNo(SqliteConnection conn)
    {
        conn.Open();
        SqliteCommand command = new SqliteCommand();
        command.Connection = conn;
        command.CommandText = "CREATE TABLE Apps(PackageName TEXT NOT NULL, DesktopFile TEXT NOT NULL, AppImageFile TEXT NOT NULL, Version TEXT NOT NULL)";
        command.ExecuteNonQuery();
 
        Console.WriteLine("The database was successfully created.");
    }

    public static void InsertApp(SqliteConnection conn, string nameApp, string desktopFile, string appImageFile, string version)
    {
        conn.Open();
 
        SqliteCommand command = new SqliteCommand();
        command.Connection = conn;
        command.CommandText = "INSERT INTO Apps (PackageName, DesktopFile, AppImageFile, Version) VALUES (@name, @desktop, @appimage, @version)";
        command.Parameters.AddWithValue("@name", nameApp);
        command.Parameters.AddWithValue("@desktop", desktopFile);
        command.Parameters.AddWithValue("@appimage", appImageFile);
        command.Parameters.AddWithValue("@version", version);
        int number = command.ExecuteNonQuery();
    }
    public static void DeleteApp(SqliteConnection conn, string name)
    {
        conn.Open();
 
        SqliteCommand command = new SqliteCommand();
        command.Connection = conn;
        command.CommandText = "DELETE FROM Apps WHERE PackageName=@id";
        command.Parameters.AddWithValue("@id",name);
        command.ExecuteNonQuery();
    }

    public static List<AppInfo> GetAllApps(SqliteConnection conn)
    {
        var listItems = new List<AppInfo>() { };
        string sql = "SELECT PackageName, Version FROM `Apps`;";
        
        conn.Open();
        using var cmd = new SqliteCommand(sql, conn);
        using SqliteDataReader rdr = cmd.ExecuteReader();
        
        while (rdr.Read())
        {
            if(rdr.HasRows)
            {
                var name = rdr.GetString(0);
                var version = rdr.GetString(1);
                var item = new AppInfo() { AppName = name, Version = version };
                listItems.Add(item);

            }
        }
        return listItems;
    }

    
    public static bool checkApp(SqliteConnection conn, string appName)
    {
        conn.Open();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Apps WHERE PackageName=@id";
        cmd.Parameters.AddWithValue("@id",appName);
        SqliteDataReader reader = cmd.ExecuteReader();  
        if(reader.HasRows)return true;
        else return false;
    }

    public static AppInfo? ReadAppInfo(SqliteConnection conn, string appName)
    {
        conn.Open();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Apps WHERE PackageName=@app";
        cmd.Parameters.AddWithValue("@app",appName);
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                reader.Read();
                var name = reader.GetString(0);
                var desktop = reader.GetString(1);
                var appimage = reader.GetString(2);
                var version = reader.GetString(3);

                return new AppInfo() {AppName = name, DesktopFile = desktop, AppImageFile = appimage, Version = version};
            }
            else
            {
                return null;
            }
        }
    }

}