using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.DataAccess;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class CoursePageViewModel
    {
        public ObservableCollection<CategoryItemModel> CategoryCourses { get; set; }
        public ObservableCollection<CategoryItemModel> CategoryTechnology { get; set; }
        public ObservableCollection<CategoryItemModel> CategoryTeacher { get; set; }

        public CoursePageViewModel()
        {
            CategoryCourses = new ObservableCollection<CategoryItemModel>();
            CategoryCourses.Add(new CategoryItemModel("全部", true));
            CategoryCourses.Add(new CategoryItemModel("公开课"));
            CategoryCourses.Add(new CategoryItemModel("VIP课"));

            CategoryTechnology = new ObservableCollection<CategoryItemModel>();
            CategoryTechnology.Add(new CategoryItemModel("全部", true));
            CategoryTechnology.Add(new CategoryItemModel("C#/.NET"));
            CategoryTechnology.Add(new CategoryItemModel("ASP.NET"));
            CategoryTechnology.Add(new CategoryItemModel("SQL Server"));

            CategoryTeacher = new ObservableCollection<CategoryItemModel>();
            CategoryTeacher.Add(new CategoryItemModel("全部", true));
            foreach(var item in LocalDataAccess.GetInstance().GetTeachers())
                CategoryTeacher.Add(new CategoryItemModel(item));

        }
    }
}
