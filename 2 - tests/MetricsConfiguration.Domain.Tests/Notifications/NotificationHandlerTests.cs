using MetricsConfiguration.Domain.Notifications;
using NSubstitute;

namespace MetricsConfiguration.Domain.Tests.Notifications
{
    public class NotificationHandlerTests
    {
        private readonly NotificationHandler _sut;
        public NotificationHandlerTests()
        {
            _sut = Substitute.For<NotificationHandler>();
        }

        [Fact(DisplayName = "HasNotification")]
        [Trait("HasNotification", "Should Return True When Exist Notification")]
        public void Should_Return_True_When_Exist_Notification()
        {
            // Arrange
            _sut.AddNotification("Message", "Error");

            // Act
            var result = _sut.HasNotification();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "HasNotification")]
        [Trait("HasNotification", "Should Return False When Not Exist Notification")]
        public void Should_Return_False_When_Not_Exist_Notification()
        {
            // Arrange

            // Act
            var result = _sut.HasNotification();

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "GetMessage")]
        [Trait("GetMessage", "Should Return Message When Exist Notification")]
        public void Should_Return_Message_When_Exist_Notification()
        {
            // Arrange
            _sut.AddNotification("Message", "Error");

            // Act
            var result = _sut.GetMessage();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact(DisplayName = "GetMessage")]
        [Trait("GetMessage", "Should Return Messagem Null When Not Exist Notification")]
        public void Should_Return_Message_Null_When_Not_Exist_Notification()
        {
            // Arrange

            // Act
            var result = _sut.GetMessage();

            // Assert
            Assert.Null(result);
        }


        [Fact(DisplayName = "GetStatusCode")]
        [Trait("GetStatusCode", "Should Return Status Code When Exist Notification")]
        public void Should_Return_Status_Code_When_Exist_Notification()
        {
            // Arrange
            _sut.AddNotification("Message", "Error");

            // Act
            var result = _sut.GetStatusCode();

            // Assert
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "GetStatusCode")]
        [Trait("GetStatusCode", "Should Return Status Code Null When Not Exist Notification")]
        public void Should_Return_StatusCode_Null_When_Not_Exist_Notification()
        {
            // Arrange

            // Act
            var result = _sut.GetStatusCode();

            // Assert
            Assert.Null(result);
        }


        [Fact(DisplayName = "GetNotifications")]
        [Trait("GetNotifications", "Should Return List Notifications When Exist Notification")]
        public void Should_Return_List_Notifications_When_Exist_Notification()
        {
            // Arrange
            _sut.AddNotification("Message", "Error 1");
            _sut.AddNotification("Message", "Error 2");

            // Act
            var result = _sut.GetNotifications();

            // Assert
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "GetNotifications")]
        [Trait("GetNotifications", "Should Return List Empty When Not Exist Notification")]
        public void Should_Return_List_Empty_When_Not_Exist_Notification()
        {
            // Arrange

            // Act
            var result = _sut.GetNotifications();

            // Assert
            Assert.Empty(result);
        }


        [Fact(DisplayName = "AddNotification")]
        [Trait("AddNotification", "Should Add New Notification With Message And Error")]
        public void Should_Add_New_Notification_With_Message_And_Error()
        {
            // Arrange
            var message = "Message";
            var error = "Error";

            // Act
            _sut.AddNotification(message, error);

            // Assert
            Assert.NotNull(_sut._notifications);
        }


        [Fact(DisplayName = "AddNotification")]
        [Trait("AddNotification", "Should Add New Notification With Status Code, Message And Error")]
        public void Should_Add_New_Notification_With_Status_Code_Message_And_Error()
        {
            // Arrange
            var statusCode = 400;
            var message = "Message";
            var error = "Error";

            // Act
            _sut.AddNotification(statusCode, message, error);

            // Assert
            Assert.NotNull(_sut._notifications);
        }


        [Fact(DisplayName = "AddNotification")]
        [Trait("AddNotification", "Should Add New Notification With Notification")]
        public void Should_Add_New_Notification_With_Notification()
        {
            // Arrange
            var notification = new Notification("Message", "Error", 400);

            // Act
            _sut.AddNotification(notification);

            // Assert
            Assert.NotNull(_sut._notifications);
        }

        [Fact(DisplayName = "AddNotifications")]
        [Trait("AddNotifications", "Should Add New Notification With Notifications")]
        public void Should_Add_New_Notification_With_Notifications()
        {
            // Arrange
            var notifications = new List<Notification>() {
                new Notification("Message", "Error", 400),
                new Notification("Message 1", "Error 1", 400),
                new Notification("Message 2", "Error 2", 500),
            };

            // Act
            _sut.AddNotifications(notifications);

            // Assert
            Assert.NotNull(_sut._notifications);
        }


        [Fact(DisplayName = "AddNotification")]
        [Trait("AddNotificationa", "Should Add New Notification With Status Code And Message")]
        public void Should_Add_New_Notification_With_Status_Code_And_Message()
        {
            // Arrange
            var statusCode = 400;
            var message = "Message";

            // Act
            _sut.AddNotification(statusCode, message);


            // Assert
            Assert.NotNull(_sut._notifications);
        }


        [Fact(DisplayName = "AddError")]
        [Trait("AddError", "Should Add Error")]
        public void Should_Add_Error()
        {
            // Arrange
            var error = "Message";

            // Act
            _sut.AddError(error);


            // Assert
            Assert.NotNull(_sut._notifications);
        }

    }
}
