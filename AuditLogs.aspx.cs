using System;
using System.Collections.Generic;

namespace MandarinQuest
{
    public partial class AuditLogs : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLogs();
            }
        }

        void LoadLogs()
        {

            List<string> logs;

            if (Session["Logs"] == null)
            {
                logs = new List<string>();

                logs.Add("Admin logged into system");
                logs.Add("User Alice account updated");
                logs.Add("User John account disabled");
                logs.Add("Roles updated");

                Session["Logs"] = logs;
            }

            logs = (List<string>)Session["Logs"];

            string output = "";

            foreach (string log in logs)
            {
                output += "<div class='log'>" + log + "</div>";
            }

            litLogs.Text = output;
        }

    }
}