using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Task.Domain;
using wojilu.ORM;

namespace wojilu.Apps.Task.Service
{
    public class TaskService
    {
        public TaskInfo FindTaskById(int id)
        {
            return cdb.findById<TaskInfo>(id);
        }

        public List<TaskInfo> FindAllTasks()
        {
            List<TaskInfo> tasks = cdb.findAll<TaskInfo>();
            tasks.Sort();
            return tasks;
        }

        public List<TaskInfo> FindTasksByCatgory(int catgoryId)
        {
            List<TaskInfo> tasks = cdb.findBy<TaskInfo>("CatgoryId", catgoryId);
            tasks.Sort();
            return tasks;
        }

        public List<TaskInfo> FindAvaibleTasksByCatgory(int catgoryId)
        {
            List<TaskInfo> tasks = cdb.findBy<TaskInfo>("CatgoryId", catgoryId);
            tasks.Sort();
            return tasks.FindAll(c => c.Enable == 1);
        }

        public List<TaskInfo> FindUnCompletedTasks(int catgoryId, int userId)
        {
            List<TaskInfo> tasks = cdb.findBy<TaskInfo>("CatgoryId", catgoryId);
            tasks.Sort();
            if(tasks.Count == 0)
                return tasks;
            tasks = tasks.FindAll(c => c.Enable == 1);
            string taskIds = string.Empty;
            tasks.ForEach(c => {
                if(c.Day == "forever")
                    taskIds += c.Id + ",";
            });
            taskIds = taskIds.TrimEnd(',');
            int completedTaskId = 0;
            List<int> completedTaskIds = new List<int>();
            if(!string.IsNullOrEmpty(taskIds))
            {
                System.Data.IDataReader reader = db.RunReader<UserTaskLog>(string.Format("select taskId from UserTaskLog where taskId in ({0}) and userId={1}", taskIds, userId));
                while(reader.Read())
                {
                    completedTaskId = reader.GetInt32(0);
                    if(!completedTaskIds.Contains(completedTaskId))
                        completedTaskIds.Add(completedTaskId);
                }
            }
            List<TaskInfo> unCompletedTask = new List<TaskInfo>();
            foreach(TaskInfo task in tasks)
            {
                if(!completedTaskIds.Contains(task.Id))
                    unCompletedTask.Add(task);
            }
            return unCompletedTask;
        }

        public List<TaskInfo> FindCompletedTasks(int userId)
        {
            List<int> completedTaskId = new List<int>();
            int id = 0;
            System.Data.IDataReader reader = db.RunReader<UserTaskLog>(string.Format("select taskId from UserTaskLog where userId={0}", userId));
            while(reader.Read())
            {
                id = reader.GetInt32(0);
                if(!completedTaskId.Contains(id))
                    completedTaskId.Add(id);
            }
            List<TaskInfo> tasks = cdb.findAll<TaskInfo>().FindAll(c => completedTaskId.Contains(c.Id));
            tasks.Sort();
            return tasks;
        }

        public List<TaskCatgory> FindAllCatgorys()
        {
            List<TaskCatgory> catgorys = cdb.findAll<TaskCatgory>();
            catgorys.Sort();
            return catgorys;
        }

        public TaskCatgory FindCatgoryById(int id)
        {
            return cdb.findById<TaskCatgory>(id);
        }

        public UserTaskLog FindLog(int userId, int taskId)
        {
            return UserTaskLog.find("UserId=:uid and taskId=:tid").set("uid", userId).set("tid", taskId).first();
        }

        //todo 性能优化
        public UserTaskLog FindLogByTime(int userId, int taskId, string filter)
        {
            if(filter == "forever")
                return FindLog(userId, taskId);

            EntityInfo ei = Entity.GetInfo(typeof(UserTaskLog));
            String t = ei.Dialect.GetTimeQuote();
            string condition = "select * from " + ei.TableName + "  where UserId=" + userId + " and TaskId=" + taskId;
            String fs = "  and Created between " + t + "{0}" + t + " and " + t + "{1}" + t + "  order by Created desc";
            DateTime now = DateTime.Now;
            if(filter == "today")
               condition +=  string.Format(fs, now.ToShortDateString(), now.AddDays(1).ToShortDateString());
            else if(filter == "week")
                condition +=  string.Format(fs, now.AddDays(-7).ToShortDateString(), now.AddDays(1).ToShortDateString());
            else if(filter == "month")
                condition +=  string.Format(fs, now.AddMonths(-1).ToShortDateString(), now.AddDays(1).ToShortDateString());
            else if(filter == "month3")
               condition += string.Format(fs, now.AddMonths(-3).ToShortDateString(), now.AddDays(1).ToShortDateString());

            List<UserTaskLog> logs = UserTaskLog.findBySql(ei.Dialect.GetLimit(condition, 1));
            if(logs.Count >= 1)
                return logs[0];
            else
                return null;
        }

        /// <summary>
        /// 查找用户几天内的任务记录，按时间降序排列
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <param name="days"></param>
        /// <returns>返回时间降序列表</returns>
        public List<UserTaskLog> FindLogsByDays(int userId, int taskId, int days)
        {
            EntityInfo ei = Entity.GetInfo(typeof(UserTaskLog));
            //String t = ei.Dialect.GetTimeQuote();
            //DateTime now = DateTime.Now;
            //String fs = " Created between " + t + "{0}" + t + " and " + t + "{1}" + t + " ";
            //fs = string.Format(fs, now.AddDays(-days).ToShortDateString(), now.AddDays(1).ToShortDateString());
            string sql = string.Format("select * from {2} where UserId={0} and TaskId={1} order by Created desc",userId,taskId,ei.TableName);
            return UserTaskLog.findBySql(ei.Dialect.GetLimit(sql, days));
        }
    }
}
