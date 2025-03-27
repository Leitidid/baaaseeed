using System;
using System.Collections.Generic;

namespace baaaseeed;

public partial class Krasavci
{
    public string? Фамилия { get; set; }

    public string? Имя { get; set; }

    public string? Отчество { get; set; }

    public string? НазваниеКоманды { get; set; }

    public DateOnly? ДатаПриемаВКоманду { get; set; }

    public int? ЧислоЗаброшенныхШайб { get; set; }

    public int? КоличествоГолевыхПередач { get; set; }

    public int? ШтрафноеВремя { get; set; }

    public int? КолмчествоСыгранныхМатчей { get; set; }
}
