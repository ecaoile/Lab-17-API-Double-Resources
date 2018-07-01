using Microsoft.EntityFrameworkCore;
using System;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;
using Xunit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;

namespace XUnitTestTodoApi
{
    public class TodoListTest
    {
        /// <summary>
        /// tests the following: 5. Create a List
        /// </summary>
        [Fact]
        public void CanCreateList()
        {
            // Note: database names need to be different or else
            // there will be issues with keys already being taken
            // during testing. I thought each method would be isolated,
            // but they're apparently connected if the database names
            // in memory are the the same.
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test5Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoListController lc = new TodoListController(context);
                TodoList datList = new TodoList();

                // Act
                var result = lc.Create(datList);
                var answer = result.Result;
                var x = (ObjectResult)answer;

                // Assert
                Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)x.StatusCode);
            }
        }

        /// <summary>
        /// tests the following: 6. Read a List
        /// </summary>
        [Fact]
        public void CanReadTodoList()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test6Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 1;
                datList1.Name = "house chores";

                TodoList datList2 = new TodoList();
                datList2.ID = 2;
                datList2.Name = "errands";

                TodoListController lc = new TodoListController(context);

                // Act
                var created1 = lc.Create(datList1);
                var created2 = lc.Create(datList2);

                var result1 = lc.GetById(datList1.ID);
                var result2 = lc.GetById(datList2.ID);

                // Assert
                Assert.Equal(datList1.ID, result1.Result.Value.ID);
                Assert.Equal(datList2.ID, result2.Result.Value.ID);
            }
        }

        /// <summary>
        /// tests the following: 7. Update a List
        /// </summary>
        [Fact]
        public void CanUpdateTodoList()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder
                <TodoDbContext>()
                .UseInMemoryDatabase("Test7database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 1;
                datList1.Name = "house chores";

                // note: ID must be the same in order to update
                TodoList datList2 = new TodoList();
                datList2.ID = 1;
                datList2.Name = "omg dem house chores doe";

                TodoListController lc = new TodoListController(context);

                // Act
                var create1 = lc.Create(datList1);

                var result1 = lc.Update(datList1.ID, datList2);
                var result2 = lc.GetById(datList1.ID);

                // Assert
                Assert.Equal(datList2.Name, result2.Result.Value.Name);
            }
        }

        /// <summary>
        /// tests the following: 8. Delete a list
        /// </summary>
        [Fact]
        public void CanDeleteTodoList()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test8Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 1;
                datList1.Name = "house chores";

                TodoList datList2 = new TodoList();
                datList2.ID = 2;
                datList2.Name = "errands";

                TodoListController lc = new TodoListController(context);

                // Act
                var created1 = lc.Create(datList1);
                var created2 = lc.Create(datList2);

                var deletedItem = lc.Delete(datList1.ID);

                var result1 = lc.GetById(datList1.ID);
                var result2 = lc.GetById(datList2.ID);

                // Assert
                Assert.Null(result1.Result.Value);
                Assert.NotNull(result2);
            }
        }

        /// <summary>
        /// tests the following: 9. Add Items to a List
        /// </summary>
        [Fact]
        public async void CanAddItemsToList()
        {
            DbContextOptions<TodoDbContext> options = new
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test9Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 1;
                datList1.Name = "house chores";

                TodoListController lc = new TodoListController(context);

                TodoItem datItem1 = new TodoItem();
                datItem1.ID = 1;
                datItem1.Name = "vacuum the floor";
                datItem1.IsComplete = false;
                datItem1.DatListID = datList1.ID;

                TodoItem datItem2 = new TodoItem();
                datItem2.ID = 2;
                datItem2.Name = "wash the dishes";
                datItem2.IsComplete = true;
                datItem2.DatListID = datList1.ID;

                TodoController ic = new TodoController(context);

                // Act - note: this requires async in order for the
                // result below to include all of the items in the list
                // Otherwise, the result query displays as having no
                // items according to the test even if it actually does.
                var createdList = await lc.Create(datList1);
                var createdItem1 = await ic.Create(datItem1);
                var createdItem2 = await ic.Create(datItem2);


                var result1 = lc.GetById(datList1.ID);

                // Assert
                Assert.Equal(datItem1, result1.Result.Value.TodoItems[0]);
                Assert.Equal(datItem2, result1.Result.Value.TodoItems[1]);
            }
        }

        /// <summary>
        /// tests the following: 10. Remove items from a list
        /// </summary>
        [Fact]
        public async void CanRemoveItemsFromList()
        {
            DbContextOptions<TodoDbContext> options = new
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("Test10Database").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 1;
                datList1.Name = "house chores";

                TodoListController lc = new TodoListController(context);

                TodoItem datItem1 = new TodoItem();
                datItem1.ID = 30;
                datItem1.Name = "vacuum the floor";
                datItem1.IsComplete = false;
                datItem1.DatListID = datList1.ID;

                TodoItem datItem2 = new TodoItem();
                datItem2.ID = 40;
                datItem2.Name = "wash the dishes";
                datItem2.IsComplete = true;
                datItem2.DatListID = datList1.ID;

                TodoController ic = new TodoController(context);

                // Act - note: this requires async in order for the
                // result below to include all of the items in the list
                // Otherwise, the result query displays as having no
                // items according to the test even if it actually does.
                var createdList = await lc.Create(datList1);
                var createdItem1 = await ic.Create(datItem1);
                var createdItem2 = await ic.Create(datItem2);

                var deletedItem = await ic.Delete(datItem1.ID);

                var result1 = lc.GetById(datList1.ID);
                var listCount = result1.Result.Value.TodoItems.Count();
                var todoInList = result1.Result.Value.TodoItems[0].Name;
                
                // Assert
                Assert.Equal(1, listCount);
                Assert.Equal(datItem2.Name, todoInList);
            }
        }
    }
}
