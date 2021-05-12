using System;
using Hitcom_AccountingEquipment;
using Hitcom_AccountingEquipment.PageFolder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestHitcom
{
    [TestClass]
    public class HitcomTestProject
    {
        [TestMethod]
        public void MinLenghtLoginTest()
        {
            bool predict = false;
            string Login = "";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void MaxLenghtLoginTest()
        {
            bool predict = false;
            string Login = "aW2faBasdasdhAScXzTasdaQWasTRVh@td";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void LoginWithUpperText()
        {
            bool predict = true;
            string Login = "Aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void LoginWithoutUpperText()
        {
            bool predict = false;
            string Login = "aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void LoginWithLowerText()
        {
            bool predict = true;
            string Login = "Aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void LoginWithoutLowerText()
        {
            bool predict = false;
            string Login = "AW241GC";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void LoginWithNumberText()
        {
            bool predict = true;
            string Login = "Aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void LoginWithoutNumerText()
        {
            bool predict = false;
            string Login = "AWsbeGC";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.LoginEnteringTextCheck(Login);
            Assert.AreEqual(predict, result);
        }

        [TestMethod]
        public void MinLenghtPasswordTest()
        {
            bool predict = false;
            string Password = "";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);

        }
        [TestMethod]
        public void MaxLenghtPasswordTest()
        {
            bool predict = false;
            string Password = "aW2faBasdasdhAScXzTasdaQWasTRVh@td";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void PasswordWithUpperText()
        {
            bool predict = true;
            string Password = "Aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void PasswordWithoutUpperText()
        {
            bool predict = false;
            string Password = "aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void PasswordWithLowerText()
        {
            bool predict = true;
            string Password = "Aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void PasswordWithoutLowerText()
        {
            bool predict = false;
            string Password = "AW241GC";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void PasswordWithNumberText()
        {
            bool predict = true;
            string Password = "Aw241gc";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void PasswordWithoutNumerText()
        {
            bool predict = false;
            string Password = "AWsbeGC";
            AuthorizationData AuData = new AuthorizationData();
            bool result = AuData.PasswordEnteringTextCheck(Password);
            Assert.AreEqual(predict, result);
        }
        [TestMethod]
        public void PasswordGenerateNew()
        {
            SenderMail SenderMail = new SenderMail();
            string Predict = SenderMail.SenderCode();
            string result = SenderMail.SenderCode();
            Assert.AreEqual(Predict, result);
        }
            [TestMethod]
            public void EmailSenderTestTrue()
            {
                SenderMail SenderMail = new SenderMail();
                bool Predict = true;
                bool result = SenderMail.senderMAil("any@mail.ru", "asS21");
                Assert.AreEqual(Predict, result);
            }
            [TestMethod]
            public void EmailSenderTestFalse()
            {
                SenderMail SenderMail = new SenderMail();
                bool Predict = false;
                bool result = SenderMail.senderMAil("", "asS21");
                Assert.AreEqual(Predict, result);
            }
            [TestMethod]
            public void CheckGrantedAccessTodelInRoomPage()
            {
                SenderMail SenderMail = new SenderMail();
                RoomPage Page = new RoomPage();
                SenderMail.PositionName = "Администратор";
                bool Predict = true;
                bool result = Page.AccessToDelBtn(2);
                Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInRoomPage()
        {
            SenderMail SenderMail = new SenderMail();
            RoomPage Page = new RoomPage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(-2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessWithHalfDataTodelInRoomPage()
        {
            SenderMail SenderMail = new SenderMail();
            RoomPage Page = new RoomPage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInRoomPageHalfData()
        {
            SenderMail SenderMail = new SenderMail();
            RoomPage Page = new RoomPage();
            SenderMail.PositionName = "Администратор";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(0);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckGrantedAccessTodelInRepairPage()
        {
            SenderMail SenderMail = new SenderMail();
            RepairPage Page = new RepairPage();
            SenderMail.PositionName = "Администратор";
            bool Predict = true;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInRepairPage()
        {
            SenderMail SenderMail = new SenderMail();
            RepairPage Page = new RepairPage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(-2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessWithHalfDataTodelInRepairPage()
        {
            SenderMail SenderMail = new SenderMail();
            RepairPage Page = new RepairPage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInRepairPageHalfData()
        {
            SenderMail SenderMail = new SenderMail();
            RepairPage Page = new RepairPage();
            SenderMail.PositionName = "Администратор";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(0);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckGrantedAccessTodelInNomenclaturePage()
        {
            SenderMail SenderMail = new SenderMail();
            NomenclaturePage Page = new NomenclaturePage();
            SenderMail.PositionName = "Администратор";
            bool Predict = true;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInNomenclaturePage()
        {
            SenderMail SenderMail = new SenderMail();
            NomenclaturePage Page = new NomenclaturePage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(-2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessWithHalfDataTodelInNomenclaturePage()
        {
            SenderMail SenderMail = new SenderMail();
            NomenclaturePage Page = new NomenclaturePage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInNomenclaturePageHalfData()
        {
            SenderMail SenderMail = new SenderMail();
            NomenclaturePage Page = new NomenclaturePage();
            SenderMail.PositionName = "Администратор";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(0);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckGrantedAccessTodelInInventarizationPage()
        {
            SenderMail SenderMail = new SenderMail();
            InventarizationPage Page = new InventarizationPage();
            SenderMail.PositionName = "Администратор";
            bool Predict = true;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInInventarizationPage()
        {
            SenderMail SenderMail = new SenderMail();
            InventarizationPage Page = new InventarizationPage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(-2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessWithHalfDataTodelInInventarizationPage()
        {
            SenderMail SenderMail = new SenderMail();
            InventarizationPage Page = new InventarizationPage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInInventarizationPageHalfData()
        {
            SenderMail SenderMail = new SenderMail();
            InventarizationPage Page = new InventarizationPage();
            SenderMail.PositionName = "Администратор";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(0);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckGrantedAccessTodelInEquipmentPage()
        {
            SenderMail SenderMail = new SenderMail();
            EquipmentPage Page = new EquipmentPage();
            SenderMail.PositionName = "Администратор";
            bool Predict = true;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInEquipmentPage()
        {
            SenderMail SenderMail = new SenderMail();
            NomenclaturePage Page = new NomenclaturePage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(-2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessWithHalfDataTodelInEquipmentPage()
        {
            SenderMail SenderMail = new SenderMail();
            EquipmentPage Page = new EquipmentPage();
            SenderMail.PositionName = "Менеджер";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(2);
            Assert.AreEqual(Predict, result);
        }
        [TestMethod]
        public void CheckDeniedAccessTodelInEquipmentPageHalfData()
        {
            SenderMail SenderMail = new SenderMail();
            EquipmentPage Page = new EquipmentPage();
            SenderMail.PositionName = "Администратор";
            bool Predict = false;
            bool result = Page.AccessToDelBtn(0);
            Assert.AreEqual(Predict, result);
        }

    }
}
