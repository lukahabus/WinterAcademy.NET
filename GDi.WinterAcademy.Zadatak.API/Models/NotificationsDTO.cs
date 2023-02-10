namespace GDi.WinterAcademy.Zadatak.API.Models
{
    public class NotificationsDTO
    {
        public record NotificationModel
        (
            long Id,
            float Status,
            DateTime DateTimeReceived,
            long SensorId
        );
    }
}
