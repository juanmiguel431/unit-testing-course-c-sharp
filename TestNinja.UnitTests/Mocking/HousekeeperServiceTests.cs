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

        [SetUp]
        public void SetUp()
        {
            _emailManager = new Mock<IEmailManager>();
            _fileManager = new Mock<IFileManager>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _housekeeperStatementReportStorage = new Mock<IHousekeeperStatementReportStorage>();
            _xtraMessageBox = new Mock<IXtraMessageBox>();

            _service = new HousekeeperService(
                _unitOfWork.Object,
                _emailManager.Object,
                _fileManager.Object,
                _housekeeperStatementReportStorage.Object,
                _xtraMessageBox.Object);

            _statementDate = new DateTime();
            _houseKeeper = new Housekeeper { Oid = 1, FullName = "fullname", Email = "email", StatementEmailBody = "body" };
        }

        [Test]
        public void SendStatementEmails_WhenThereAreHouseKeeper_SaveStatementAndEmailFileAreExecuted()
        {
            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    _houseKeeper
                }.AsQueryable());

            _housekeeperStatementReportStorage.Setup(s =>
                    s.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
                .Returns($"Sandpiper Statement {_statementDate:yyyy-MM} {_houseKeeper.FullName}.pdf");

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
        public void SendStatementEmails_WhenHouseKeeperEmailIsNull_DoNotExecuteStorageMethods()
        {
            _houseKeeper.Email = "";

            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    _houseKeeper
                }.AsQueryable());

            _service.SendStatementEmails(_statementDate);
            
            _housekeeperStatementReportStorage.VerifyNoOtherCalls();
            _emailManager.VerifyNoOtherCalls();
            _fileManager.VerifyNoOtherCalls();
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        public void SendStatementEmails_WhenSaveStatementDoNotReturnFileName_EmailSenderAndFileDeletionIsNotExecuted()
        {
            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    _houseKeeper
                }.AsQueryable());

            _service.SendStatementEmails(_statementDate);
            
            _emailManager.VerifyNoOtherCalls();
            _fileManager.VerifyNoOtherCalls();
            _xtraMessageBox.VerifyNoOtherCalls();
        }
        
        [Test]
        public void SendStatementEmails_WhenEmailManagerThrowsAnException_FileIsNotDeletedAndShowsMessageBox()
        {
            _unitOfWork.Setup(u => u.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    _houseKeeper
                }.AsQueryable());

            _housekeeperStatementReportStorage.Setup(s =>
                    s.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate))
                .Returns($"Sandpiper Statement {_statementDate:yyyy-MM} {_houseKeeper.FullName}.pdf");

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