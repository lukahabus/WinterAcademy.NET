using GDi.WinterAcademy.Zadatak.Core.Entities;
using static GDi.WinterAcademy.Zadatak.API.Models.NotificationsDTO;

namespace GDi.WinterAcademy.Zadatak.API.Models
{
    public record GetSensorsResponse(
        List<SensorTypeModel> Sensors);

    public record SensorModel
    (
        long? Id,
        float CurrentStatus,
        long TypeId,
        string? TypeDescription,
        float? TypeLowestValueExpected,
        float? TypeHighestValueExpected,
        List<NotificationModel>? Notifications
    );

    public record SensorTypeModel
    (
        long Id,
        string Description,
        float LowestValueExpected,
        float HighestValueExpected
    );

}
