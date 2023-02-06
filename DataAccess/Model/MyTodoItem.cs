using System;

namespace DataAccess.Model
{
    public class MyTodoItem : MyBaseClass
    {
        public string Title { get; set; } = "Default Title";
        public string Description { get; set; } = "This is the default description";
        public bool IsDone { get; set; } = false;
    }
}
