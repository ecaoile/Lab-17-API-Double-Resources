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
    public class TodoListTest
    {
        // 5. Create a List
        [Fact]
        public void CanCreateList()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("DatDatabase").Options;

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

        // 6. Read a List
        [Fact]
        public void CanReadTodoList()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 3;
                datList1.Name = "house chores";

                TodoList datList2 = new TodoList();
                datList2.ID = 4;
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

        // 7. Update a List
        [Fact]
        public void CanUpdateTodoList()
        {
            DbContextOptions<TodoDbContext> options = new 
                DbContextOptionsBuilder
                <TodoDbContext>()
                .UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 5;
                datList1.Name = "house chores";

                // note: ID must be the same in order to update
                TodoList datList2 = new TodoList();
                datList2.ID = 5;
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

        // 8. Delete a list
        [Fact]
        public void CanDeleteTodoList()
        {
            DbContextOptions<TodoDbContext> options = new DbContextOptionsBuilder<TodoDbContext>().UseInMemoryDatabase("DatDatabase").Options;

            using (TodoDbContext context = new TodoDbContext(options))
            {
                // Arrange
                TodoList datList1 = new TodoList();
                datList1.ID = 3;
                datList1.Name = "house chores";

                TodoList datList2 = new TodoList();
                datList2.ID = 4;
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
    }
}
