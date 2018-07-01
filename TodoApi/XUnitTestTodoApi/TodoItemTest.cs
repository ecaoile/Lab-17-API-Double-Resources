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
        
        // Note: database names need to have different names in order to
        // avoid conflicts
        /// <summary>
        /// Tests the following: 1. Create a ToDO item
        /// </summary>
        [Fact]
        public void CanCreateTodoItem()
        {

            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test1Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoItem datItem = new TodoItem();
                datItem.ID = 1;
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

        /// <summary>
        /// Tests the following: 2. Read a TODO Item
        /// </summary>
        [Fact]
        public void CanReadTodoItem()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test2Database").Options;

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

        /// <summary>
        /// tests the following: 3. Update a ToDo item
        /// </summary>
        [Fact]
        public void CanUpdateTodoItem()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test3Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoItem datItem1 = new TodoItem();
                datItem1.ID = 1;
                datItem1.Name = "walk the dog";
                datItem1.IsComplete = false;
                datItem1.DatListID = 1;

                // note: ID must be the same in order to update
                TodoItem datItem2 = new TodoItem();
                datItem2.ID = 1;
                datItem2.Name = "pet the dog";
                datItem2.IsComplete = true;
                datItem2.DatListID = 2;

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

        /// <summary>
        /// tests the following: 4. Delete a ToDo item
        /// </summary>
        [Fact]
        public async void CanDeleteTodoItem()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test4Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                // Note: For this test, the ID has to be different
                // than 1 because async is forcing the ic controller 
                // to create a dummy todo with an ID of 1
                TodoItem datItem1 = new TodoItem();
                datItem1.ID = 2;
                datItem1.Name = "walk the dog";
                datItem1.IsComplete = false;

                TodoItem datItem2 = new TodoItem();
                datItem2.ID = 3;
                datItem2.Name = "wash the dishes";
                datItem2.IsComplete = true;

                TodoController ic = new TodoController(context);

                // Act - note: create and delete need to be async
                // in order for the result value to display as null
                // for the deleted playlist
                var created1 =  await ic.Create(datItem1);
                var created2 =  await ic.Create(datItem2);

                var deletedItem = await ic.Delete(datItem1.ID);

                var result1 = ic.GetById(datItem1.ID);
                var result2 = ic.GetById(datItem2.ID);

                // Assert
                Assert.Null(result1.Result.Value);
                Assert.NotNull(result2);
            }
        }
    }
}
