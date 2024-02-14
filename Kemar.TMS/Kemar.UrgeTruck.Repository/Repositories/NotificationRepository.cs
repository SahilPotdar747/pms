using AutoMapper;
using Kemar.TMS.Domain.RequestModel;
using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Interface;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Kemar.UrgeTruck.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.Repositories
{
    public class NotificationRepository : INotification
    {
        private readonly IKUrgeTruckContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public NotificationRepository(IKUrgeTruckContextFactory contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddNewNotification(NotificationRequest notification)
        {
            var resMessage = "Notification ";
            try
            {
                using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
                if (notification.CreatedBy == null)
                    notification.CreatedBy = "System";
                if (notification.CreatedDate == null)
                    notification.CreatedDate = DateTime.Now;
                kUrgeTruckContext.Notification.Add(_mapper.Map<Notification>(notification));
                resMessage = resMessage + UrgeTruckMessages.added_successfully;

                await kUrgeTruckContext.SaveChangesAsync();
                return ResultModelFactory.UpdateSucess(resMessage);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while register Notification " + ex);
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, UrgeTruckMessages.Error_while_addupdate_Notification, ex);
            }
        }

        public async Task<List<NotificationResponse>> GetMyNotification(int Userid)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var notification = await kUrgeTruckContext.Notification.OrderByDescending(x => x.NotificationId).Where(x => x.UserId == Userid && !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<NotificationResponse>>(notification);
        }

        public async Task<int> GetMyNotificationCount(int Userid)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var notification = await  kUrgeTruckContext.Notification.Where(x => x.UserId == Userid && !x.IsDeleted).CountAsync();
            return notification;
        }

        public async Task<NotificationCount> GetMyNotificationCountAsync(int Userid)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            NotificationCount countValue = new NotificationCount();
            countValue.Count = await kUrgeTruckContext.Notification.Where(x => x.UserId == Userid && !x.IsDeleted).CountAsync();
            return countValue;
        }

        public async Task<List<NotificationResponse>> GetCurrentNotifications(int Userid, int count)
        {
            using KUrgeTruckContext kUrgeTruckContext= _contextFactory.CreateKGASContext(); 
            var notificationCount = await kUrgeTruckContext.Notification.Where(x=>x.UserId == Userid && !x.IsDeleted).Skip(count).ToListAsync();
            return _mapper.Map<List<NotificationResponse>>(notificationCount);
        }

        public async Task<ResultModel> CloseNotificationByUser(int notificationId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var deleteNotification = await kUrgeTruckContext.Notification.Where(x => x.NotificationId == notificationId).FirstOrDefaultAsync();
            if (deleteNotification != null)
            {
                deleteNotification.IsDeleted = true;
                kUrgeTruckContext.Notification.Update(deleteNotification);
            }
            await kUrgeTruckContext.SaveChangesAsync();
            return ResultModelFactory.UpdateSucess();
        }

        public async Task<List<NotificationResponse>> GetNotificationForUIAlert(int UserId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var notificationCount = await kUrgeTruckContext.Notification.Where(x => x.UserId == UserId && !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<NotificationResponse>>(notificationCount);
        }

        public async Task<List<NotificationResponse>> GetDesktopNotifications(int Userid)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var notificationCount = await kUrgeTruckContext.Notification.Where(x => x.UserId == Userid && !x.PushedToDesktop && !x.IsDeleted).ToListAsync();
            foreach(var item in notificationCount)
            {
                item.PushedToDesktop = true;
                kUrgeTruckContext.Notification.Update(item);
            }
            if (notificationCount.Count > 0)
                await kUrgeTruckContext.SaveChangesAsync();
            return _mapper.Map<List<NotificationResponse>>(notificationCount);
        }

        public async Task<List<NotificationResponse>> GetMobileNotifications(int Userid)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var notificationCount = await kUrgeTruckContext.Notification.Where(x => x.UserId == Userid && !x.PushedToMobile && !x.IsDeleted).ToListAsync();
            return _mapper.Map<List<NotificationResponse>>(notificationCount);
        }

        public async Task<ResultModel> CloseNotificationByUserForDesktop(int notificationId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var deleteNotification = await kUrgeTruckContext.Notification.Where(x => x.NotificationId == notificationId).FirstOrDefaultAsync();
            if (deleteNotification != null)
            {
                deleteNotification.PushedToDesktop = true;
                kUrgeTruckContext.Notification.Update(deleteNotification);
                await kUrgeTruckContext.SaveChangesAsync();
            }
            return ResultModelFactory.UpdateSucess();
        }

        public async Task<ResultModel> CloseNotificationByUserForMobile(int notificationId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var deleteNotification = await kUrgeTruckContext.Notification.Where(x => x.NotificationId == notificationId).FirstOrDefaultAsync();
            if (deleteNotification != null)
            {
                deleteNotification.PushedToMobile = true;
                kUrgeTruckContext.Notification.Update(deleteNotification);
                await kUrgeTruckContext.SaveChangesAsync();
            }
            return ResultModelFactory.UpdateSucess();
        }

        public async Task<ResultModel> CloseAllNotificationByUser(int userId)
        {
            using KUrgeTruckContext kUrgeTruckContext = _contextFactory.CreateKGASContext();
            var deleteNotification = await kUrgeTruckContext.Notification.Where(x => x.UserId == userId).ToListAsync();
            foreach (var item in deleteNotification)
            {
                item.IsDeleted = true;
                kUrgeTruckContext.Notification.Update(item);
            }
            if (deleteNotification.Count > 0)
                await kUrgeTruckContext.SaveChangesAsync();
            return ResultModelFactory.UpdateSucess();
        }
    }
}
