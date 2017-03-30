<%--This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'--%>

<%--
    This file shows the code for a sample login page for PROACTIS P2P.
    In this sample,  the user is asked to enter their username, password and to select a database as normal.
    When they click Logon,  a method is called on the DoCustomLogin.aspx page.    

    This example also handles the displaying of any system information messages.
--%>

<%@ Page Language="C#" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Microsoft.Win32" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.Xsl" %>
<%@ Import Namespace="System.Xml.XPath" %>


<!DOCTYPE html>




<script runat="server">
    public string strException = "";
    public List<string> databaseTitles = null;


    public List<string> infoMessages = new List<string>();
    public List<string> alertMessages = new List<string>();
    public List<string> errorMessages = new List<string>();

    #region Helper Methods

    private List<string> GetDatabaseTitles() {
        List<string> titles = new List<string>();

        //Configure the grs namespace
        var nt = new NameTable();
        var nsmgr = new XmlNamespaceManager(nt);
        nsmgr.AddNamespace("grs", "http://www.getrealsystems.com/xml/xml-ns");

        //Load the file
        var locationOfFile = GetDatabasesFilePath();
        var databaseDOM = new XmlDocument(nt);
        databaseDOM.Load(locationOfFile);

        XmlNodeList entries = databaseDOM.DocumentElement.SelectNodes("grs:Entry", nsmgr) as XmlNodeList;
        foreach (XmlElement entry in entries) {
            titles.Add(entry.GetAttribute("grs:Title"));
        }
        return titles;
    }

    private string GetDatabasesFilePath() {
        return Path.Combine(GetConfigurationPath(), @"PROACTIS_Databases.xml");
    }

    private string GetConfigurationPath() {
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

    private sealed class RegistryServices {
        private const string REGISTRY_ROOT = @"HKEY_LOCAL_MACHINE\Software\Get Real Systems Ltd\";

        public string GRSRootKey {
            get {
                return REGISTRY_ROOT;
            }
        }

        public string ReadString(string wholeKey, string defaultValueIfMissing = "") {
            //Guard.CheckForNullOrBlankArgument(wholeKey, nameof(wholeKey));

            string value = string.Empty;
            bool valueExists = true;        //Assume the best!

            // Decode the Registry Path we have just been given into a structure.
            var parts = ParseKey(wholeKey);

            // Open the registry key
            var r = parts.RegistryKeyRoot.OpenSubKey(parts.SubKeyPath);

            // If the subkey doesn'r exist,  then R will be null
            if (r == null) {
                // The subkey doesn't exist -  return the default value instead
                value = defaultValueIfMissing;

                // Record that the value is missing
                valueExists = false;
            }
            else {
                // Read back our value.  string.empty will be returned if the value doesn't exist.
                var valueAsObject = r.GetValue(parts.ValueName);

                // Tidy up after ourselves
                r.Close();

                // Does the value currently exist?  If not, then GetValue returns NULL.  The value
                // could exist but be empty.
                if (valueAsObject == null) {
                    // The value isn't currently set in the registry,  return the default value instead
                    value = defaultValueIfMissing;

                    // Record that the value is missing
                    valueExists = false;
                }
                else {
                    value = (string)valueAsObject;
                    valueExists = true;
                }
            }

            // Tidy up
            parts.RegistryKeyRoot.Close();

            // Pass back the final value.
            return value;

        }

        public RegistryKey ReadKey(string partialKey) {
            //Guard.CheckForNullOrBlankArgument(partialKey, nameof(partialKey));

            // Decode the Registry Path we have just been given into a structure.
            var parts = ParseKey(partialKey, true);

            // Open the registry key
            return parts.RegistryKeyRoot.OpenSubKey(parts.SubKeyPath);
        }


        #region Private Methods

        private string ExpandKey(string key) {
            if (key.ToUpperInvariant().StartsWith("HKEY_"))
                return key;
            else
                return this.GRSRootKey + key;
        }

        private bool IsOurKey(string key) {
            return key.ToLower().StartsWith(this.GRSRootKey.ToLower());
        }

        private KeyParts ParseKey(string wholeKey, bool partialKeyPassed = false) {
            //Default the root part of the path if it's missing
            var expandedKey = ExpandKey(wholeKey);

            //If we are running as a 64bit app,  then we need to read our own settings from the 32bit hive.
            var wowHive = IsOurKey(expandedKey) ? RegistryView.Registry32 : RegistryView.Default;
            var split = expandedKey.Split(new char[] { '\\' });

            if (split.Length < 2)
                throw new ArgumentOutOfRangeException("wholeKey", "Invalid registry key");

            var root = default(RegistryKey);
            switch (split[0].ToUpperInvariant()) {
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

            for (var i = 1; i < endMarker; i++) {
                if (string.IsNullOrEmpty(result.SubKeyPath)) {
                    result.SubKeyPath = split[i];
                }
                else {
                    result.SubKeyPath += @"\" + split[i];
                }
            }
            return result;
        }

        private struct KeyParts {
            public RegistryKey RegistryKeyRoot;
            public string SubKeyPath;
            public string ValueName;
        }

        #endregion
    }


    private List<string> ParseMessages(string encodedMessages) {
        if (!string.IsNullOrEmpty(encodedMessages)) {
            string decodedString = Encoding.Unicode.GetString(Convert.FromBase64String(encodedMessages));
            return decodedString.Split('|').ToList();
        }
        else {
            return new List<string>();
        }
    }


    #endregion



    protected void Page_Load(Object Sender, EventArgs e) {
        infoMessages = ParseMessages(HttpContext.Current.Request.QueryString["InfoMessages"]);
        alertMessages = ParseMessages(HttpContext.Current.Request.QueryString["AlertMessages"]);
        errorMessages = ParseMessages(HttpContext.Current.Request.QueryString["ErrorMessages"]);

        strException = Session["Exception"] as string;
        databaseTitles = GetDatabaseTitles();
    }
</script>


<html>
<head>
    <title>Custom Login</title>
</head>
<body style="padding:40px;">


    <% if (infoMessages.Count > 0) { %>
    <div class="messages infoMessages" style="background-color:#d9edf7; color:#31708f;">
    <% foreach (string message in infoMessages) { %>
        <p><%=message%></p>
    <% } %>
    </div>
    <% } %>
    <% if (alertMessages.Count > 0) { %>
    <div class="messages alertMessages" style="background-color:#fffda1; color:#8a6d3b;">
    <% foreach (string message in alertMessages) { %>
        <p><%=message%></p>
    <% } %>
    </div>
    <% } %>
    <% if (errorMessages.Count > 0) { %>
    <div class="messages errorMessages" style="background-color:#f2dede; color:#a9445f;">
    <% foreach (string message in errorMessages) { %>
        <p><%=message%></p>
    <% } %>
    </div>
    <% } %>


    <h2>Custom Login</h2>

    <form method="post" name="frmForm" action="DoCustomLogin.aspx">

    <div style="display:inline-block; border: solid 1px #999; border-radius:10px; padding:20px;">
        <p>User Name</p>
        <input id="txtUsername" name="username" type="text" value="" />
        <p>Password</p>
        <input id="txtPassword" name="password" type="text" value="" />
        <p>Database Title</p>
        <select name="databaseTitle">
<% foreach (string title in databaseTitles) { %>
            <option value="<%=title%>"><%=title%></option>
<% } %>
        </select>
        <div style="margin-top:20px;">
            <input type="submit" value="Login" style="float:right;" />
        </div>
    </div>
    </form>

    <% if (!String.IsNullOrEmpty(strException)) { %>
        <p>Exception</p>
        <textarea rows="30" cols="200"><%=strException%></textarea>
    <% } %>


</body>
</html>






