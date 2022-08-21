using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HousekeeperServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IHousekeeperStatementReportStorage> _housekeeperStatementReportStorage;
        private Mock<IEmailManager> _emailManager;
        private Mock<IFileManager> _fileManager;
        private Mock<IXtraMessageBox> _xtraMessageBox;

        [SetUp]
        public void SetUp()
        {
            _emailManager = new Mock<IEmailManager>();
            _fileManager = new Mock<IFileManager>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _housekeeperStatementReportStorage = new Mock<IHousekeeperStatementReportStorage>();
            _xtraMessageBox = new Mock<IXtraMessageBox>();

            HousekeeperService.EmailManager = _emailManager.Object;
            HousekeeperService.FileManager = _fileManager.Object;
            HousekeeperService.UnitOfWork = _unitOfWork.Object;
            HousekeeperService.HousekeeperStatementReportStorage = _housekeeperStatementReportStorage.Object;
            HousekeeperService._xtraMessageBox = _xtraMessageBox.Object;
        }

        [Test]
        public void SendStatementEmails_WhenThereAreHouseKeeper_SaveStatementAndEmailFileAreExecuted()
        {
            var houseKeeper = new Housekeeper { Oid = 1, FullName = "fullname", Email = "email", StatementEmailBody = "body" };
            var statementDate = new DateTime();

            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    houseKeeper
                }.AsQueryable());

            _housekeeperStatementReportStorage.Setup(s =>
                    s.SaveStatement(houseKeeper.Oid, houseKeeper.FullName, statementDate))
                .Returns($"Sandpiper Statement {statementDate:yyyy-MM} {houseKeeper.FullName}.pdf");

            HousekeeperService.SendStatementEmails(statementDate);

            _housekeeperStatementReportStorage.Verify(a =>
                a.SaveStatement(houseKeeper.Oid, houseKeeper.FullName, statementDate));

            _emailManager.Verify(a =>
                a.EmailFile(
                    houseKeeper.Email,
                    houseKeeper.StatementEmailBody,
                    It.Is<string>(p => p.Contains(".pdf")),
                    It.Is<string>(p => p.Contains("Sandpiper Statement"))));
            
            _fileManager.Verify(f => f.Delete(It.IsAny<string>()));
            
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        public void SendStatementEmails_WhenHouseKeeperEmailIsNull_DoNotExecuteStorageMethods()
        {
            var houseKeeper = new Housekeeper { Oid = 1, FullName = "fullname", Email = "", StatementEmailBody = "body" };
            var statementDate = new DateTime();

            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    houseKeeper
                }.AsQueryable());

            HousekeeperService.SendStatementEmails(statementDate);
            
            _housekeeperStatementReportStorage.VerifyNoOtherCalls();
            _emailManager.VerifyNoOtherCalls();
            _fileManager.VerifyNoOtherCalls();
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        public void SendStatementEmails_WhenSaveStatementDoNotReturnFileName_EmailSenderAndFileDeletionIsNotExecuted()
        {
            var houseKeeper = new Housekeeper { Oid = 1, FullName = "fullname", Email = "email", StatementEmailBody = "body" };
            var statementDate = new DateTime();

            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    houseKeeper
                }.AsQueryable());

            HousekeeperService.SendStatementEmails(statementDate);
            
            _emailManager.VerifyNoOtherCalls();
            _fileManager.VerifyNoOtherCalls();
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        public void SendStatementEmails_WhenEmailManagerThrowsAnException_FileIsNotDeletedAndShowsMessageBox()
        {
            var houseKeeper = new Housekeeper { Oid = 1, FullName = "fullname", Email = "email", StatementEmailBody = "body" };
            var statementDate = new DateTime();

            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    houseKeeper
                }.AsQueryable());

            _housekeeperStatementReportStorage.Setup(s =>
                    s.SaveStatement(houseKeeper.Oid, houseKeeper.FullName, statementDate))
                .Returns($"Sandpiper Statement {statementDate:yyyy-MM} {houseKeeper.FullName}.pdf");

            _emailManager.Setup(m => 
                m.EmailFile(
                    houseKeeper.Email,
                    houseKeeper.StatementEmailBody,
                    It.Is<string>(p => p.Contains(".pdf")),
                    It.IsAny<string>())).Throws<Exception>();
            
            HousekeeperService.SendStatementEmails(statementDate);
            
            _xtraMessageBox.Verify(m => 
                m.Show(
                    It.IsAny<string>(),
                    It.Is<string>(p => p.Contains(houseKeeper.Email)),
                    MessageBoxButtons.OK));
            
            _fileManager.VerifyNoOtherCalls();
        }
    }
}