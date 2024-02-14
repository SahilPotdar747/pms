using Kemar.TMS.Business.Interfaces;
using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Business.BusinessLogic
{
    public class CheckTaskStatusBusinessLogic : ICheckTaskStatus
    {
        private readonly ITaskTransaction _taskTransaction;
        private readonly INotification _notification;

        public CheckTaskStatusBusinessLogic(ITaskTransaction taskTransaction, INotification notification)
        {
            _taskTransaction = taskTransaction;
            _notification = notification;
        }

        public async Task StartServiceAsync()
        {
            try
            {
                await CheckTaskStatus();
                await UpcommingTaskNotification();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CheckTaskStatus()
        {
            try
            {
                var NowDateTime = DateTime.Now;
                var newTaskList = await _taskTransaction.GetNotStartedTask();
                foreach (var item in newTaskList)
                {
                    if (item.ExceptedStartDate >= DateTime.Now && item.ExceptedEndDate > NowDateTime && item.Status != TaskTransactionStatus.Pending)
                    {
                        item.Status = TaskTransactionStatus.Pending;
                        await _taskTransaction.updateTaskStatus(item);
                    }
                    else if (item.ExceptedEndDate <= NowDateTime)
                    {
                        item.Status = TaskTransactionStatus.Overdue;
                        await _taskTransaction.updateTaskStatus(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpcommingTaskNotification()
        {
            try
            {
                var upcommingTask = await _taskTransaction.GetUpcommingTaskforNotification();
                foreach (var item in upcommingTask)
                {
                    NotificationRequest notification = new NotificationRequest();
                    notification.Title = NotificationTitle.UpcommingTask;
                    notification.Message = item.Title + " excepted start time " + item.ExceptedStartDate;
                    notification.UserId = (int)item.AssignedTo;
                    notification.CreatedBy = "Service";
                    notification.CreatedDate = DateTime.Now;

                    await _notification.AddNewNotification(notification);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}