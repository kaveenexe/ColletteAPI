/*
 * File: MongoDbSettings.cs
 * Description: This class holds the settings for configuring the connection to the MongoDB database.
 */

namespace ColletteAPI.Data
{
    /*
     * Class: MongoDbSettings
     * This class is used to store the MongoDB configuration settings such as the connection string and database name.
     * These settings are typically populated from the application's configuration files (e.g., appsettings.json).
     */
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
