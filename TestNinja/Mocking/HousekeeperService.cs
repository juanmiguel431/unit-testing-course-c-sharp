using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestNinja.Mocking
{
    public static class HousekeeperService
    {
        public static IUnitOfWork UnitOfWork;
        public static IEmailManager EmailManager;
        public static IFileManager FileManager;
        public static IHousekeeperStatementReportStorage HousekeeperStatementReportStorage;
        public static IXtraMessageBox _xtraMessageBox;

        public static void SendStatementEmails(DateTime statementDate)
        {
            var housekeepers = UnitOfWork.Query<Housekeeper>();

            foreach (var housekeeper in housekeepers)
            {
                if (string.IsNullOrWhiteSpace(housekeeper.Email))
                    continue;

                var statementFilename = HousekeeperStatementReportStorage.SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);

                if (string.IsNullOrWhiteSpace(statementFilename))
                    continue;

                var emailAddress = housekeeper.Email;
                var emailBody = housekeeper.StatementEmailBody;

                try
                {
                    EmailFile(emailAddress, emailBody, statementFilename, $"Sandpiper Statement {statementDate:yyyy-MM} {housekeeper.FullName}");
                }
                catch (Exception e)
                {
                    _xtraMessageBox.Show(e.Message, $"Email failure: {emailAddress}", MessageBoxButtons.OK);
                }
            }
        }

        private static void EmailFile(string emailAddress, string emailBody, string filename, string subject)
        {
            EmailManager.EmailFile(emailAddress, emailBody, filename, subject);
            FileManager.Delete(filename);
        }
    }

    public enum MessageBoxButtons
    {
        OK
    }

    public interface IXtraMessageBox
    {
        void Show(string s, string housekeeperStatements, MessageBoxButtons ok);
    }

    public class XtraMessageBox : IXtraMessageBox
    {
        public void Show(string s, string housekeeperStatements, MessageBoxButtons ok)
        {
        }
    }

    public class MainForm
    {
        public bool HousekeeperStatementsSending { get; set; }
    }

    public class DateForm
    {
        public DateForm(string statementDate, object endOfLastMonth)
        {
        }

        public DateTime Date { get; set; }

        public DialogResult ShowDialog()
        {
            return DialogResult.Abort;
        }
    }

    public enum DialogResult
    {
        Abort,
        OK
    }

    public class SystemSettingsHelper
    {
        public static string EmailSmtpHost { get; set; }
        public static int EmailPort { get; set; }
        public static string EmailUsername { get; set; }
        public static string EmailPassword { get; set; }
        public static string EmailFromEmail { get; set; }
        public static string EmailFromName { get; set; }
    }

    public class Housekeeper
    {
        public string Email { get; set; }
        public int Oid { get; set; }
        public string FullName { get; set; }
        public string StatementEmailBody { get; set; }
    }

    public interface IHousekeeperStatementReport
    {
        bool HasData { get; set; }
        void CreateDocument();
        void ExportToPdf(string filename);
    }

    public class HousekeeperStatementReport : IHousekeeperStatementReport
    {
        public HousekeeperStatementReport(int housekeeperOid, DateTime statementDate)
        {
        }

        public bool HasData { get; set; }

        public void CreateDocument()
        {
        }

        public void ExportToPdf(string filename)
        {
        }
    }
}