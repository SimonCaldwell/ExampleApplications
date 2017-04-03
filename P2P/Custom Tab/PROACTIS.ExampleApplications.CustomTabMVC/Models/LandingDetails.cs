namespace PROACTIS.ExampleApplications.CustomTabMVC.Models
{
    /// <summary>
    /// Information provided by the initial call from P2P
    /// </summary>
    public class LandingDetails
    {
        /// <summary>
        /// One time token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Name of the application (tab)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL of the application
        /// </summary>
        public string URL { get; set; }
        public string DatabaseTitle { get; internal set; }
        public string SessionID { get; internal set; }
        public SessionDetails SessionDetails { get; internal set; }
    }
}