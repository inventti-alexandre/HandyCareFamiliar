﻿namespace HandyCareFamiliar.Services
{
    public interface ILocalNotifications
    {
        void SendLocalNotification(string title, string description, long time);
    }
}