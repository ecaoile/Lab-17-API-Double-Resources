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
    public class UnitTest1
    {
        // 1. Create a ToDO item
        [Fact]
        public void CanCreateTodoItem()
        {

            DbContextOptions<TodoDbContext> options = new DbContextOptionsBuilder<TodoDbContext>().UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoItem datItem1 = new TodoItem();
                datItem1.Name = "walk the dog";
                datItem1.IsComplete = false;
                //datItem1.DatListID = 1;

                TodoItem datItem2 = new TodoItem();
                datItem2.Name = "wash the dishes";
                datItem2.IsComplete = true;
                //datItem1.DatListID = 1;

                TodoController lc = new TodoController(context);

                // Act
                var result1 = lc.Create(datItem1);
                var result2 = lc.Create(datItem2);

                var answer1 = result1.Result;
                var x = (ObjectResult)answer1;

                var answer2 = result2.Result;
                var y = (ObjectResult)answer2;

                // Assert
                Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)x.StatusCode);
                Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)y.StatusCode);
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

                TodoController lc = new TodoController(context);

                // Act
                var created1 = lc.Create(datItem1);
                var created2 = lc.Create(datItem2);

                var result1 = lc.GetById(datItem1.ID);
                var result2 = lc.GetById(datItem2.ID);

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
                datItem1.ID = 1;
                datItem1.Name = "walk the dog";
                datItem1.IsComplete = false;

                TodoItem datItem2 = new TodoItem();
                datItem1.ID = 2;
                datItem2.Name = "wash the dishes";
                datItem2.IsComplete = true;

                TodoController lc = new TodoController(context);

                // Act
                var created1 = lc.Create(datItem1);
                var created2 = lc.Create(datItem2);

                var result1 = lc.Update(datItem1.ID, datItem2);
                var result2 = lc.GetById(datItem1.ID);

                // Assert
                Assert.Equal(datItem2.Name, result2.Result.Value.Name);
                Assert.Equal(datItem2.IsComplete, result2.Result.Value.IsComplete);
                Assert.Equal(datItem2.DatListID, result2.Result.Value.DatListID);
            }
        }
    }
}
