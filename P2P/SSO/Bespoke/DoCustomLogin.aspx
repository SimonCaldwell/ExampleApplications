<%--This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'--%>

<%--
    This file shows the processing code for a sample login page for PROACTIS P2P.  This is called by the CustomLogin.aspx page will
    passes the entered username, password and database title are form post parameters

    Once this page has successfully validated a user,  it needs to write a token to the dsdba.CustomLoginTokens table
    and then redirect the user to CustomLoginAsync page in the core product.

    If the user was not successfully validated,  then the CustomLogin.aspx page is redisplayed.

--%>

<%@ Page Language="C#" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Microsoft.Win32" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.Xsl" %>
<%@ Import Namespace="System.Xml.XPath" %>


<!DOCTYPE html>

<script runat="server">

    public string strToken = "";

    #region Helper Methods

    private void GetDatabase(string title, ref string databaseName, ref string databaseServerName)
    {
        //Configure the grs namespace
        var nt = new NameTable();
        var nsmgr = new XmlNamespaceManager(nt);
        nsmgr.AddNamespace("grs", "http://www.getrealsystems.com/xml/xml-ns");

        //Load the file
        var locationOfFile = GetDatabasesFilePath();
        var databaseDOM = new XmlDocument(nt);
        databaseDOM.Load(locationOfFile);

        //Find our entry
        XmlElement entry = databaseDOM.DocumentElement.SelectSingleNode("grs:Entry[@grs:Title='" + title + "']", nsmgr) as XmlElement;
        if (entry == null)
            throw new Exception(string.Format("Can not find the entry Title={0} in {1}", title, locationOfFile));
        else
        {
            databaseName = entry.GetAttribute("grs:Database");
            databaseServerName = entry.GetAttribute("grs:Server");
        }
    }

    private string GetDatabasesFilePath()
    {
        return Path.Combine(GetConfigurationPath(), @"PROACTIS_Databases.xml");
    }

    private string GetConfigurationPath()
    {
        RegistryServices objRegistry = new RegistryServices();
        string path = string.Empty;
#if (DEBUG)
            path = objRegistry.ReadString(objRegistry.GRSRootKey + @"Proactis Server\ConfigurationFolderLocation", "C:");
#else
        path = objRegistry.ReadString(objRegistry.GRSRootKey + @"Installs\HomeFolder", "");
        if (path == "")
        {
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.GetFullPath(Path.Combine(path, @"ConfigurationFolder"));
        }
        else
            path = Path.GetFullPath(Path.Combine(path, @"ConfigurationFolder"));
#endif
        return path;
    }

    private sealed class RegistryServices
    {
        private const string REGISTRY_ROOT = @"HKEY_LOCAL_MACHINE\Software\Get Real Systems Ltd\";

        public string GRSRootKey
        {
            get
            {
                return REGISTRY_ROOT;
            }
        }

        public string ReadString(string wholeKey, string defaultValueIfMissing = "")
        {
            //Guard.CheckForNullOrBlankArgument(wholeKey, nameof(wholeKey));

            string value = string.Empty;
            bool valueExists = true;        //Assume the best!

            // Decode the Registry Path we have just been given into a structure.
            var parts = ParseKey(wholeKey);

            // Open the registry key
            var r = parts.RegistryKeyRoot.OpenSubKey(parts.SubKeyPath);

            // If the subkey doesn'r exist,  then R will be null
            if (r == null)
            {
                // The subkey doesn't exist -  return the default value instead
                value = defaultValueIfMissing;

                // Record that the value is missing
                valueExists = false;
            }
            else
            {
                // Read back our value.  string.empty will be returned if the value doesn't exist.
                var valueAsObject = r.GetValue(parts.ValueName);

                // Tidy up after ourselves
                r.Close();

                // Does the value currently exist?  If not, then GetValue returns NULL.  The value
                // could exist but be empty.
                if (valueAsObject == null)
                {
                    // The value isn't currently set in the registry,  return the default value instead
                    value = defaultValueIfMissing;

                    // Record that the value is missing
                    valueExists = false;
                }
                else
                {
                    value = (string)valueAsObject;
                    valueExists = true;
                }
            }

            // Tidy up
            parts.RegistryKeyRoot.Close();

            // Pass back the final value.
            return value;

        }

        public RegistryKey ReadKey(string partialKey)
        {
            //Guard.CheckForNullOrBlankArgument(partialKey, nameof(partialKey));

            // Decode the Registry Path we have just been given into a structure.
            var parts = ParseKey(partialKey, true);

            // Open the registry key
            return parts.RegistryKeyRoot.OpenSubKey(parts.SubKeyPath);
        }


        #region Private Methods

        private string ExpandKey(string key)
        {
            if (key.ToUpperInvariant().StartsWith("HKEY_"))
                return key;
            else
                return this.GRSRootKey + key;
        }

        private bool IsOurKey(string key)
        {
            return key.ToLower().StartsWith(this.GRSRootKey.ToLower());
        }

        private KeyParts ParseKey(string wholeKey, bool partialKeyPassed = false)
        {
            //Default the root part of the path if it's missing
            var expandedKey = ExpandKey(wholeKey);

            //If we are running as a 64bit app,  then we need to read our own settings from the 32bit hive.
            var wowHive = IsOurKey(expandedKey) ? RegistryView.Registry32 : RegistryView.Default;
            var split = expandedKey.Split(new char[] { '\\' });

            if (split.Length < 2)
                throw new ArgumentOutOfRangeException("wholeKey", "Invalid registry key");

            var root = default(RegistryKey);
            switch (split[0].ToUpperInvariant())
            {
                case "HKEY_LOCAL_MACHINE":
                    root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, wowHive);
                    break;
                case "HKEY_CLASSES_ROOT":
                    root = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, wowHive);
                    break;
                case "HKEY_CURRENT_USER":
                    root = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, wowHive);
                    break;
                case "HKEY_USERS":
                    root = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, wowHive);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("wholeKey", "Invalid registry hive: " + split[0].ToUpperInvariant());
            }

            // Populate our structure with the various parts of the key
            var result = new KeyParts();
            result.RegistryKeyRoot = root;
            result.ValueName = split[split.Length - 1];

            int endMarker = (partialKeyPassed ? split.Length : split.Length - 1);

            for (var i = 1; i < endMarker; i++)
            {
                if (string.IsNullOrEmpty(result.SubKeyPath))
                {
                    result.SubKeyPath = split[i];
                }
                else
                {
                    result.SubKeyPath += @"\" + split[i];
                }
            }
            return result;
        }

        private struct KeyParts
        {
            public RegistryKey RegistryKeyRoot;
            public string SubKeyPath;
            public string ValueName;
        }

        #endregion
    }

    private string CreateDatabaseConnectionString(string strDatabaseTitle)
    {
        string databaseName = "";
        string databaseServerName = "";
        string password = null;

        GetDatabase(strDatabaseTitle, ref databaseName, ref databaseServerName);

        RegistryServices registry = new RegistryServices();
        RegistryKey regKey = registry.ReadKey(@"Proactis Server\Database Passwords");
        if (regKey == null)
        {
            throw new Exception("No entry in the registry for database passwords.");
        }

        if (regKey.GetValue(databaseServerName) != null)
        {
            //Found a password specific to the databse server.
            password = (string)regKey.GetValue(databaseServerName);
        }
        if (string.IsNullOrEmpty(password) && regKey.GetValue("NonServerSpecific") != null)
        {
            password = (string)regKey.GetValue("NonServerSpecific");
        }

        string connectionString = "Data Source=" + databaseServerName + ";Initial Catalog=" + databaseName + "; User Id=InternalUser; Password=" + password;
        return connectionString;
    }

    #endregion

    protected void Page_Load(Object Sender, EventArgs e)
    {

        var strUsername = Request.Form["username"];
        var strPassword = Request.Form["password"];
        var strDatabaseTitle = Request.Form["databaseTitle"];

        // Put your user validation code in here
        if (strUsername.ToLower() != "david" || strPassword != "secret")
        {
            // Login has failed
            Session.Add("Exception", "Login Failed");
            Response.Redirect("CustomLogin.aspx");
            Response.End();
        }

        // User is valid - log them in.
        try
        {
            // Get a connection string to the PROACTIS database so that we can write a custom token
            var connectionString = CreateDatabaseConnectionString(strDatabaseTitle);

            // Generate a one-time use token
            strToken = strDatabaseTitle + "@" + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

            // Write the token to the database
            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "insert into dsdba.CustomLoginTokens (Token, LoginID, Expires) SELECT @Token, @Username, DateAdd(s, 30, GETDATE())";
                    cmd.Parameters.AddWithValue("@Token", strToken);
                    cmd.Parameters.AddWithValue("@Username", strUsername);
                    cmd.Connection = cn;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        catch (Exception ex)
        {
            Session.Add("Exception", ex.ToString());
            Response.Redirect("CustomLogin.aspx");
            Response.End();
        }
    }
</script>


<html>
<head>
    <title>Do Custom Login</title>
</head>
<body style="padding: 40px;">
    <form method="post" id="frmForm" action="../../SystemLogon/CustomLoginAsync?token=<%=strToken%>">
    </form>

    <script type="text/javascript">
        document.getElementById("frmForm").submit();
    </script>

</body>
</html>






