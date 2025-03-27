using System;
using System.Collections.Generic;

namespace baaaseeed;

public partial class Player
{
    public int Id { get; set; }

    public string? Фамилия { get; set; }

    public string? Имя { get; set; }

    public string? Отчество { get; set; }

    public string? НазваниеКоманды { get; set; }

    public DateOnly? ДатаПриема { get; set; }

    public int? ЗаброшенныеШайбы { get; set; }

    public int? ГолевыеПодачи { get; set; }

    public int? ШтрафноеВремя { get; set; }

    public int? СыгранныеМатчи { get; set; }
}
