using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Common;
using WpfApp1.DataAccess.DataEntity;
using WpfApp1.Model;

namespace WpfApp1.DataAccess
{
    public class LocalDataAccess
    {
        private static LocalDataAccess instance;
        private LocalDataAccess() { }
        public static LocalDataAccess GetInstance()
        {
            return instance ?? (instance = new LocalDataAccess());
        }
        SqlConnection conn;
        SqlCommand comm;
        SqlDataAdapter adapter;
        // 资源关闭
        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
        }
        // 连接数据库
        private void DBConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            if (conn == null)
                conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // 检查用户名和密码
        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            DBConnection();
            try
            {
                if (conn != null)
                {
                    string sql = "select * from users where user_name=@user_name and password=@pwd and is_validation=1";
                    adapter = new SqlDataAdapter(sql, conn);
                    // 参数设置
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = userName });
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@pwd", SqlDbType.VarChar) 
                    { Value = MD5Provider.GetMD5(pwd+"@"+userName) });
                    DataTable dataTable = new DataTable();
                    if (adapter.Fill(dataTable) == 0)
                        throw new Exception("用户名或密码错误！");
                    DataRow dr = dataTable.Rows[0];
                    if (dr.Field<int>("is_can_login") == 0)
                        throw new Exception("当前用户权限不足！");
                    UserEntity userInfo = new UserEntity();
                    userInfo.UserName = dr.Field<string>("user_name");
                    userInfo.RealName = dr.Field<string>("real_name");
                    userInfo.Password = dr.Field<string>("password");
                    userInfo.Avatar = dr.Field<string>("avatar");
                    userInfo.Gender = dr.Field<int>("gender");
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
            return null;
        }
        // 课程数据
        public List<CoursServicesModel> GetCoursePlayRecord()
        {
            DBConnection();
            try
            {
                List<CoursServicesModel> cModelList = new List<CoursServicesModel>();
                if (conn != null)
                {
                    string sql = @"select 
                                                a.course_name, a.course_id, 
	                                            b.play_count, b.is_growing, b.growing_rate,
	                                            c.platform_name
                                            from courses a
                                            join play_record b
                                            on a.course_id = b.course_id
                                            join platforms c
                                            on b.platform_id = c.platform_id
                                            order by a.course_id, c.platform_id; ";
                    adapter = new SqlDataAdapter(sql, conn);
                    DataTable dataTable = new DataTable();
                    int count = adapter.Fill(dataTable);

                    string courseId = "";
                    CoursServicesModel cModel = null;
                    foreach(DataRow dr in dataTable.AsEnumerable())
                    {
                        string tempId = dr.Field<string>("course_id");
                        if (courseId != tempId)
                        {
                            courseId = tempId;
                            cModel = new CoursServicesModel();
                            cModelList.Add(cModel);
                            cModel.CourseName = dr.Field<string>("course_name");
                            cModel.SeriesCollection = new LiveCharts.SeriesCollection();
                            cModel.SeriesList = new ObservableCollection<SeriesModel>();
                        }
                        if (cModel != null)
                        {
                            cModel.SeriesCollection.Add(new PieSeries
                            {
                                Title = dr.Field<string>("platform_name"),
                                Values = new ChartValues<ObservableValue> { new ObservableValue((double)dr.Field<decimal>("play_count") )},
                                DataLabels = false
                            });
                            cModel.SeriesList.Add(new SeriesModel
                            {
                                SeriesName = dr.Field<string>("platform_name"),
                                CurrentValue = dr.Field<decimal>("play_count"),
                                IsGrowing = dr.Field<int>("is_growing") == 1,
                                ChangeRate = (int)dr.Field<decimal>("growing_rate")
                            }); ; ;
                        }
                    }
                }
                return cModelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        // 查询老师名称
        public List<string> GetTeachers()
        {
            List<string> result = new List<string>();
            DBConnection();
            try
            {
                if (conn != null)
                {
                    string sql = "select real_name from users where is_teacher = 1";
                    adapter = new SqlDataAdapter(sql, conn);
                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);
                    if (count > 0)
                        result = table.AsEnumerable().Select(c => c.Field<string>("real_name")).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
    }
}
