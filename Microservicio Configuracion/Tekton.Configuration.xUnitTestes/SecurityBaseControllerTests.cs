namespace Tekton.Security.xUnitTestes
{
    using Tekton.Seguridad;
    using Xunit;

    public class SecurityBaseControllerTests
    {
        private class TestSecurityBaseController : SecurityBaseController
        {
            public int? PublicUserId => base.UserId;

            public string? PublicUserRoleId => base.UserRoleId;

            public string? PublicUserRole => base.UserRole;

            public string? PublicUserEmail => base.UserEmail;

            public string? PublicUserNickName => base.UserNickName;

            public string? PublicUserName => base.UserName;
        }

        private TestSecurityBaseController _testClass;

        public SecurityBaseControllerTests()
        {
            _testClass = new TestSecurityBaseController();
        }

        [Fact]
        public void CanGetPublicUserId()
        {
            // Assert
            Assert.Null(_testClass.PublicUserId);
        }

        [Fact]
        public void CanGetPublicUserRoleId()
        {
            // Assert
            Assert.Null(_testClass.PublicUserRoleId);
        }

        [Fact]
        public void CanGetPublicUserRole()
        {
            // Assert
            Assert.Null(_testClass.PublicUserRole);
        }

        [Fact]
        public void CanGetPublicUserEmail()
        {
            // Assert
            Assert.Null(_testClass.PublicUserEmail);
        }

        [Fact]
        public void CanGetPublicUserNickName()
        {
            // Assert
            Assert.Null(_testClass.PublicUserNickName);
        }

        [Fact]
        public void CanGetPublicUserName()
        {
            // Assert
            Assert.Null(_testClass.PublicUserName);
        }
    }
}