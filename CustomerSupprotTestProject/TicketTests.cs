using CustomerSupport.Controllers;
using CustomerSupport.Models;
using CustomerSupport.Models.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using CustomerSupport.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CustomerSupportTestProject
{
	[ExcludeFromCodeCoverage]
	public class TicketTests
	{
		[Fact]
		public void TestGetTickets_ReturnsExpectedViewModel()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<HomeController>>();
			var mockTicketDataAccess = new Mock<ITicketDataAccess>();
			var sampleTickets = new List<CustomerSupportItem>
			{
				new CustomerSupportItem { Id = 1, Name = "Ticket 1", Description = "Description 1", CreationTime=new DateTime(2024,04,10,08,00,00), DueDate=new DateTime(2024,05,20,08,00,00) },

				new CustomerSupportItem { Id = 2, Name = "Ticket 2", Description = "Description 2", CreationTime=new DateTime(2024,03,08,10,00,00), DueDate=new DateTime(2024,04,10,08,00,00) },

				new CustomerSupportItem { Id = 3, Name = "Ticket 3", Description = "Description 3", CreationTime=new DateTime(2024,04,06,12,00,00), DueDate=new DateTime(2024,05,10,10,00,00) },

				new CustomerSupportItem { Id = 4, Name = "Ticket 4", Description = "Description 4", CreationTime=new DateTime(2024,03,10,08,00,00), DueDate=new DateTime(2024,04,06,12,00,00) },
			};

			mockTicketDataAccess.Setup(repo => repo.GetAllTickets()).Returns(sampleTickets);

			var controller = new HomeController(mockLogger.Object, mockTicketDataAccess.Object);

			// Act
			var result = controller.GetTickets();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(sampleTickets.Count, result.TicketList.Count);
		}


		[Fact]
		public void TestInsertTicket()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<HomeController>>();
			var mockTicketDataAccess = new Mock<ITicketDataAccess>();
			var sampleTicket = new CustomerSupportItem
			{
				Id = 4,
				Name = "Ticket 4",
				Description = "Description 4",
				//CreationTime = new DateTime(2024, 03, 10, 08, 00, 00),
				DueDate = new DateTime(2024, 04, 06, 12, 00, 00)
			};

			mockTicketDataAccess.Setup(x => x.InsertTicket(It.IsAny<CustomerSupportItem>())).Returns(true);

			var controller = new HomeController(mockLogger.Object, mockTicketDataAccess.Object);

			// Act
			var result = controller.Insert(sampleTicket);

			// Assert
			mockTicketDataAccess.Verify(x => x.InsertTicket(It.Is<CustomerSupportItem>(t => t.Name == sampleTicket.Name && t.Description == sampleTicket.Description && t.DueDate == sampleTicket.DueDate)), Times.Once());

			var redirectResult = Assert.IsType<RedirectResult>(result);
			Assert.Equal("/Home", redirectResult.Url);
		}


		[Fact]
		public void TestDeleteTicket()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<HomeController>>();
			var mockTicketDataAccess = new Mock<ITicketDataAccess>();
			var sampleTicketId = 1;

			mockTicketDataAccess.Setup(x => x.DeleteTicketById(sampleTicketId)).Returns(true);

			var controller = new HomeController(mockLogger.Object, mockTicketDataAccess.Object);

			// Act
			var result = controller.Delete(sampleTicketId) as JsonResult;

			// Assert
			Assert.NotNull(result);
			Assert.NotNull(result.Value);

			var valueType = result.Value.GetType();
			var successProperty = valueType.GetProperty("success");
			Assert.NotNull(successProperty);

			var successValue = successProperty.GetValue(result.Value, null);
			Assert.IsType<bool>(successValue);
			Assert.True((bool)successValue);

			mockTicketDataAccess.Verify(x => x.DeleteTicketById(sampleTicketId), Times.Once);
		}


		[Fact]
		public void TestGetById_ReturnsTicket()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<HomeController>>();
			var mockTicketDataAccess = new Mock<ITicketDataAccess>();
			var sampleTicketId = 1;
			var expectedTicket = new CustomerSupportItem
			{
				Id = sampleTicketId,
				Name = "Sample Ticket",
				Description = "Sample Description",
				CreationTime = DateTime.Now,
				DueDate = DateTime.Now.AddDays(5)
			};

			mockTicketDataAccess.Setup(x => x.GetTicketById(sampleTicketId)).Returns(expectedTicket);
			var controller = new HomeController(mockLogger.Object, mockTicketDataAccess.Object);

			// Act
			var result = controller.GetById(sampleTicketId);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(expectedTicket.Id, result.Id);
			Assert.Equal(expectedTicket.Name, result.Name);

			mockTicketDataAccess.Verify(x => x.GetTicketById(sampleTicketId), Times.Once);
		}


		[Fact]
		public void TestPopulateForm()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<HomeController>>();
			var mockTicketDataAccess = new Mock<ITicketDataAccess>();
			var sampleTicketId = 1;
			var expectedTicket = new CustomerSupportItem {
				Id = sampleTicketId,
				Name = "Sample Ticket",
				Description = "Sample Description",
				CreationTime = DateTime.Now,
				DueDate = DateTime.Now.AddDays(5)
			};

			mockTicketDataAccess.Setup(x => x.GetTicketById(sampleTicketId)).Returns(expectedTicket);
			var controller = new HomeController(mockLogger.Object, mockTicketDataAccess.Object);

			// Act
			var result = controller.PopulateForm(sampleTicketId) as JsonResult;

			// Assert
			Assert.NotNull(result);
			Assert.Equal(expectedTicket, result.Value);

			mockTicketDataAccess.Verify(x => x.GetTicketById(sampleTicketId), Times.Once);
		}


		[Fact]
		public void TestUpdate_RedirectsToHomeOnSuccess()
		{
			// Arrange
			var mockLogger = new Mock<ILogger<HomeController>>();
			var mockTicketDataAccess = new Mock<ITicketDataAccess>();
			var controller = new HomeController(mockLogger.Object, mockTicketDataAccess.Object);
			var sampleTicket = new CustomerSupportItem {
				Id = 1,
				Name = "Sample Ticket",
				Description = "Sample Description",
				CreationTime = DateTime.Now,
				DueDate = DateTime.Now.AddDays(5)
			};

			mockTicketDataAccess.Setup(x => x.UpdateTicket(It.IsAny<CustomerSupportItem>())).Returns(true);

			// Act
			var result = controller.Update(sampleTicket) as RedirectResult;

			// Assert
			Assert.NotNull(result);
			Assert.Equal("/Home", result.Url);

			mockTicketDataAccess.Verify(x => x.UpdateTicket(It.Is<CustomerSupportItem>(t => t == sampleTicket)), Times.Once);
		}
	}
}