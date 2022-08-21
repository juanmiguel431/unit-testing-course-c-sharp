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
        private HousekeeperService _service;
        private DateTime _statementDate;
        private Housekeeper _houseKeeper;
        private readonly string _filename = "Sandpiper Statement.pdf";

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _emailManager = new Mock<IEmailManager>();
            _fileManager = new Mock<IFileManager>();
            _housekeeperStatementReportStorage = new Mock<IHousekeeperStatementReportStorage>();
            _xtraMessageBox = new Mock<IXtraMessageBox>();

            _houseKeeper = new Housekeeper { Oid = 1, FullName = "fullname", Email = "email", StatementEmailBody = "body" };
            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper> { _houseKeeper }.AsQueryable());

            _service = new HousekeeperService(
                _unitOfWork.Object,
                _emailManager.Object,
                _fileManager.Object,
                _housekeeperStatementReportStorage.Object,
                _xtraMessageBox.Object);

            _statementDate = new DateTime();
        }

        [Test]
        public void SendStatementEmails_WhenThereAreHouseKeeper_SaveStatementAndEmailFileAreExecuted()
        {
            _housekeeperStatementReportStorage.Setup(s =>
                    s.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
                .Returns(_filename);

            _service.SendStatementEmails(_statementDate);

            _housekeeperStatementReportStorage.Verify(a =>
                a.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate));

            _emailManager.Verify(a =>
                a.EmailFile(
                    _houseKeeper.Email,
                    _houseKeeper.StatementEmailBody,
                    It.Is<string>(p => p.Contains(".pdf")),
                    It.Is<string>(p => p.Contains("Sandpiper Statement"))));
            
            _fileManager.Verify(f => f.Delete(It.IsAny<string>()));
            
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void SendStatementEmails_WhenHouseKeeperEmailIsNull_DoNotExecuteStorageMethods(string email)
        {
            _houseKeeper.Email = email;

            _service.SendStatementEmails(_statementDate);
            
            _housekeeperStatementReportStorage.Verify(a =>
                a.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate), times: Times.Never);

            _emailManager.VerifyNoOtherCalls();
            _fileManager.VerifyNoOtherCalls();
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_WhenSaveStatementDoNotReturnFileName_EmailSenderAndFileDeletionIsNotExecuted(string filename)
        {
            _housekeeperStatementReportStorage.Setup(s =>
                    s.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
                .Returns(filename);

            _service.SendStatementEmails(_statementDate);
            
            _emailManager.VerifyNoOtherCalls();
            _fileManager.VerifyNoOtherCalls();
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        public void SendStatementEmails_WhenEmailManagerThrowsAnException_FileIsNotDeletedAndShowsMessageBox()
        {
            _housekeeperStatementReportStorage.Setup(s =>
                    s.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
                .Returns(_filename);

            _emailManager.Setup(m => 
                m.EmailFile(
                    _houseKeeper.Email,
                    _houseKeeper.StatementEmailBody,
                    It.Is<string>(p => p.Contains(".pdf")),
                    It.IsAny<string>())).Throws<Exception>();
            
            _service.SendStatementEmails(_statementDate);
            
            _xtraMessageBox.Verify(m => 
                m.Show(
                    It.IsAny<string>(),
                    It.Is<string>(p => p.Contains(_houseKeeper.Email)),
                    MessageBoxButtons.OK));
            
            _fileManager.VerifyNoOtherCalls();
        }
    }
}