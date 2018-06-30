using Microsoft.EntityFrameworkCore;
using System;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;
using Xunit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace XUnitTestTodoApi
{
    public class TodoItemTest
    {
        // 1. Create a ToDO item
        [Fact]
        public void CanCreateTodoItem()
        {

            DbContextOptions<TodoDbContext> options = new DbContextOptionsBuilder<TodoDbContext>().UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoItem datItem = new TodoItem();
                datItem.ID = 5;
                datItem.Name = "creating a test method";
                datItem.IsComplete = false;

                TodoController ic = new TodoController(context);

                // Act
                var result = ic.Create(datItem);

                var answer = result.Result;
                var x = (ObjectResult)answer;

                // Assert
                Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)x.StatusCode);
            }
        }

        // 2. Read a TODO Item
        [Fact]
        public void CanReadTodoItem()
        {
            DbContextOptions<TodoDbContext> options = new DbContextOptionsBuilder<TodoDbContext>().UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoItem datItem1 = new TodoItem();
                datItem1.ID = 1;
                datItem1.Name = "walk the dog";
                datItem1.IsComplete = false;

                TodoItem datItem2 = new TodoItem();
                datItem2.ID = 2;
                datItem2.Name = "wash the dishes";
                datItem2.IsComplete = true;

                TodoController ic = new TodoController(context);

                // Act
                var created1 = ic.Create(datItem1);
                var created2 = ic.Create(datItem2);

                var result1 = ic.GetById(datItem1.ID);
                var result2 = ic.GetById(datItem2.ID);

                // Assert
                Assert.Equal(datItem1.ID, result1.Result.Value.ID);
                Assert.Equal(datItem2.ID, result2.Result.Value.ID);
            }
        }

        // 3. Update a ToDo item
        [Fact]
        public void CanUpdateTodoItem()
        {
            DbContextOptions<TodoDbContext> options = new DbContextOptionsBuilder<TodoDbContext>().UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoItem datItem1 = new TodoItem();
                datItem1.ID = 6;
                datItem1.Name = "walk the dog";
                datItem1.IsComplete = false;

                TodoItem datItem2 = new TodoItem();
                datItem2.ID = 6;
                datItem2.Name = "pet the dog";
                datItem2.IsComplete = true;

                TodoController ic = new TodoController(context);

                // Act
                var created1 = ic.Create(datItem1);

                var result1 = ic.Update(datItem1.ID, datItem2);
                var result2 = ic.GetById(datItem1.ID);

                // Assert
                Assert.Equal(datItem2.Name, result2.Result.Value.Name);
                Assert.Equal(datItem2.IsComplete, result2.Result.Value.IsComplete);
                Assert.Equal(datItem2.DatListID, result2.Result.Value.DatListID);
            }
        }

        // 3. Update a ToDo item
        [Fact]
        public void CanDeleteTodoItem()
        {
            DbContextOptions<TodoDbContext> options = new DbContextOptionsBuilder<TodoDbContext>().UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoItem datItem1 = new TodoItem();
                datItem1.ID = 8;
                datItem1.Name = "walk the dog";
                datItem1.IsComplete = false;

                TodoItem datItem2 = new TodoItem();
                datItem2.ID = 9;
                datItem2.Name = "wash the dishes";
                datItem2.IsComplete = true;

                TodoController ic = new TodoController(context);

                // Act
                var created1 = ic.Create(datItem1);
                var created2 = ic.Create(datItem2);

                var deletedItem = ic.Delete(datItem1.ID);

                var result1 = ic.GetById(datItem1.ID);
                var result2 = ic.GetById(datItem2.ID);

                // Assert
                Assert.Null(result1.Result.Value);
                Assert.NotNull(result2);
            }
        }
    }
}
